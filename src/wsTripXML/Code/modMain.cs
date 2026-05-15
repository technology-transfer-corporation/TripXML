using System;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.UI.WebControls;
using System.Xml;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using PaymentServices;
using TripXML.Core.Models.Base;
using TripXMLMain;
using static TripXMLMain.modCore;


namespace wsTripXML.wsTravelTalk
{

    public static class modMain
    {

        private struct EncodingTables
        {
            public string TableName;
            public string FileName;
            public bool PlainText;
        }

        public enum enLogType
        {
            Request = 1,
            Response = 2
        }

        public const string CVoyageID = "00000";
        public const int CPrdTimeOut = 80;

        #region  Message Request 

        public static void LogResponse(ref string strResponse, ref TravelTalkCredential ttCredential, DateTime StartTime, int ttServiceID, string ServerName, ref string UUID)
        {

            string strResp;

            // If ttServiceID = 6 And strResponse.IndexOf("<Success") <> -1 Then
            // strResp = "<OTA_AirLowFareSearchRS><Success/></OTA_AirLowFareSearchRS>"
            // ElseIf ttServiceID = 7 And strResponse.IndexOf("<Success") <> -1 Then
            // strResp = "<OTA_AirLowFareSearchPlusRS><Success/></OTA_AirLowFareSearchPlusRS>"
            // ElseIf ttServiceID = 68 And strResponse.IndexOf("<Success") <> -1 Then
            // strResp = "<OTA_AirLowFareSearchScheduleRS><Success/></OTA_AirLowFareSearchScheduleRS>"
            // Else
            // strResp = strResponse
            // End If

            strResp = strResponse;

            cLog oLog;
            // Dim startCounter As Date
            var sb = new StringBuilder();

            try
            {
                if (ttServiceID != 2 & ttServiceID != 6 & ttServiceID != 7 & ttServiceID != 24 & ttServiceID != 25 & ttServiceID != 81 & ttServiceID != 85)
                {
                    // startCounter = Now
                    oLog = new cLog();
                    oLog.LogResponse(UUID, ref ServerName, TravelTalkCredential.RequestorID, ttCredential.UserID, sb.Append(ttCredential.Providers[0].Name).Append(" ").Append(ttCredential.System).ToString(), ttServiceID, ref strResp, StartTime);
                    sb.Remove(0, sb.Length);
                }
            }

            // Just Ignore Log Class will Log Error to Log File
            catch (Exception ex)
            {
            }
            finally
            {
                GC.Collect();
            }

        }

        public static void PreServiceRequest(ref string strRequest, ref HttpApplicationState oApp, ref TravelTalkCredential ttCredential, ref TripXMLProviderSystems ttProviderSystems, DateTime StartTime, int ttServiceID, string ServerName, ref string UUID, string Version = "", bool isDefault = false)
        {
            XmlDocument oDoc;
            bool validateXSDIn;
            cLog oLog;
            bool logged = false;

            try
            {
                if (isDefault)
                {
                    switch (ttServiceID)
                    {
                        case (int)ttServices.PNRReprice:
                            {
                                ttCredential = GetTravelTalkDefaultTravelportCredential(ref strRequest, ttServiceID, ttCredential);
                                break;
                            }
                        case (int)ttServices.PNRRead:
                            {
                                ttCredential = GetTravelTalkDefaultTravelportCredential(ref strRequest, ttServiceID, ttCredential);
                                break;
                            }
                        case (int)ttServices.ShowMileage:
                            {
                                ttCredential = GetTravelTalkDefaultSabreCredential(ref strRequest, ttServiceID, ttCredential);
                                break;
                            }

                        default:
                            {
                                ttCredential = GetTravelTalkDefaultAmadeusCredential(ref strRequest, ttServiceID);
                                break;
                            }
                    }
                }
                else
                {
                    ttCredential = GetTravelTalkCredential(ref strRequest, ttServiceID);
                }

                oDoc = (XmlDocument)oApp.Get("ttACL");
                // validateXSDIn = oApp.Get(sb.Append("XSD").Append(ttCredential.UserID).Append("In").ToString())
                validateXSDIn = Conversions.ToBoolean(oApp.Get($"XSD{ttCredential.UserID}In"));

                // SQL Message Log
                try
                {
                    if (ttServiceID != 81)
                    {
                        oLog = new cLog();
                        // sb.Append(.Providers(0).Name).Append(" ").Append(.System).ToString()
                        UUID = oLog.LogRequest(ServerName, TravelTalkCredential.RequestorID, ttCredential.UserID, $"{ttCredential.Providers[0].Name} {ttCredential.System}", ttServiceID, ref strRequest, StartTime);
                    }
                    logged = true;
                }
                // Just Ignore Log Class will Log Error to Log File
                catch (Exception e)
                {
                }
                finally
                {
                    GC.Collect();
                }

                if (modCore.Trace)
                    CoreLib.SendTrace(ttCredential.UserID, $"ttMain {ttServiceID}", "============= OTA Request ============= ", strRequest, UUID);

                TripXMLTools.TripXMLLoad.GetProviderSystem(ref ttProviderSystems, ttCredential);

                try
                {
                    if (validateXSDIn)
                    {
                        CoreLib.ValidateXML(strRequest, ttServiceID, (int)enSchemaType.Request, ttCredential.UserID, Version);
                    }
                }
                catch (Exception exx)
                {
                    throw new Exception($"Invalid Request. Schema Validation Failed.{Constants.vbNewLine}{exx.Message}");
                }

                // AuthenticateUser(oDoc, ttCredential)
                // ttProviderSystems = oApp.Get(sb.Append("PS").Append(ttCredential.Providers(0).Name).Append(ttCredential.UserID).Append(ttCredential.System).Append(ttCredential.Providers(0).PCC).ToString())

                // ttProviderSystems = oApp.Get($"PS{ttCredential.Providers(0).Name}{ttCredential.UserID}{ttCredential.System}{ttCredential.Providers.First().PCC}")

                ttProviderSystems.LogUUID = UUID;

                if (ttProviderSystems.AmadeusWS == true)
                {
                    ttCredential.Providers[0].Name = "AmadeusWS";

                    if (ttCredential.System == "Test")
                    {
                        ttProviderSystems.URL = "https://test.webservices.amadeus.com";
                    }
                    else if (ttCredential.System == "Training")
                    {
                        ttProviderSystems.URL = "https://production.webservices.amadeus.com";
                    }
                    else
                    {
                        ttProviderSystems.URL = "https://production.webservices.amadeus.com";
                    }
                }

                if (ttCredential.Providers[0].Name != "Amadeus")
                {
                    // ttProviderSystems = oApp.Get(sb.Append("PS").Append(ttCredential.Providers(0).Name).Append(ttCredential.UserID).Append(ttCredential.System).Append(ttCredential.Providers(0).PCC).ToString())
                    // sb.Remove(0, sb.Length())
                    if (ttProviderSystems.System is null)
                    {
                        // $"Access denied to {ttCredential.Providers(0).Name} - {ttCredential.System} system. Or invalid provider or PCC ({ttCredential.Providers(0).PCC})."
                        throw new Exception($"Access denied to {ttCredential.Providers[0].Name} - {ttCredential.System} system. Or invalid provider or PCC ({ttCredential.Providers[0].PCC}).");
                    }
                    if (ttCredential.Providers[0].PCC.Trim().Length > 0 & ttCredential.Providers[0].Name != "Sabre" & ttCredential.Providers[0].Name != "Galileo")
                    {
                        ttProviderSystems.PCC = ttCredential.Providers[0].PCC;
                    }
                }
            }

            catch (Exception ex)
            {
                if (!logged)
                {
                    if (modCore.Trace)
                        CoreLib.SendTrace(ttCredential.UserID, $"ttMain {ttServiceID}", "============= OTA Request ============= ", strRequest, ttProviderSystems.LogUUID);

                    if (UUID is not null)
                    {
                        string argProvider = $"{ttCredential.Providers[0].Name} {ttCredential.System}";
                        LogMessageToFile((int)enLogType.Request, ref UUID, ref ServerName, ref TravelTalkCredential.RequestorID, ref ttCredential.UserID, ref argProvider, ttServiceID, ref strRequest, StartTime, 0, ex.Message);
                    }
                }
                throw;
            }
            finally
            {
                GC.Collect();
            }
        }

        public static void PreServiceRequestPool(ref string strRequest, ref HttpApplicationState oApp, ref TravelTalkCredential ttCredential, ref TripXMLProviderSystems ttProviderSystems, DateTime StartTime, int ttServiceID, string ServerName, ref string UUID, string Version = "")
        {
            XmlDocument oDoc;
            bool validateXSDIn;
            cLog oLog;
            bool logged = false;
            var sb = new StringBuilder();

            try
            {
                ttCredential = GetTravelTalkCredential(ref strRequest, ttServiceID);

                oDoc = (XmlDocument)oApp.Get("ttACL");
                // If oDoc Is Nothing Then
                // Throw New Exception("Failed to find ttACL")
                // End If

                // validateXSDIn = oApp.Get(sb.Append("XSD").Append(ttCredential.UserID).Append("In").ToString())
                validateXSDIn = Conversions.ToBoolean(oApp.Get($"XSD{ttCredential.UserID}In"));
                sb.Remove(0, sb.Length);

                // SQL Message Log
                try
                {

                    oLog = new cLog();
                    if (ttServiceID != 2 & ttServiceID != 6 & ttServiceID != 7 & ttServiceID != 24 & ttServiceID != 25 & ttServiceID != 81 & ttServiceID != 85)
                    {
                        UUID = oLog.LogRequest(ServerName, TravelTalkCredential.RequestorID, ttCredential.UserID, sb.Append(ttCredential.Providers[0].Name).Append(" ").Append(ttCredential.System).ToString(), ttServiceID, ref strRequest, StartTime);
                        sb.Remove(0, sb.Length);
                    }
                    else
                    {
                        // request set to empty to to send data over network for nothing 
                        string argMessage = "";
                        UUID = oLog.LogRequest(ServerName, TravelTalkCredential.RequestorID, ttCredential.UserID, sb.Append(ttCredential.Providers[0].Name).Append(" ").Append(ttCredential.System).ToString(), ttServiceID, ref argMessage, StartTime);
                        sb.Remove(0, sb.Length);
                    }

                    logged = true;
                    ttProviderSystems.AddLog = logged;
                }

                // Just Ignore Log Class will Log Error to Log File
                catch (Exception e)
                {
                }
                finally
                {
                    GC.Collect();
                }

                if (modCore.Trace)
                    CoreLib.SendTrace(ttCredential.UserID, sb.Append("ttMain ").Append(ttServiceID).ToString(), "============= OTA Request ============= ", strRequest, UUID);
                sb.Remove(0, sb.Length);

                try
                {
                    if (validateXSDIn)
                    {
                        CoreLib.ValidateXML(strRequest, ttServiceID, (int)enSchemaType.Request, ttCredential.UserID, Version);
                    }
                }
                catch (Exception exx)
                {
                    throw new Exception(sb.Append("Invalid Request. Schema Validation Failed.").Append(Constants.vbNewLine).Append(exx.Message).ToString());
                }

                TripXMLTools.TripXMLLoad.GetProviderSystem(ref ttProviderSystems, ttCredential);
                ttProviderSystems.LogUUID = UUID;
            }

            // AuthenticateUser(oDoc, ttCredential)
            catch (Exception ex)
            {
                if (!logged)
                {
                    if (modCore.Trace)
                        CoreLib.SendTrace(ttCredential.UserID, sb.Append("ttMain ").Append(ttServiceID).ToString(), "============= OTA Request ============= ", strRequest, UUID);
                    sb.Remove(0, sb.Length);
                    if (UUID is not null)
                    {
                        string argProvider = sb.Append(ttCredential.Providers[0].Name).Append(" ").Append(ttCredential.System).ToString();
                        LogMessageToFile((int)enLogType.Request, ref UUID, ref ServerName, ref TravelTalkCredential.RequestorID, ref ttCredential.UserID, ref argProvider, ttServiceID, ref strRequest, StartTime, 0, ex.Message);
                        sb.Remove(0, sb.Length);
                    }
                }
                throw;
            }
            finally
            {
                GC.Collect();
            }
        }

        public static void PostServiceRequest(ref string strResponse, bool ValidateXSDOut, int ttServiceID, string UserID, string Version = "")
        {
            var sb = new StringBuilder();

            try
            {
                if (ValidateXSDOut)
                {
                    CoreLib.ValidateXML(strResponse, ttServiceID, (int)enSchemaType.Response, UserID, Version);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(sb.Append("Invalid Response. Schema Validation Failed.").Append(Constants.vbNewLine).Append(ex.Message).ToString());
            }
            finally
            {
                GC.Collect();
            }

        }

        public static void ndcPreServiceRequest(ref string strRequest, ref HttpApplicationState oApp, ref TravelTalkCredential ttCredential, ref TripXMLProviderSystems ttProviderSystems, DateTime StartTime, int ttServiceID, string ServerName, ref string UUID, POS POS, string Version = "")
        {
            XmlDocument oDoc = null;
            bool ValidateXSDIn;
            cLog oLog = null;
            bool Logged = false;
            var sb = new StringBuilder();

            try
            {
                ttCredential = ndcGetTravelTalkCredential(ref strRequest, ttServiceID, POS);

                oDoc = (XmlDocument)oApp.Get("ttACL");
                ValidateXSDIn = Conversions.ToBoolean(oApp.Get(sb.Append("XSD").Append(ttCredential.UserID).Append("In").ToString()));
                sb.Remove(0, sb.Length);

                if (modCore.Trace)
                    CoreLib.SendTrace(ttCredential.UserID, sb.Append("ttMain ").Append(ttServiceID).ToString(), "============= OTA Request ============= ", strRequest, ttProviderSystems.LogUUID);
                sb.Remove(0, sb.Length);

                // SQL Message Log
                try
                {

                    oLog = new cLog();
                    if (ttServiceID != 2 & ttServiceID != 6 & ttServiceID != 7 & ttServiceID != 24 & ttServiceID != 25 & ttServiceID != 81 & ttServiceID != 85)
                    {
                        UUID = oLog.LogRequest(ServerName, TravelTalkCredential.RequestorID, ttCredential.UserID, sb.Append(ttCredential.Providers[0].Name).Append(" ").Append(ttCredential.System).ToString(), ttServiceID, ref strRequest, StartTime);
                        sb.Remove(0, sb.Length);
                    }
                    else
                    {
                        // request set to empty to to send data over network for nothing 
                        string argMessage = "";
                        UUID = oLog.LogRequest(ServerName, TravelTalkCredential.RequestorID, ttCredential.UserID, sb.Append(ttCredential.Providers[0].Name).Append(" ").Append(ttCredential.System).ToString(), ttServiceID, ref argMessage, StartTime);
                        sb.Remove(0, sb.Length);
                    }


                    Logged = true;
                }

                // Just Ignore Log Class will Log Error to Log File
                catch (Exception e)
                {
                }
                finally
                {
                    if (oLog is not null)
                    {
                        oLog = null;
                    }
                }

                try
                {
                    if (ValidateXSDIn)
                    {
                        CoreLib.ValidateXML(strRequest, ttServiceID, (int)enSchemaType.Request, ttCredential.UserID, Version);
                    }
                }
                catch (Exception exx)
                {
                    throw new Exception(sb.Append("Invalid Request. Schema Validation Failed.").Append(Constants.vbNewLine).Append(exx.Message).ToString());
                    sb.Remove(0, sb.Length);
                }

                ndcAuthenticateUser(ref oDoc, ttCredential);
            }

            catch (Exception ex)
            {
                if (!Logged)
                {
                    if (modCore.Trace)
                        CoreLib.SendTrace(ttCredential.UserID, sb.Append("ttMain ").Append(ttServiceID).ToString(), "============= OTA Request ============= ", strRequest, ttProviderSystems.LogUUID);
                    sb.Remove(0, sb.Length);
                    if (UUID is not null)
                    {
                        string argProvider = sb.Append(ttCredential.Providers[0].Name).Append(" ").Append(ttCredential.System).ToString();
                        LogMessageToFile((int)enLogType.Request, ref UUID, ref ServerName, ref TravelTalkCredential.RequestorID, ref ttCredential.UserID, ref argProvider, ttServiceID, ref strRequest, StartTime, 0, ex.Message);
                        sb.Remove(0, sb.Length);
                    }
                }
                throw ex;
            }
            finally
            {
                GC.Collect();
            }
            sb = null;
        }

        public static void LogDeals(ref string strRequest, ref string strResponse)
        {

            cLogData oLogData;

            try
            {
                oLogData = new cLogData();
                oLogData.LogDataDeals(strRequest, strResponse);
            }

            // Just Ignore Log Class will Log Error to Log File
            catch (Exception ex)
            {
            }
            finally
            {
                GC.Collect();
            }

        }

        public static string GetDeals(ref string strRequest)
        {

            cLogData oLogData;
            string strResponse = "";

            try
            {
                oLogData = new cLogData();
                strResponse = oLogData.GetDataDeals(strRequest);
            }
            // Just Ignore Log Class will Log Error to Log File
            catch (Exception ex)
            {
            }
            finally
            {
                GC.Collect();
            }

            return strResponse;
        }

        #endregion

        #region  Authentication 

        public static TravelTalkCredential GetTravelTalkDefaultTravelportCredential(ref string strRequest, int ttServiceID, TravelTalkCredential ttOldCredential)
        {
            TravelTalkCredential ttCredential = default;
            XmlDocument oReqDoc;
            XmlElement oRoot;
            XmlNode oNodePOS;
            int i;
            int count;
            bool customErr = false;
            string strError;
            var sb = new StringBuilder();

            strRequest = strRequest.Replace("URL=\"\"", sb.Append("URL=\"").Append(HttpContext.Current.Request.UserHostName).Append("\"").ToString());
            sb.Remove(0, sb.Length);

            try
            {
                oReqDoc = new XmlDocument();
                oReqDoc.LoadXml(strRequest);
                oRoot = oReqDoc.DocumentElement;
            }
            catch (Exception ex)
            {
                throw new Exception(sb.Append("Error Loading Request XML Document.").Append(ex.Message).ToString());
            }

            try
            {
                if (!oRoot.HasChildNodes)
                {
                    customErr = true;
                    throw new Exception(sb.Append("Invalid or empty request.").Append(Constants.vbNewLine).Append(strRequest).ToString());
                }

                oNodePOS = oRoot.SelectSingleNode("POS");

                if (oNodePOS is null)
                {
                    customErr = true;
                    throw new Exception("POS node element is missing or not valid.");
                }

                if (oNodePOS.SelectSingleNode("Source/RequestorID/@ID") is null)
                {
                    customErr = true;
                    throw new Exception("RequestorID is missing or not valid.");
                }

                TravelTalkCredential.RequestorID = oNodePOS.SelectSingleNode("Source/RequestorID").Attributes["ID"].Value;
                ttCredential.System = oNodePOS.SelectSingleNode("TPA_Extensions/Provider/System").InnerText;
                ttCredential.UserID = "Travelport";
                ttCredential.Password = oNodePOS.SelectSingleNode("TPA_Extensions/Provider/Password").InnerText;

                if (ttCredential.UserID == "FlightSite" & !strRequest.Contains("<VendorPref Code=") & (ttServiceID == 6 | ttServiceID == 7))
                {
                    strRequest = strRequest.Replace("<System>", "<Name>Amadeus</Name><System>");
                    oReqDoc.LoadXml(strRequest);
                    oRoot = oReqDoc.DocumentElement;
                    oNodePOS = oRoot.SelectSingleNode("POS");
                }

                count = oNodePOS.SelectNodes("TPA_Extensions/Provider/Name").Count - 1;

                ttCredential.Providers = new modCore.Provider[count + 1];

                var loopTo = count;
                for (i = 0; i <= loopTo; i++)
                    ttCredential.Providers[i].PCC = "";

                ttCredential.Providers[0].PCC = ttOldCredential.Providers[0].PCC;
                ttCredential.Providers[0].Name = "Travelport";

                oNodePOS = oNodePOS.SelectSingleNode("TPA_Extensions/Provider");
            }
            catch (Exception ex)
            {
                if (customErr)
                {
                    strError = ex.Message;
                }
                else
                {
                    strError = "Error Loading User Credentials. POS node is missing or incomplete.";
                }
                throw new Exception(strError);
            }

            return ttCredential;

        }

        public static TravelTalkCredential GetTravelTalkDefaultSabreCredential(ref string request, int ttServiceID, TravelTalkCredential ttOldCredential)
        {
            TravelTalkCredential ttCredential = default;
            XmlDocument oReqDoc;
            XmlElement oRoot;
            XmlNode oNodePOS;
            int i;
            int count;
            bool customErr = false;
            string strError;
            var sb = new StringBuilder();

            request = request.Replace("URL=\"\"", sb.Append("URL=\"").Append(HttpContext.Current.Request.UserHostName).Append("\"").ToString());
            sb.Remove(0, sb.Length);

            try
            {
                oReqDoc = new XmlDocument();
                oReqDoc.LoadXml(request);
                oRoot = oReqDoc.DocumentElement;
            }
            catch (Exception ex)
            {
                throw new Exception(sb.Append("Error Loading Request XML Document.").Append(ex.Message).ToString());
            }

            try
            {
                if (!oRoot.HasChildNodes)
                {
                    customErr = true;
                    throw new Exception(sb.Append("Invalid or empty request.").Append(Constants.vbNewLine).Append(request).ToString());
                }

                oNodePOS = oRoot.SelectSingleNode("POS");

                if (oNodePOS is null)
                {
                    customErr = true;
                    throw new Exception("POS node element is missing or not valid.");
                }

                if (oNodePOS.SelectSingleNode("Source/RequestorID/@ID") is null)
                {
                    customErr = true;
                    throw new Exception("RequestorID is missing or not valid.");
                }

                TravelTalkCredential.RequestorID = oNodePOS.SelectSingleNode("Source/RequestorID").Attributes["ID"].Value;
                ttCredential.System = oNodePOS.SelectSingleNode("TPA_Extensions/Provider/System").InnerText;
                ttCredential.UserID = "Sabre";
                ttCredential.Password = oNodePOS.SelectSingleNode("TPA_Extensions/Provider/Password").InnerText;

                count = oNodePOS.SelectNodes("TPA_Extensions/Provider/Name").Count - 1;

                ttCredential.Providers = new modCore.Provider[count + 1];

                var loopTo = count;
                for (i = 0; i <= loopTo; i++)
                    ttCredential.Providers[i].PCC = "";

                ttCredential.Providers[0].PCC = ttOldCredential.Providers[0].PCC;
                ttCredential.Providers[0].Name = "Sabre";

                oNodePOS = oNodePOS.SelectSingleNode("TPA_Extensions/Provider");
            }
            catch (Exception ex)
            {
                if (customErr)
                {
                    strError = ex.Message;
                }
                else
                {
                    strError = "Error Loading User Credentials. POS node is missing or incomplete.";
                }
                throw new Exception(strError);
            }

            return ttCredential;

        }

        public static TravelTalkCredential GetTravelTalkDefaultAmadeusCredential(ref string strRequest, int ttServiceID)
        {
            TravelTalkCredential ttCredential = default;
            XmlDocument oReqDoc;
            XmlElement oRoot;
            XmlNode oNodePOS;
            int i;
            int count;
            bool customErr = false;
            string strError;
            var sb = new StringBuilder();

            strRequest = strRequest.Replace("URL=\"\"", sb.Append("URL=\"").Append(HttpContext.Current.Request.UserHostName).Append("\"").ToString());
            sb.Remove(0, sb.Length);

            try
            {
                oReqDoc = new XmlDocument();
                oReqDoc.LoadXml(strRequest);
                oRoot = oReqDoc.DocumentElement;
            }
            catch (Exception ex)
            {
                throw new Exception(sb.Append("Error Loading Request XML Document.").Append(ex.Message).ToString());
            }

            try
            {
                if (!oRoot.HasChildNodes)
                {
                    customErr = true;
                    throw new Exception(sb.Append("Invalid or empty request.").Append(Constants.vbNewLine).Append(strRequest).ToString());
                }

                oNodePOS = oRoot.SelectSingleNode("POS");

                if (oNodePOS is null)
                {
                    customErr = true;
                    throw new Exception("POS node element is missing or not valid.");
                }

                if (oNodePOS.SelectSingleNode("Source/RequestorID/@ID") is null)
                {
                    customErr = true;
                    throw new Exception("RequestorID is missing or not valid.");
                }

                TravelTalkCredential.RequestorID = oNodePOS.SelectSingleNode("Source/RequestorID").Attributes["ID"].Value;
                ttCredential.System = oNodePOS.SelectSingleNode("TPA_Extensions/Provider/System").InnerText;
                ttCredential.UserID = "Amadeus";
                ttCredential.Password = oNodePOS.SelectSingleNode("TPA_Extensions/Provider/Password").InnerText;

                if (ttCredential.UserID == "FlightSite" & !strRequest.Contains("<VendorPref Code=") & (ttServiceID == 6 | ttServiceID == 7))
                {
                    strRequest = strRequest.Replace("<System>", "<Name>Amadeus</Name><System>");
                    oReqDoc.LoadXml(strRequest);
                    oRoot = oReqDoc.DocumentElement;
                    oNodePOS = oRoot.SelectSingleNode("POS");
                }

                count = oNodePOS.SelectNodes("TPA_Extensions/Provider/Name").Count - 1;

                ttCredential.Providers = new modCore.Provider[count + 1];

                var loopTo = count;
                for (i = 0; i <= loopTo; i++)
                    ttCredential.Providers[i].PCC = "";

                // If oNodePOS.SelectSingleNode("Source").Attributes("PseudoCityCode") Is Nothing Then

                ttCredential.Providers[0].PCC = "NYC1S211F";
                ttCredential.Providers[0].Name = "Amadeus";


                // For i = 0 To count
                // .Providers(i).Name = "Amadeus"
                // If Not oNodePOS.SelectNodes("Name").Item(i).Attributes("PseudoCityCode") Is Nothing Then
                // If oNodePOS.SelectNodes("Name").Item(i).Attributes("PseudoCityCode").Value.Trim.Length > 0 Then
                // .Providers(i).PCC = oNodePOS.SelectNodes("Name").Item(i).Attributes("PseudoCityCode").Value.Trim
                // End If
                // ElseIf i > 0 Then
                // .Providers(i).PCC = "*" + .Providers(i - 1).PCC
                // End If
                // Next
                oNodePOS = oNodePOS.SelectSingleNode("TPA_Extensions/Provider");
            }

            catch (Exception ex)
            {
                if (customErr)
                {
                    strError = ex.Message;
                }
                else
                {
                    strError = "Error Loading User Credentials. POS node is missing or incomplete.";
                }
                throw new Exception(strError);
            }

            return ttCredential;

        }

        public static TravelTalkCredential GetTravelTalkCredential(ref string strRequest, int ttServiceID)
        {
            TravelTalkCredential ttCredential = default;
            XmlDocument oReqDoc;
            XmlElement oRoot;
            XmlNode oNodePOS;
            int i;
            int count;
            bool customErr = false;
            string strError;

            // strRequest = strRequest.Insert(strRequest.IndexOf("<RequestorID") + 13, " URL=""").Append(HttpContext.Current.Request.UserHostName).Append(""" ")
            strRequest = strRequest.Replace("URL=\"\"", $"URL=\"{HttpContext.Current.Request.UserHostName}\"");

            try
            {
                oReqDoc = new XmlDocument();
                oReqDoc.LoadXml(strRequest);
                oRoot = oReqDoc.DocumentElement;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Loading Request XML Document.{ex.Message}");
            }

            try
            {
                if (!oRoot.HasChildNodes)
                {
                    customErr = true;
                    throw new Exception($"Invalid or empty request.{Environment.NewLine}{strRequest}");
                }

                oNodePOS = oRoot.SelectSingleNode("POS");

                if (oNodePOS is null)
                {
                    customErr = true;
                    throw new Exception("POS node element is missing or not valid.");
                }

                if (oNodePOS.SelectSingleNode("Source/RequestorID/@ID") is null)
                {
                    customErr = true;
                    throw new Exception("RequestorID is missing or not valid.");
                }

                TravelTalkCredential.RequestorID = oNodePOS.SelectSingleNode("Source/RequestorID").Attributes["ID"].Value;
                ttCredential.System = oNodePOS.SelectSingleNode("TPA_Extensions/Provider/System").InnerText;
                ttCredential.UserID = oNodePOS.SelectSingleNode("TPA_Extensions/Provider/Userid").InnerText;
                ttCredential.Password = oNodePOS.SelectSingleNode("TPA_Extensions/Provider/Password").InnerText;

                if (ttCredential.UserID == "FlightSite" & !strRequest.Contains("<VendorPref Code=") & (ttServiceID == 6 | ttServiceID == 7))
                {
                    strRequest = strRequest.Replace("<System>", "<Name>Amadeus</Name><System>");
                    oReqDoc.LoadXml(strRequest);
                    oRoot = oReqDoc.DocumentElement;
                    oNodePOS = oRoot.SelectSingleNode("POS");
                }

                count = oNodePOS.SelectNodes("TPA_Extensions/Provider/Name").Count - 1;

                ttCredential.Providers = new modCore.Provider[count + 1];

                var loopTo = count;
                for (i = 0; i <= loopTo; i++)
                    ttCredential.Providers[i].PCC = "";

                if (oNodePOS.SelectSingleNode("Source").Attributes["PseudoCityCode"] is null)
                {
                    ttCredential.Providers[0].PCC = "";
                }
                else
                {
                    ttCredential.Providers[0].PCC = oNodePOS.SelectSingleNode("Source").Attributes["PseudoCityCode"].Value;
                }

                oNodePOS = oNodePOS.SelectSingleNode("TPA_Extensions/Provider");

                var loopTo1 = count;
                for (i = 0; i <= loopTo1; i++)
                {
                    ttCredential.Providers[i].Name = oNodePOS.SelectNodes("Name").Item(i).InnerText;
                    if (oNodePOS.SelectNodes("Name").Item(i).Attributes["PseudoCityCode"] is not null)
                    {
                        if (oNodePOS.SelectNodes("Name").Item(i).Attributes["PseudoCityCode"].Value.Trim().Length > 0)
                        {
                            ttCredential.Providers[i].PCC = oNodePOS.SelectNodes("Name").Item(i).Attributes["PseudoCityCode"].Value.Trim();
                        }
                    }
                    else if (i > 0)
                    {
                        ttCredential.Providers[i].PCC = "*" + ttCredential.Providers[i - 1].PCC;
                    }
                }
            }

            catch (Exception ex)
            {
                if (customErr)
                {
                    strError = ex.Message;
                }
                else
                {
                    strError = "Error Loading User Credentials. POS node is missing or incomplete.";
                }
                throw new Exception(strError);
            }

            return ttCredential;

        }

        public static TravelTalkCredential ndcGetTravelTalkCredential(ref string strRequest, int ttServiceID, POS POS)
        {
            TravelTalkCredential ttCredential = default;
            // Dim oReqDoc As XmlDocument = Nothing
            // Dim oRoot As XmlElement = Nothing
            // Dim oNodePOS As XmlNode = Nothing
            int i;
            int Count;
            bool CustomErr = false;
            string strError = "";
            var sb = new StringBuilder();

            // strRequest = strRequest.Insert(strRequest.IndexOf("<RequestorID") + 13, " URL=""").Append(HttpContext.Current.Request.UserHostName).Append(""" ")
            strRequest = strRequest.Replace("URL=\"\"", sb.Append("URL=\"").Append(HttpContext.Current.Request.UserHostName).Append("\"").ToString());
            sb.Remove(0, sb.Length);

            // Try
            // oReqDoc = New XmlDocument
            // oReqDoc.LoadXml(strRequest)
            // oRoot = oReqDoc.DocumentElement
            // Catch ex As Exception
            // CustomErr = True
            // Throw New Exception(sb.Append("Error Loading Request XML Document.").Append(ex.Message).ToString())
            // sb.Remove(0, sb.Length())
            // End Try

            try
            {
                // If Not oRoot.HasChildNodes Then
                // CustomErr = True
                // Throw New Exception(sb.Append("Invalid or empty request.").Append(vbNewLine).Append(strRequest).ToString())
                // sb.Remove(0, sb.Length())
                // End If

                // oNodePOS = oRoot.SelectSingleNode("POS")

                // If oNodePOS Is Nothing Then
                // CustomErr = True
                // Throw New Exception("POS node element is missing or not valid.")
                // End If

                // If oNodePOS.SelectSingleNode("Source/RequestorID/@ID") Is Nothing Then
                // CustomErr = True
                // Throw New Exception("RequestorID is missing or not valid.")
                // End If


                if (POS.Source.First()?.RequestorID.ID is not null)
                {
                    TravelTalkCredential.RequestorID = POS.Source.First().RequestorID.ID.ToString();
                }

                if (POS.TPA_Extensions.Provider.GDSSystem is not null)
                {
                    ttCredential.System = POS.TPA_Extensions.Provider.GDSSystem.ToString();
                }

                if (POS.TPA_Extensions.Provider.Userid is not null)
                {
                    ttCredential.UserID = POS.TPA_Extensions.Provider.Userid.ToString();
                }

                if (POS.TPA_Extensions.Provider.Password is not null)
                {
                    ttCredential.Password = POS.TPA_Extensions.Provider.Password.ToString();
                }

                // .RequestorID = oNodePOS.SelectSingleNode("Source/RequestorID").Attributes("ID").Value
                // .System = oNodePOS.SelectSingleNode("TPA_Extensions/Provider/System").InnerText
                // .UserID = oNodePOS.SelectSingleNode("TPA_Extensions/Provider/Userid").InnerText
                // .Password = oNodePOS.SelectSingleNode("TPA_Extensions/Provider/Password").InnerText

                Count = POS.TPA_Extensions.Provider.Name.Length;

                ttCredential.Providers = new modCore.Provider[Count];

                // For i = 0 To Count
                // .Providers(i).PCC = ""
                // Next

                // If POS.Source.PseudoCityCode Is Nothing Then
                // .Providers(0).PCC = ""
                // Else
                ttCredential.Providers[0].PCC = POS.Source.First().PseudoCityCode.ToString();
                // End If

                // oNodePOS = oNodePOS.SelectSingleNode("TPA_Extensions/Provider")

                var loopTo = Count - 1;
                for (i = 0; i <= loopTo; i++)
                {
                    ttCredential.Providers[i].Name = POS.TPA_Extensions.Provider.Name[i].Value;
                    if (POS.TPA_Extensions.Provider.Name[i].PseudoCityCode is not null)
                    {
                        if (!string.IsNullOrEmpty(POS.TPA_Extensions.Provider.Name[i].PseudoCityCode.ToString()))
                        {
                            ttCredential.Providers[i].PCC = POS.TPA_Extensions.Provider.Name[i].PseudoCityCode.ToString();
                        }
                        // ElseIf i > 0 Then
                        // .Providers(i).PCC = "*" + .Providers(i - 1).PCC
                    }

                    // For i = 0 To Count
                    // .Providers(i).Name = oNodePOS.SelectNodes("Name").Item(i).InnerText
                    // If Not oNodePOS.SelectNodes("Name").Item(i).Attributes("PseudoCityCode") Is Nothing Then
                    // If oNodePOS.SelectNodes("Name").Item(i).Attributes("PseudoCityCode").Value.Trim.Length > 0 Then
                    // .Providers(i).PCC = oNodePOS.SelectNodes("Name").Item(i).Attributes("PseudoCityCode").Value.Trim
                    // End If
                    // ElseIf i > 0 Then
                    // .Providers(i).PCC = "*" + .Providers(i - 1).PCC
                    // End If
                    // Next
                }
            }

            catch (Exception ex)
            {
                if (CustomErr)
                {
                    strError = ex.Message;
                }
                else
                {
                    strError = "Error Loading User Credentials. POS node Is missing Or incomplete.";
                }
                throw new Exception(strError);
            }
            sb = null;
            return ttCredential;

        }

        public static void AuthenticateUser(ref XmlDocument oDoc, TravelTalkCredential ttCredential)
        {
            XmlElement oRoot;
            XmlNode oNode;
            var sb = new StringBuilder();

            oRoot = oDoc.DocumentElement;

            oNode = oRoot.SelectSingleNode(sb.Append("Customer[@RequestorID='").Append(TravelTalkCredential.RequestorID).Append("']").ToString());
            sb.Remove(0, sb.Length);

            if (oNode is null)
            {
                throw new Exception(sb.Append("Customer ").Append(TravelTalkCredential.RequestorID).Append(" is not valid.").ToString());
            }
            else
            {
                oNode = oNode.SelectSingleNode(sb.Append("User[Username='").Append(ttCredential.UserID).Append("']").ToString());
                sb.Remove(0, sb.Length);
                if (oNode is null)
                {
                    throw new Exception(sb.Append("User  ").Append(ttCredential.UserID).Append(" is not valid for Customer ").Append(TravelTalkCredential.RequestorID).ToString());
                }
            }

            if (Strings.StrComp(ttCredential.Password, oNode["Password"].InnerText) != 0)
            {
                throw new Exception(sb.Append("Password ").Append(ttCredential.Password).Append(" is not valid for User ").Append(ttCredential.UserID).ToString());
            }
            else if (DateAndTime.DateDiff(DateInterval.Day, Conversions.ToDate(oNode.SelectSingleNode("Services/Start").InnerText), DateTime.Today) < 0L)
            {
                throw new Exception(sb.Append("Access Denied. Services will start on ").Append(oNode.SelectSingleNode("Services/Start").InnerText).ToString());
            }
            else if (DateAndTime.DateDiff(DateInterval.Day, Conversions.ToDate(oNode.SelectSingleNode("Services/End").InnerText), DateTime.Today) > 0L)
            {
                throw new Exception(sb.Append("Access Denied. Services expired on ").Append(oNode.SelectSingleNode("Services/End").InnerText).ToString());
            }
        }

        public static void ndcAuthenticateUser(ref XmlDocument oDoc, TravelTalkCredential ttCredential)
        {
            XmlElement oRoot = null;
            XmlNode oNode = null;
            var sb = new StringBuilder();

            oRoot = oDoc.DocumentElement;
            oNode = oRoot.SelectSingleNode(sb.Append("Customer[@RequestorID='").Append(TravelTalkCredential.RequestorID).Append("']").ToString());
            sb.Remove(0, sb.Length);

            if (oNode is null)
            {
                throw new Exception(sb.Append("Customer ").Append(TravelTalkCredential.RequestorID).Append(" is not valid.").ToString());
                sb.Remove(0, sb.Length);
            }
            else
            {
                oNode = oNode.SelectSingleNode(sb.Append("User[Username='").Append(ttCredential.UserID).Append("']").ToString());
                sb.Remove(0, sb.Length);
                if (oNode is null)
                {
                    throw new Exception(sb.Append("User  ").Append(ttCredential.UserID).Append(" is not valid for Customer ").Append(TravelTalkCredential.RequestorID).ToString());
                    sb.Remove(0, sb.Length);
                }
            }

            if (Strings.StrComp(ttCredential.Password, oNode["Password"].InnerText) != 0)
            {
                throw new Exception(sb.Append("Password ").Append(ttCredential.Password).Append(" is not valid for User ").Append(ttCredential.UserID).ToString());
                sb.Remove(0, sb.Length);
            }
            else if (DateAndTime.DateDiff(DateInterval.Day, Conversions.ToDate(oNode.SelectSingleNode("Services/Start").InnerText), DateTime.Today) < 0L)
            {
                throw new Exception(sb.Append("Access Denied. Services will start on ").Append(oNode.SelectSingleNode("Services/Start").InnerText).ToString());
                sb.Remove(0, sb.Length);
            }
            else if (DateAndTime.DateDiff(DateInterval.Day, Conversions.ToDate(oNode.SelectSingleNode("Services/End").InnerText), DateTime.Today) > 0L)
            {
                throw new Exception(sb.Append("Access Denied. Services expired on ").Append(oNode.SelectSingleNode("Services/End").InnerText).ToString());
                sb.Remove(0, sb.Length);
            }
        }

        #endregion

        #region  Get Decode Values 

        public static string GetDecodeValue(ref DataView oDV, ref string strCode)
        {
            try
            {
                if (oDV is null)
                {
                    return string.Empty;
                }
                foreach (DataRow row in oDV.Table.Rows)
                {
                    if (row["Code"].ToString().Trim().ToUpper().Equals(strCode.Trim().ToUpper()))
                    {
                        return row["Name"].ToString();
                    }
                    if (row["Code"].ToString().Trim().ToUpper().Contains(strCode.Trim().ToUpper()))
                    {
                        return row["Name"].ToString();
                    }
                }

                // AK: This has to be perform if nothing were found. 
                var elems = strCode.Split(' ').ToList();

                foreach (string word in elems)
                {
                    foreach (DataRow row in oDV.Table.Rows)
                    {
                        if (row["Code"].ToString().Trim().ToUpper().Contains(strCode.Trim().ToUpper()))
                        {
                            return row["Name"].ToString();
                        }
                    }
                }

                return string.Empty;
            }

            // Dim i As Integer
            // i = oDV.Find(strCode)
            // If i > -1 Then
            // Return oDV.Item(i).Item("Name").ToString
            // Else
            // Return ""
            // End If

            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        public static string GetCodeValue(ref DataView oDV, ref string strID)
        {
            try
            {
                foreach (DataRow row in oDV.Table.Rows)
                {
                    if (row["ID"].ToString().Trim().ToUpper().Equals(strID.Trim().ToUpper()))
                    {
                        return row["Code"].ToString();
                    }
                    if (row["ID"].ToString().Trim().ToUpper().Contains(strID.Trim().ToUpper()))
                    {
                        return row["Code"].ToString();
                    }
                }

                return string.Empty;
            }

            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        public static string GetEncodeValue(ref DataView oDV, ref string strName)
        {
            try
            {
                if (string.IsNullOrEmpty(strName))
                {
                    return string.Empty;
                }

                if (strName.Contains("OPERATED BY"))
                {
                    strName = strName.Replace("OPERATED BY ", "");
                }

                // Search all for exact name
                foreach (DataRow row in oDV.Table.Rows)
                {
                    if (row["Name"].ToString().Trim().ToUpper().Equals(strName.Trim().ToUpper()))
                    {
                        return row["Code"].ToString();
                    }
                }
                // Search all for similar name
                foreach (DataRow row in oDV.Table.Rows)
                {
                    if (row["Name"].ToString().Trim().ToUpper().Contains(strName.Trim().ToUpper()))
                    {
                        return row["Code"].ToString();
                    }
                }

                // ******************************
                // Try to cut Airline Name
                // Example: Trans American Airlines F Ta
                // But we need to look only at: Trans American Airlines
                // ******************************
                strName = strName.ToUpper().Replace("AIR LINES", "AIRLINES");

                string[] lstAirName = strName.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                string shortName = string.Empty;

                // In case if Code already in the RTSVI line.
                // 0123456789012345678901234567890123456789
                // 0         1         2         3  
                // Azul Linhas Aereas Brasil Ad 2924
                // - AD is an airline code
                // AEROLITORAL DBA AEROMEXIC AM
                // - AM is an airline code
                // SKYWEST AS AMERICAN EAGLE
                // SKYWEST should be used

                if (strName.ToUpper().Contains(" AS "))
                {

                    foreach (string word in lstAirName)
                    {
                        if (word.Equals("AS"))
                        {
                            break;
                        }

                        foreach (DataRow row in oDV.Table.Rows)
                        {
                            if (row["Name"].ToString().Trim().ToUpper().Equals(word.Trim().ToUpper()))
                            {
                                return row["Code"].ToString();
                            }

                            if (row["Name"].ToString().Trim().ToUpper().Contains(word.Trim().ToUpper()))
                            {
                                return row["Code"].ToString();
                            }
                        }
                    }
                }
                else
                {
                    int lastIndex = lstAirName.Count() - 1;

                    if (Information.IsNumeric(lstAirName.Last()) && lstAirName[lastIndex - 1].Length.Equals(2))
                    {
                        return lstAirName[lastIndex - 1];
                    }

                    if (!Information.IsNumeric(lstAirName[lastIndex]) && lstAirName[lastIndex].Length.Equals(2))
                    {
                        return lstAirName.Last();
                    }

                }



                foreach (string word in lstAirName)
                {
                    if (Information.IsNumeric(word))
                    {
                        continue;
                    }
                    if (word.Length > 2)
                    {
                        shortName += string.Format(" {0}", word);
                    }
                    if (word.ToUpper().Equals("AIRLINES"))
                    {
                        break;
                    }
                }

                foreach (DataRow row in oDV.Table.Rows)
                {
                    if (row["Name"].ToString().Trim().ToUpper().Equals(shortName.Trim().ToUpper()))
                    {
                        return row["Code"].ToString();
                    }

                    if (row["Name"].ToString().Trim().ToUpper().Contains(shortName.Trim().ToUpper()))
                    {
                        return row["Code"].ToString();
                    }
                }

                return string.Empty;
            }
            // i = oDV.Find(strName)
            // If i > -1 Then
            // Return oDV.Item(i).Item("Code").ToString
            // Else
            // Return ""
            // End If
            catch (Exception ex)
            {
                return "";
            }
        }

        public static bool IsDecodeValue(ref DataView oDV, ref string strCode)
        {

            return oDV.Find(strCode) > -1;

        }

        public static bool IsCruiseFilterValue(ref DataView oDV, string strCruise, string strCode)
        {
            var oVals = new object[2];

            oVals[0] = strCruise;
            oVals[1] = strCode;

            return oDV.Find(oVals) > -1;

        }

        public static string GetCruiseFilterValue(ref DataView oDV, string strCruise, string strCode)
        {
            int i;
            var oVals = new object[2];

            oVals[0] = strCruise;
            oVals[1] = strCode;

            i = oDV.Find(oVals);

            if (i > -1)
            {
                return oDV[i]["value"].ToString();
            }
            else
            {
                return "";
            }

        }

        // Public Function IsNothing(ByVal Item As Object, ByVal Replace As Object) As Object
        // If Item Is Nothing Then
        // Return Replace
        // Else
        // Return Item.Value
        // End If
        // End Function
        public static object IsNothing(object Item, object Replace)
        {
            if (Item is null)
            {
                return Replace;
            }
            else
            {
                // Cast to the specific type that has the .Value property
                XmlNode node = Item as XmlNode;
                if (node is not null)
                {
                    return node.Value;
                }

                return Item;
            }
        }


        #endregion

        #region  Send Request to Providers 

        #region  Send Air Services Request to GDS 

        public static string SendAirRequestGalileo(ttServices Service, ref TravelTalkCredential ttCredential, ref TripXMLProviderSystems ttProviderSystems, ref string strRequest, string Version = "")
        {
            string strResponse = "";
            Galileo.AirServices ttService;

            try
            {
                ttService = new Galileo.AirServices();

                ttService.Version = Version;
                ttService.XslPath = XslPath;
                ttService.ProviderSystems = ttProviderSystems;
                ttService.Request = strRequest;

                switch (Service)
                {
                    case ttServices.AirFlifo:
                        {
                            strResponse = ttService.AirFlifo();
                            break;
                        }
                    case ttServices.AirPrice:
                        {
                            strResponse = ttService.AirPrice();
                            break;
                        }
                    case ttServices.AirRules:
                        {
                            strResponse = ttService.AirRules();
                            break;
                        }
                    case ttServices.AirSeatMap:
                        {
                            strResponse = ttService.AirSeatMap();
                            break;
                        }
                    case ttServices.LowFarePlus:
                        {
                            strResponse = ttService.LowFarePlus();
                            break;
                        }
                    case ttServices.FareDisplay:
                        {
                            strResponse = ttService.FareDisplay();
                            break;
                        }

                    default:
                        {
                            throw new Exception(string.Format("{0} Message is not supported by Galileo.", Service.ToString()));
                        }

                }

                return strResponse;
            }

            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                GC.Collect();
            }

        }

        public static string SendAirRequestAmadeusWS(ttServices Service, ref TravelTalkCredential ttCredential, ref TripXMLProviderSystems ttProviderSystems, ref string strRequest, string Version = "")
        {
            AmadeusWS.AirServices ttService;
            string strResponse = "";

            try
            {
                ttService = new AmadeusWS.AirServices();

                ttService.Version = Version;
                ttService.XslPath = XslPath;
                ttService.ttProviderSystems = ttProviderSystems;
                ttService.Request = strRequest;

                switch (Service)
                {
                    case ttServices.AirFlifo:
                        {
                            if (!ttProviderSystems.AmadeusWS | ttProviderSystems.AmadeusWS & !string.IsNullOrEmpty(ttProviderSystems.AmadeusWSSchema[enAmadeusWSSchema.Air_FlightInfo]))
                            {
                                strResponse = ttService.AirFlifo();
                            }
                            else
                            {
                                throw new Exception("Air_FlightInfo not authorized");
                            }

                            break;
                        }
                    case ttServices.AirPrice:
                        {
                            strResponse = ttService.AirPrice();
                            break;
                        }
                    case ttServices.AirRules:
                        {
                            strResponse = ttService.AirRules();
                            break;
                        }
                    case ttServices.AirSeatMap:
                        {
                            strResponse = ttService.AirSeatMap();
                            break;
                        }
                    // Case ttServices.LowFarePlus
                    // strResponse = .LowFarePlus()
                    case ttServices.FareInfo:
                        {
                            strResponse = ttService.FareInfo();
                            break;
                        }
                    case ttServices.FareDisplay:
                        {
                            strResponse = ttService.FareDisplay();
                            break;
                        }
                    case ttServices.AirSchedule:
                        {
                            strResponse = ttService.AirSchedule();
                            break;
                        }

                    default:
                        {
                            throw new Exception(string.Format("{0} Message is not supported by Amadeus.", Service.ToString()));
                        }

                        // ttAA = .ttAPIAdapter
                }

                return strResponse;
            }

            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                GC.Collect();
            }

        }

        public static string SendAirRequestSabre(ttServices Service, ref TravelTalkCredential ttCredential, ref TripXMLProviderSystems ttProviderSystems, ref string strRequest, string Version = "")
        {
            string strResponse = "";
            Sabre.AirServices ttService;

            try
            {
                ttService = new Sabre.AirServices();

                ttService.Version = Version;
                ttService.XslPath = XslPath;
                ttService.ProviderSystems = ttProviderSystems;
                ttService.Request = strRequest;

                switch (Service)
                {
                    case ttServices.AirFlifo:
                        {
                            strResponse = ttService.AirFlifo();
                            break;
                        }
                    case ttServices.AirPrice:
                        {
                            strResponse = ttService.AirPrice();
                            break;
                        }
                    case ttServices.AirRules:
                        {
                            strResponse = ttService.AirRules();
                            break;
                        }
                    case ttServices.AirSeatMap:
                        {
                            strResponse = ttService.AirSeatMap();
                            break;
                        }
                    case ttServices.FareDisplay:
                        {
                            strResponse = ttService.FareDisplay();
                            break;
                        }

                    default:
                        {
                            throw new Exception(string.Format("{0} Message is not supported by Sabre.", Service.ToString()));
                        }

                }

                return strResponse;
            }

            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                GC.Collect();
            }

        }

        public static string SendAirRequestWorldspan(ttServices Service, ref TravelTalkCredential ttCredential, ref TripXMLProviderSystems ttProviderSystems, ref string strRequest, string Version = "")
        {
            string strResponse = "";
            Worldspan.AirServices ttService;

            try
            {
                ttService = new Worldspan.AirServices();

                ttService.Version = Version;
                ttService.XslPath = XslPath;
                ttService.ProviderSystems = ttProviderSystems;
                ttService.Request = strRequest;

                switch (Service)
                {
                    case ttServices.AirPrice:
                        {
                            strResponse = ttService.AirPrice();
                            break;
                        }
                    case ttServices.AirRules:
                        {
                            strResponse = ttService.AirRules();
                            break;
                        }
                    case ttServices.AirSeatMap:
                        {
                            strResponse = ttService.AirSeatMap();
                            break;
                        }
                    case ttServices.FareDisplay:
                        {
                            strResponse = ttService.FareDisplay();
                            break;
                        }

                    default:
                        {
                            throw new Exception(string.Format("{0} Message is not supported by Worldspan.", Service.ToString()));
                        }

                }

                return strResponse;
            }

            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                GC.Collect();
            }

        }

        // Public Function SendAirRequestAirCanada(ByVal Service As ttServices, ByRef ttCredential As TravelTalkCredential, ByRef ttProviderSystems As TripXMLProviderSystems, ByRef strRequest As String, Optional ByVal Version As String = "") As String
        // Dim ttService As AirCanada.AirServices = Nothing
        // Dim strResponse As String = ""
        // Try
        // ttService = New AirCanada.AirServices
        // With ttService
        // .Version = Version
        // .XslPath = gXslPath
        // .ttProviderSystems = ttProviderSystems
        // .Request = strRequest
        // Select Case Service
        // Case ttServices.AirPrice
        // strResponse = .AirPrice()
        // 'Case ttServices.AirRules
        // '    strResponse = .AirRules()
        // Case ttServices.AirSeatMap
        // strResponse = .AirSeatMap()
        // 'Case ttServices.FareInfo
        // '    strResponse = .FareInfo()
        // 'Case ttServices.FareDisplay
        // '    strResponse = .FareDisplay()
        // 'Case ttServices.AirSchedule
        // '    strResponse = .AirSchedule()
        // End Select
        // End With
        // Return strResponse
        // Catch ex As Exception
        // Throw ex
        // Finally
        // If Not ttService Is Nothing Then ttService = Nothing
        // End Try
        // End Function

        // Public Function SendAirRequestPyton(ByVal Service As ttServices, ByRef ttCredential As TravelTalkCredential, ByRef ttProviderSystems As TripXMLProviderSystems, ByRef strRequest As String, Optional ByVal Version As String = "") As String
        // Dim ttService As Pyton.AirServices = Nothing
        // Dim strResponse As String = ""
        // Try
        // ttService = New Pyton.AirServices
        // With ttService
        // .Version = Version
        // .XslPath = gXslPath
        // .ttProviderSystems = ttProviderSystems
        // .Request = strRequest
        // Select Case Service
        // Case ttServices.AirPrice
        // strResponse = .AirPrice()
        // 'Case ttServices.AirRules
        // '    strResponse = .AirRules()
        // 'Case ttServices.AirSeatMap
        // '   strResponse = .AirSeatMap()
        // 'Case ttServices.FareInfo
        // '    strResponse = .FareInfo()
        // 'Case ttServices.FareDisplay
        // '    strResponse = .FareDisplay()
        // 'Case ttServices.AirSchedule
        // '    strResponse = .AirSchedule()
        // End Select
        // End With
        // Return strResponse
        // Catch ex As Exception
        // Throw ex
        // Finally
        // If Not ttService Is Nothing Then ttService = Nothing
        // End Try
        // End Function

        public static string SendAirRequestTravelport(ttServices Service, ref TravelTalkCredential ttCredential, ref TripXMLProviderSystems ttProviderSystems, ref string strRequest, string Version = "")
        {
            string strResponse = "";
            Travelport.AirServices ttService;

            try
            {
                ttService = new Travelport.AirServices();

                ttService.Version = Version;
                ttService.XslPath = XslPath;
                ttService.ProviderSystems = ttProviderSystems;
                ttService.Request = strRequest;

                switch (Service)
                {
                    case ttServices.AirFlifo:
                        {
                            strResponse = ttService.AirFlifo();
                            break;
                        }
                    case ttServices.AirPrice:
                        {
                            strResponse = ttService.AirPrice();
                            break;
                        }
                    case ttServices.AirRules:
                        {
                            strResponse = ttService.AirRules();
                            break;
                        }
                    case ttServices.AirSeatMap:
                        {
                            strResponse = ttService.AirSeatMap();
                            break;
                        }
                    case ttServices.LowFarePlus:
                        {
                            strResponse = ttService.LowFarePlus();
                            break;
                        }
                    case ttServices.FareDisplay:
                        {
                            strResponse = ttService.FareDisplay();
                            break;
                        }

                    default:
                        {
                            throw new Exception(string.Format("{0} Message is not supported by Galileo.", Service.ToString()));
                        }

                }

                return strResponse;
            }

            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                GC.Collect();
            }

        }
        #endregion

        #region  Send Car Services Request to GDS 

        public static string SendCarRequestGalileo(ttServices Service, ref TravelTalkCredential ttCredential, ref TripXMLProviderSystems ttProviderSystems, ref string strRequest, string Version = "")
        {
            string strResponse = "";
            Galileo.CarServices ttService;

            try
            {
                ttService = new Galileo.CarServices();

                ttService.Version = Version;
                ttService.XslPath = XslPath;
                ttService.ProviderSystems = ttProviderSystems;
                ttService.Request = strRequest;

                switch (Service)
                {
                    case ttServices.CarInfo:
                        {
                            strResponse = ttService.CarInfo();
                            break;
                        }

                }

                return strResponse;
            }

            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                GC.Collect();
            }

        }

        public static string SendCarRequestAmadeusWS(ttServices Service, ref TravelTalkCredential ttCredential, ref TripXMLProviderSystems ttProviderSystems, ref string strRequest, string Version = "")
        {
            CarServices ttService;
            string strResponse = "";

            try
            {
                ttService = new CarServices();

                ttService.Version = Version;
                ttService.XslPath = XslPath;
                ttService.ttProviderSystems = ttProviderSystems;
                ttService.Request = strRequest;

                switch (Service)
                {
                    case ttServices.CarInfo:
                        {
                            strResponse = ttService.CarInfo();
                            break;
                        }
                    case ttServices.CarRules:
                        {
                            strResponse = ttService.CarRules();
                            break;
                        }
                    case ttServices.CarList:
                        {
                            strResponse = ttService.CarList();
                            break;
                        }

                        // ttAA = .ttAPIAdapter
                }

                return strResponse;
            }

            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                GC.Collect();
            }

        }

        public static string SendCarRequestSabre(ttServices Service, ref TravelTalkCredential ttCredential, ref TripXMLProviderSystems ttProviderSystems, ref string strRequest, string Version = "")
        {
            string strResponse = "";
            Sabre.CarServices ttService;

            try
            {
                ttService = new Sabre.CarServices();

                ttService.Version = Version;
                ttService.XslPath = XslPath;
                ttService.ProviderSystems = ttProviderSystems;
                ttService.Request = strRequest;

                switch (Service)
                {
                    case ttServices.CarInfo:
                        {
                            strResponse = ttService.CarInfo();
                            break;
                        }

                }

                return strResponse;
            }

            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                GC.Collect();
            }

        }

        #endregion

        #region  Send Hotel Services Request to GDS 

        public static string SendHotelRequestGalileo(ttServices Service, ref TravelTalkCredential ttCredential, ref TripXMLProviderSystems ttProviderSystems, ref string strRequest, string Version = "")
        {
            string strResponse = "";
            Galileo.HotelServices ttService;

            try
            {
                ttService = new Galileo.HotelServices();

                ttService.Version = Version;
                ttService.XslPath = XslPath;
                ttService.ProviderSystems = ttProviderSystems;
                ttService.Request = strRequest;

                switch (Service)
                {
                    case ttServices.HotelInfo:
                        {
                            strResponse = ttService.HotelInfo();
                            break;
                        }

                }

                return strResponse;
            }

            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                GC.Collect();
            }

        }

        public static string SendHotelRequestAmadeusWS(ttServices Service, ref TravelTalkCredential ttCredential, ref TripXMLProviderSystems ttProviderSystems, ref string strRequest, string Version = "")
        {
            HotelServices ttService;
            string strResponse = "";

            try
            {
                ttService = new HotelServices();

                ttService.Version = Version;
                ttService.XslPath = XslPath;
                ttService.ttProviderSystems = ttProviderSystems;
                ttService.Request = strRequest;

                switch (Service)
                {
                    case ttServices.HotelInfo:
                        {
                            strResponse = ttService.HotelInfo();
                            break;
                        }

                        // ttAA = .ttAPIAdapter
                }

                return strResponse;
            }

            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                GC.Collect();
            }

        }

        public static string SendHotelRequestSabre(ttServices Service, ref TravelTalkCredential ttCredential, ref TripXMLProviderSystems ttProviderSystems, ref string strRequest, string Version = "")
        {
            string strResponse = "";
            Sabre.HotelServices ttService;

            try
            {
                ttService = new Sabre.HotelServices();

                ttService.Version = Version;
                ttService.XslPath = XslPath;
                ttService.ProviderSystems = ttProviderSystems;
                ttService.Request = strRequest;

                switch (Service)
                {
                    case ttServices.HotelInfo:
                        {
                            strResponse = ttService.HotelInfo();
                            break;
                        }
                }

                return strResponse;
            }

            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                GC.Collect();
            }

        }


        #endregion

        #region  Send PNR Services Request to GDS 

        public static string SendPNRRequestGalileo(ttServices Service, ref TravelTalkCredential ttCredential, ref TripXMLProviderSystems ttProviderSystems, ref string strRequest, string Version = "")
        {
            string strResponse = "";
            Galileo.PNRServices ttService;

            try
            {
                ttService = new Galileo.PNRServices();

                ttService.Version = Version;
                ttService.XslPath = XslPath;
                ttService.ProviderSystems = ttProviderSystems;
                ttService.Request = strRequest;

                switch (Service)
                {
                    case ttServices.PNRRead:
                        {
                            strResponse = ttService.PNRRead();
                            break;
                        }
                    case ttServices.PNRCancel:
                        {
                            strResponse = ttService.PNRCancel();
                            break;
                        }
                    case ttServices.Queue:
                        {
                            strResponse = ttService.Queue();
                            break;
                        }
                    case ttServices.QueueRead:
                        {
                            strResponse = ttService.QueueRead();
                            break;
                        }
                    case ttServices.PNRReprice:
                        {
                            strResponse = ttService.PNRReprice();
                            break;
                        }
                    case ttServices.PNREnd:
                        {
                            strResponse = ttService.PNREnd();
                            break;
                        }

                    default:
                        {
                            throw new Exception(string.Format("{0} Message is not supported by Galileo.", Service.ToString()));
                        }

                }

                return strResponse;
            }

            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                GC.Collect();
            }

        }

        public static string SendPNRRequestAmadeusWS(ttServices Service, ref TravelTalkCredential ttCredential, ref TripXMLProviderSystems ttProviderSystems, ref string strRequest, string Version = "")
        {
            AmadeusWS.PNRServices ttService;
            string strResponse = "";

            try
            {
                ttService = new AmadeusWS.PNRServices();

                ttService.Version = Version;
                ttService.XslPath = XslPath;
                ttService.Request = strRequest;
                ttService.ttProviderSystems = ttProviderSystems;

                switch (Service)
                {
                    case ttServices.PNRRead:
                        {
                            strResponse = ttService.PNRRead();
                            break;
                        }
                    case ttServices.PNREnd:
                        {
                            strResponse = ttService.PNREnd();
                            break;
                        }
                    case ttServices.PNRCancel:
                        {
                            strResponse = ttService.PNRCancel();
                            break;
                        }
                    case ttServices.Queue:
                        {
                            strResponse = ttService.Queue();
                            break;
                        }
                    case ttServices.QueueRead:
                        {
                            strResponse = ttService.QueueRead();
                            break;
                        }
                    case ttServices.PNRReprice:
                        {
                            strResponse = ttService.PNRReprice();
                            break;
                        }
                    case ttServices.PNRSplit:
                        {
                            strResponse = ttService.PNRSplit();
                            break;
                        }
                    case ttServices.SearchName:
                        {
                            strResponse = ttService.SearchName();
                            break;
                        }
                    case ttServices.TransferOwnership:
                        {
                            strResponse = ttService.TransferOwnership();
                            break;
                        }

                    default:
                        {
                            throw new Exception(string.Format("{0} Message is not supported by Amadeus.", Service.ToString()));
                        }

                        // ttAA = .ttAPIAdapter
                }

                return strResponse;
            }

            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                GC.Collect();
            }

        }

        public static string SendPNRRequestSabre(ttServices Service, ref TravelTalkCredential ttCredential, ref TripXMLProviderSystems ttProviderSystems, ref string strRequest, string Version = "")
        {
            string strResponse = "";
            Sabre.PNRServices ttService;

            try
            {
                ttService = new Sabre.PNRServices();

                ttService.Version = Version;
                ttService.XslPath = XslPath;
                ttService.ProviderSystems = ttProviderSystems;
                ttService.Request = strRequest;

                switch (Service)
                {
                    case ttServices.PNRRead:
                        {
                            strResponse = ttService.PNRRead();
                            break;
                        }
                    case ttServices.PNRCancel:
                        {
                            strResponse = ttService.PNRCancel();
                            break;
                        }
                    case ttServices.PNRReprice:
                        {
                            strResponse = ttService.PNRReprice();
                            break;
                        }
                    case ttServices.Queue:
                        {
                            strResponse = ttService.Queue();
                            break;
                        }
                    case ttServices.QueueRead:
                        {
                            strResponse = ttService.QueueRead();
                            break;
                        }
                    case ttServices.PNREnd:
                        {
                            strResponse = ttService.PNREnd();
                            break;
                        }

                    default:
                        {
                            throw new Exception(string.Format("{0} Message is not supported by Sabre.", Service.ToString()));
                        }

                }

                return strResponse;
            }

            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                GC.Collect();
            }

        }

        public static string SendPNRRequestWorldspan(ttServices Service, ref TravelTalkCredential ttCredential, ref TripXMLProviderSystems ttProviderSystems, ref string strRequest, string Version = "")
        {
            string strResponse = "";
            Worldspan.PNRServices ttService;

            try
            {
                ttService = new Worldspan.PNRServices();

                ttService.Version = Version;
                ttService.XslPath = XslPath;
                ttService.ProviderSystems = ttProviderSystems;
                ttService.Request = strRequest;

                switch (Service)
                {
                    case ttServices.PNRRead:
                        {
                            strResponse = ttService.PNRRead();
                            break;
                        }
                    case ttServices.PNRCancel:
                        {
                            strResponse = ttService.PNRCancel();
                            break;
                        }
                    case ttServices.Queue:
                        {
                            strResponse = ttService.Queue();
                            break;
                        }
                    case ttServices.PNRReprice:
                        {
                            strResponse = ttService.PNRReprice();
                            break;
                        }
                    case ttServices.PNREnd:
                        {
                            strResponse = ttService.PNREnd();
                            break;
                        }

                    default:
                        {
                            throw new Exception(string.Format("{0} Message is not supported by Worldspan.", Service.ToString()));
                        }

                }

                return strResponse;
            }

            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                GC.Collect();
            }

        }

        public static string SendPNRRequestTravelPort(ttServices Service, ref TravelTalkCredential ttCredential, ref TripXMLProviderSystems ttProviderSystems, ref string strRequest, string Version = "")
        {
            string strResponse = "";
            Travelport.PNRServices ttService;

            try
            {
                ttService = new Travelport.PNRServices();

                ttService.Version = Version;
                ttService.XslPath = XslPath;
                ttService.ProviderSystems = ttProviderSystems;
                ttService.Request = strRequest;
                // TODO: PNR Read and other commendted ones are yet to implement
                switch (Service)
                {
                    case ttServices.PNRRead:
                        {
                            strResponse = ttService.PNRRead();
                            break;
                        }
                    case ttServices.PNRReprice:
                        {
                            strResponse = ttService.PNRReprice();
                            break;
                        }
                    // Case ttServices.PNRCancel
                    // 'strResponse = .PNRCancel()
                    case ttServices.Queue:
                        {
                            strResponse = ttService.Queue();
                            break;
                        }
                        // Case ttServices.QueueRead
                        // 'strResponse = .QueueRead

                }

                return strResponse;
            }

            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                GC.Collect();
            }

        }
        #endregion

        #region  Send Travel Services Request to GDS 

        public static string SendTravelRequestGalileo(ttServices Service, ref TravelTalkCredential ttCredential, ref TripXMLProviderSystems ttProviderSystems, ref string strRequest, string Version = "")
        {
            string strResponse = "";
            Galileo.TravelServices ttService;

            try
            {
                ttService = new Galileo.TravelServices();

                ttService.Version = Version;
                ttService.XslPath = XslPath;
                ttService.ProviderSystems = ttProviderSystems;
                ttService.Request = strRequest;

                switch (Service)
                {
                    case ttServices.TravelBuild:
                        {
                            strResponse = ttService.TravelBuild();
                            break;
                        }
                    case ttServices.TravelModify:
                        {
                            strResponse = ttService.TravelModify();
                            break;
                        }
                    case ttServices.IssueTicket:
                        {
                            strResponse = ttService.IssueTicket();
                            break;
                        }
                    case ttServices.IssueTicketSessioned:
                        {
                            strResponse = ttService.IssueTicketSessioned();
                            break;
                        }
                    case ttServices.Update:
                        {
                            strResponse = ttService.Update();
                            break;
                        }
                    case ttServices.UpdateSessioned:
                        {
                            strResponse = ttService.UpdateSessioned();
                            break;
                        }
                    case ttServices.TicketVoid:
                        {
                            strResponse = ttService.VoidTicket();
                            break;
                        }
                    case ttServices.IssueMCO:
                        {
                            strResponse = ttService.IssueMCO();
                            break;
                        }

                    default:
                        {
                            throw new Exception(string.Format("{0} Message is not supported by Galileo.", Service.ToString()));
                        }

                }

                return strResponse;
            }

            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                GC.Collect();
            }

        }

        public static string SendTravelRequestAmadeusWS(ttServices Service, ref TravelTalkCredential ttCredential, ref TripXMLProviderSystems ttProviderSystems, ref string strRequest, string Version = "")
        {
            AmadeusWS.TravelServices ttService;
            string strResponse = "";

            try
            {
                ttService = new AmadeusWS.TravelServices();

                ttService.Version = Version;
                ttService.XslPath = XslPath;
                ttService.Request = strRequest.Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", "");
                ttService.ttProviderSystems = ttProviderSystems;

                switch (Service)
                {
                    case ttServices.TravelBuild:
                        {
                            strResponse = ttService.TravelBuild();
                            break;
                        }
                    // Case ttServices.TravelModify
                    // strResponse = .TravelModify()
                    case ttServices.IssueTicket:
                        {
                            strResponse = ttService.IssueTicket();
                            break;
                        }
                    case ttServices.IssueTicketSessioned:
                        {
                            strResponse = ttService.IssueTicketSessioned();
                            break;
                        }
                    case ttServices.StoredFareBuild:
                        {
                            strResponse = ttService.StoredFareBuild();
                            break;
                        }
                    case ttServices.StoredFareUpdate:
                        {
                            strResponse = ttService.StoredFareUpdate();
                            break;
                        }
                    case ttServices.Update:
                        {
                            strResponse = ttService.Update();
                            break;
                        }
                    case ttServices.UpdateSessioned:
                        {
                            strResponse = ttService.UpdateSessioned();
                            break;
                        }
                    case ttServices.TicketVoid:
                        {
                            strResponse = ttService.VoidTicket();
                            break;
                        }
                    case ttServices.TicketDisplay:
                        {
                            strResponse = ttService.DisplayTicket();
                            break;
                        }
                    case ttServices.RefundTicket:
                        {
                            strResponse = ttService.RefundTicket();
                            break;
                        }
                    case ttServices.ReissueTicket:
                        {
                            strResponse = ttService.ReissueTicket();
                            break;
                        }

                    default:
                        {
                            throw new Exception(string.Format("{0} Message is not supported by Amadeus.", Service.ToString()));
                        }

                        // ttAA = .ttAPIAdapter
                }

                return strResponse;
            }

            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                GC.Collect();
            }

        }

        public static string SendTravelRequestSabre(ttServices Service, ref TravelTalkCredential ttCredential, ref TripXMLProviderSystems ttProviderSystems, ref string strRequest, string Version = "")
        {
            string strResponse = "";
            Sabre.TravelServices ttService;

            try
            {
                ttService = new Sabre.TravelServices();

                ttService.Version = Version;
                ttService.XslPath = XslPath;
                ttService.ProviderSystems = ttProviderSystems;
                ttService.Request = strRequest;

                switch (Service)
                {
                    case ttServices.TravelBuild:
                        {
                            if (Version == "v04")
                            {
                                strResponse = ttService.TravelBuild_V4();
                                // Else
                                // strResponse = .TravelBuild_V3 'for testing only
                            }

                            break;
                        }
                    case ttServices.IssueTicket:
                        {
                            strResponse = ttService.IssueTicket();
                            break;
                        }
                    case ttServices.IssueTicketSessioned:
                        {
                            strResponse = ttService.IssueTicketSessioned();
                            break;
                        }
                    case ttServices.Update:
                        {
                            strResponse = ttService.Update();
                            break;
                        }
                    case ttServices.UpdateSessioned:
                        {
                            strResponse = ttService.UpdateSessioned();
                            break;
                        }
                    case ttServices.IssueMCO:
                        {
                            strResponse = ttService.IssueMCO();
                            break;
                        }

                    default:
                        {
                            throw new Exception(string.Format("{0} Message is not supported by Sabre.", Service.ToString()));
                        }
                }

                return strResponse;
            }

            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                GC.Collect();
            }

        }

        public static string SendTravelRequestWorldspan(ttServices Service, ref TravelTalkCredential ttCredential, ref TripXMLProviderSystems ttProviderSystems, ref string strRequest, string Version = "")
        {
            string strResponse = "";
            Worldspan.TravelServices ttService;

            try
            {
                ttService = new Worldspan.TravelServices();

                ttService.Version = Version;
                ttService.XslPath = XslPath;
                ttService.ProviderSystems = ttProviderSystems;
                ttService.Request = strRequest;

                switch (Service)
                {
                    case ttServices.TravelBuild:
                        {
                            strResponse = ttService.TravelBuild();
                            break;
                        }
                    case ttServices.Update:
                        {
                            strResponse = ttService.Update();
                            break;
                        }
                    case ttServices.UpdateSessioned:
                        {
                            strResponse = ttService.UpdateSessioned();
                            break;
                        }
                    case ttServices.IssueTicketSessioned:
                        {
                            strResponse = ttService.IssueTicketSessioned();
                            break;
                        }
                    case ttServices.Authorization:
                        {
                            strResponse = ttService.Authorization();
                            break;
                        }

                    default:
                        {
                            throw new Exception(string.Format("{0} Message is not supported by Worldspan.", Service.ToString()));
                        }

                }

                return strResponse;
            }

            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                GC.Collect();
            }

        }

        public static string SendTravelRequestTravelport(ttServices Service, ref TravelTalkCredential ttCredential, ref TripXMLProviderSystems ttProviderSystems, ref string strRequest, string Version = "")
        {
            string strResponse = "";
            Travelport.TravelServices ttService;

            try
            {
                ttService = new Travelport.TravelServices();

                ttService.Version = Version;
                ttService.XslPath = XslPath;
                ttService.ProviderSystems = ttProviderSystems;
                ttService.Request = strRequest;

                switch (Service)
                {
                    case ttServices.TravelBuild:
                        {
                            strResponse = ttService.TravelBuild(); // for testing only
                            break;
                        }
                    case ttServices.IssueTicket:
                        {
                            break;
                        }
                    // strResponse = .IssueTicket
                    case ttServices.IssueTicketSessioned:
                        {
                            break;
                        }
                    // strResponse = .IssueTicketSessioned
                    case ttServices.Update:
                        {
                            break;
                        }
                    // strResponse = .Update()
                    case ttServices.UpdateSessioned:
                        {
                            strResponse = ttService.UpdateSessioned();
                            break;
                        }

                    default:
                        {
                            throw new Exception(string.Format("{0} Message is not supported by Travelport.", Service.ToString()));
                        }

                }

                return strResponse;
            }

            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                GC.Collect();
            }

        }

        #endregion

        #region  Send Other Services Request to GDS 

        public static string SendOtherRequestGalileo(ttServices Service, ref TravelTalkCredential ttCredential, ref TripXMLProviderSystems ttProviderSystems, ref string strRequest, string Version = "")
        {
            string strResponse = "";
            Galileo.OtherServices ttService;

            try
            {
                ttService = new Galileo.OtherServices();

                ttService.Version = Version;
                ttService.XslPath = XslPath;
                ttService.ProviderSystems = ttProviderSystems;
                ttService.Request = strRequest;

                switch (Service)
                {
                    case ttServices.CreateSession:
                        {
                            strResponse = ttService.CreateSession();
                            break;
                        }
                    case ttServices.CloseSession:
                        {
                            strResponse = ttService.CloseSession();
                            break;
                        }
                    case ttServices.ShowMileage:
                        {
                            strResponse = ttService.ShowMileage();
                            break;
                        }
                    case ttServices.CCValid:
                        {
                            strResponse = ttService.CreditCardValid();
                            break;
                        }
                    case ttServices.CurConv:
                        {
                            strResponse = ttService.CurrencyConvertion();
                            break;
                        }
                    case ttServices.TimeDiff:
                        {
                            strResponse = ttService.TimeDifference();
                            break;
                        }
                    case ttServices.Cryptic:
                        {
                            strResponse = ttService.Cryptic();
                            break;
                        }
                    case ttServices.Native:
                        {
                            strResponse = ttService.Native();
                            break;
                        }
                    case ttServices.ETicketVerify:
                        {
                            strResponse = ttService.ETicketVerify();
                            break;
                        }
                    case ttServices.MultiMessage:
                        {
                            strResponse = ttService.MultiMessage();
                            break;
                        }
                    case ttServices.ProfileRead:
                        {
                            strResponse = ttService.ProfileRead();
                            break;
                        }
                    case ttServices.ProfileCreate:
                        {
                            strResponse = ttService.ProfileCreate();
                            break;
                        }

                    default:
                        {
                            throw new Exception(string.Format("{0} Message is not supported by Galileo.", Service.ToString()));
                        }

                }

                return strResponse;
            }

            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                GC.Collect();
            }

        }

        public static string SendOtherRequestAmadeusWS(ttServices Service, ref TravelTalkCredential ttCredential, ref TripXMLProviderSystems ttProviderSystems, ref string strRequest, string Version = "")
        {
            AmadeusWS.OtherServices ttService;
            string strResponse = "";

            try
            {
                ttService = new AmadeusWS.OtherServices();

                ttService.Version = Version;
                ttService.XslPath = XslPath;
                ttService.ttProviderSystems = ttProviderSystems;
                ttService.Request = strRequest;

                switch (Service)
                {
                    case ttServices.CreateSession:
                        {
                            strResponse = ttService.CreateSession();
                            break;
                        }
                    case ttServices.CloseSession:
                        {
                            strResponse = ttService.CloseSession();
                            break;
                        }
                    case ttServices.ShowMileage:
                        {
                            strResponse = ttService.ShowMileage();
                            break;
                        }
                    case ttServices.CCValid:
                        {
                            strResponse = ttService.CreditCardValid();
                            break;
                        }
                    case ttServices.CurConv:
                        {
                            strResponse = ttService.CurrencyConvertion();
                            break;
                        }
                    case ttServices.TimeDiff:
                        {
                            strResponse = ttService.TimeDifference();
                            break;
                        }
                    case ttServices.Cryptic:
                        {
                            strResponse = ttService.Cryptic();
                            break;
                        }
                    case ttServices.SalesReport:
                        {
                            strResponse = ttService.SalesReport();
                            break;
                        }
                    case ttServices.Native:
                        {
                            strResponse = ttService.Native();
                            break;
                        }
                    case ttServices.TripXMLNative:
                        {
                            strResponse = ttService.TripXMLNative();
                            break;
                        }

                    default:
                        {
                            throw new Exception(string.Format("{0} Message is not supported by Amadeus.", Service.ToString()));
                        }

                        // ttAA = .ttAPIAdapter
                }

                return strResponse;
            }

            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                GC.Collect();
            }

        }

        public static string SendOtherRequestSabre(ttServices Service, ref TravelTalkCredential ttCredential, ref TripXMLProviderSystems ttProviderSystems, ref string strRequest, string Version = "")
        {
            string strResponse = "";
            Sabre.OtherServices ttService;

            try
            {
                ttService = new Sabre.OtherServices();

                ttService.Version = Version;
                ttService.XslPath = XslPath;
                ttService.ProviderSystems = ttProviderSystems;
                ttService.Request = strRequest;

                switch (Service)
                {
                    case ttServices.CreateSession:
                        {
                            strResponse = ttService.CreateSession();
                            break;
                        }
                    case ttServices.CloseSession:
                        {
                            strResponse = ttService.CloseSession();
                            break;
                        }
                    case ttServices.ShowMileage:
                        {
                            strResponse = ttService.ShowMileage();
                            break;
                        }
                    case ttServices.CCValid:
                        {
                            strResponse = ttService.CreditCardValid();
                            break;
                        }
                    case ttServices.CurConv:
                        {
                            strResponse = ttService.CurrencyConvertion();
                            break;
                        }
                    case ttServices.TimeDiff:
                        {
                            strResponse = ttService.TimeDifference();
                            break;
                        }
                    case ttServices.Cryptic:
                        {
                            strResponse = ttService.Cryptic();
                            break;
                        }
                    case ttServices.SalesReport:
                        {
                            strResponse = ttService.SalesReport();
                            break;
                        }
                    case ttServices.Native:
                        {
                            strResponse = ttService.Native();
                            break;
                        }

                    default:
                        {
                            throw new Exception(string.Format("{0} Message is not supported by Sabre.", Service.ToString()));
                        }

                }

                return strResponse;
            }

            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                GC.Collect();
            }

        }

        public static string SendOtherRequestWorldspan(ttServices Service, ref TravelTalkCredential ttCredential, ref TripXMLProviderSystems ttProviderSystems, ref string strRequest, string Version = "")
        {
            string strResponse = "";
            Worldspan.OtherServices ttService;

            try
            {
                ttService = new Worldspan.OtherServices();

                ttService.Version = Version;
                ttService.XslPath = XslPath;
                ttService.ProviderSystems = ttProviderSystems;
                ttService.Request = strRequest;

                switch (Service)
                {
                    case ttServices.CreateSession:
                        {
                            strResponse = ttService.CreateSession();
                            break;
                        }
                    case ttServices.CloseSession:
                        {
                            strResponse = ttService.CloseSession();
                            break;
                        }
                    case ttServices.Native:
                        {
                            strResponse = ttService.Native();
                            break;
                        }
                    case ttServices.Cryptic:
                        {
                            strResponse = ttService.Cryptic();
                            break;
                        }

                    default:
                        {
                            throw new Exception(string.Format("{0} Message is not supported by Worldspan.", Service.ToString()));
                        }

                }

                return strResponse;
            }

            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                GC.Collect();
            }

        }

        public static string SendOtherRequestTravelport(ttServices Service, ref TravelTalkCredential ttCredential, ref TripXMLProviderSystems ttProviderSystems, ref string strRequest, string Version = "")
        {
            string strResponse = "";
            Travelport.OtherServices ttService;

            try
            {
                ttService = new Travelport.OtherServices();

                // .Version = Version
                // .XslPath = XslPath
                ttService.ProviderSystems = ttProviderSystems;
                ttService.Request = strRequest;

                switch (Service)
                {
                    case ttServices.CreateSession:
                        {
                            strResponse = ttService.CreateSession();
                            break;
                        }
                    case ttServices.CloseSession:
                        {
                            strResponse = ttService.CloseSession();
                            break;
                        }
                    case ttServices.Cryptic:
                        {
                            strResponse = ttService.Cryptic();
                            break;
                        }
                    case ttServices.Native:
                        {
                            strResponse = ttService.Native();
                            break;
                        }

                    default:
                        {
                            throw new Exception(string.Format("{0} Message is not supported by Travelport.", Service.ToString()));
                        }

                }

                return strResponse;
            }

            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                GC.Collect();
            }

        }

        public static string SendOtherRequestiTravelInsured(ttServices Service, ref TravelTalkCredential ttCredential, ref TripXMLProviderSystems ttProviderSystems, ref string strRequest, string Version = "")
        {
            string strResponse = "";
            // Dim ttService As ttiTravelInsuredService.OtherServices = Nothing

            try
            {
                // ttService = New ttiTravelInsuredService.OtherServices

                // With ttService
                // .Version = Version
                // .XslPath = XslPath
                // .ProviderSystems = ttProviderSystems
                // .Request = strRequest

                // Select Case Service
                // Case ttServices.InsuranceBook
                // strResponse = .InsuranceBook()
                // Case ttServices.InsuranceQuote
                // strResponse = .InsuranceQuote()
                // Case ttServices.Native
                // strResponse = .Native()
                // End Select

                // End With

                return strResponse;
            }

            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                GC.Collect();
            }

        }


        #endregion

        #region  Send Cruise Services Request to GDS 

        public static string SendCruiseRequestAmadeusWS(ttServices Service, ref TravelTalkCredential ttCredential, ref TripXMLProviderSystems ttProviderSystems, ref string strRequest, string Version = "")
        {
            AmadeusWS.CruiseServices ttService;
            string strResponse = "";

            try
            {
                ttService = new AmadeusWS.CruiseServices();

                ttService.Version = Version;
                ttService.XslPath = XslPath;
                ttService.Request = strRequest;
                ttService.ttProviderSystems = ttProviderSystems;

                switch (Service)
                {
                    case ttServices.CruiseSailAvail:
                        {
                            strResponse = ttService.CruiseSailAvail();
                            break;
                        }
                    case ttServices.CruiseFareAvail:
                        {
                            strResponse = ttService.CruiseFareAvail();
                            break;
                        }
                    case ttServices.CruiseCategoryAvail:
                        {
                            strResponse = ttService.CruiseCategoryAvail();
                            break;
                        }
                    case ttServices.CruiseCabinAvail:
                        {
                            strResponse = ttService.CruiseCabinAvail();
                            break;
                        }
                    case ttServices.CruiseCabinHold:
                        {
                            strResponse = ttService.CruiseCabinHold();
                            break;
                        }
                    case ttServices.CruiseCabinUnhold:
                        {
                            strResponse = ttService.CruiseCabinUnhold();
                            break;
                        }
                    case ttServices.CruisePriceBooking:
                        {
                            strResponse = ttService.CruisePriceBooking();
                            break;
                        }
                    case ttServices.CruiseCreateBooking:
                        {
                            strResponse = ttService.CruiseCreateBooking();
                            break;
                        }
                    case ttServices.CruiseRead:
                        {
                            strResponse = ttService.CruiseRead();
                            break;
                        }
                    case ttServices.CruiseCancelBooking:
                        {
                            strResponse = ttService.CruiseCancelBooking();
                            break;
                        }
                    case ttServices.CruiseModifyBooking:
                        {
                            strResponse = ttService.CruiseModifyBooking();
                            break;
                        }
                    case ttServices.CruisePackageAvail:
                        {
                            strResponse = ttService.CruisePackageAvail();
                            break;
                        }
                    case ttServices.CruisePackageDesc:
                        {
                            strResponse = ttService.CruisePackageDesc();
                            break;
                        }
                    case ttServices.CruiseTransferAvail:
                        {
                            strResponse = ttService.CruiseTransferAvail();
                            break;
                        }
                    case ttServices.CruiseItineraryDesc:
                        {
                            strResponse = ttService.CruiseItineraryDesc();
                            break;
                        }

                    default:
                        {
                            throw new Exception(string.Format("{0} Message is not supported by Amadeus.", Service.ToString()));
                        }
                        // ttAA = .ttAPIAdapter
                }

                return strResponse;
            }

            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                GC.Collect();
            }

        }

        public static string SendCruiseRequestGalileo(ttServices Service, ref TravelTalkCredential ttCredential, ref TripXMLProviderSystems ttProviderSystems, ref string strRequest, string Version = "")
        {
            Galileo.CruiseServices ttService;
            string strResponse = "";


            try
            {
                ttService = new Galileo.CruiseServices();

                ttService.Version = Version;
                ttService.XslPath = XslPath;
                ttService.ProviderSystems = ttProviderSystems;
                ttService.Request = strRequest;

                switch (Service)
                {
                    case ttServices.CruiseSailAvail:
                        {
                            strResponse = ttService.CruiseSailAvail();
                            break;
                        }
                    case ttServices.CruiseFareAvail:
                        {
                            strResponse = ttService.CruiseFareAvail();
                            break;
                        }
                    case ttServices.CruiseCategoryAvail:
                        {
                            strResponse = ttService.CruiseCategoryAvail();
                            break;
                        }
                    case ttServices.CruiseCabinAvail:
                        {
                            strResponse = ttService.CruiseCabinAvail();
                            break;
                        }
                    case ttServices.CruiseCabinHold:
                        {
                            strResponse = ttService.CruiseCabinHold();
                            break;
                        }
                    case ttServices.CruiseCabinUnhold:
                        {
                            strResponse = ttService.CruiseCabinUnhold();
                            break;
                        }
                    case ttServices.CruisePriceBooking:
                        {
                            strResponse = ttService.CruisePriceBooking();
                            break;
                        }
                    case ttServices.CruiseCreateBooking:
                        {
                            strResponse = ttService.CruiseCreateBooking();
                            break;
                        }
                    case ttServices.CruiseRead:
                        {
                            strResponse = ttService.CruiseRead();
                            break;
                        }
                    case ttServices.CruiseCancelBooking:
                        {
                            strResponse = ttService.CruiseCancelBooking();
                            break;
                        }
                    case ttServices.CruiseModifyBooking:
                        {
                            strResponse = ttService.CruiseModifyBooking();
                            break;
                        }
                    case ttServices.CruisePackageAvail:
                        {
                            strResponse = ttService.CruisePackageAvail();
                            break;
                        }
                    case ttServices.CruisePackageDesc:
                        {
                            strResponse = ttService.CruisePackageDesc();
                            break;
                        }
                    case ttServices.CruiseTransferAvail:
                        {
                            strResponse = ttService.CruiseTransferAvail();
                            break;
                        }

                    default:
                        {
                            throw new Exception(string.Format("{0} Message is not supported by Galileo.", Service.ToString()));
                        }
                }

                return strResponse;
            }

            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                GC.Collect();
            }

        }

        #endregion

        #region  Send Virtual Card Payment Services Request to GDS 

        public static string SendPaymentRequestGalileo(ttServices Service, ref TravelTalkCredential ttCredential, ref TripXMLProviderSystems ttProviderSystems, ref string strRequest, string Version = "")
        {
            string strResponse = "";
            Galileo.OtherServices ttService;

            try
            {
                ttService = new Galileo.OtherServices();

                ttService.Version = Version;
                ttService.XslPath = XslPath;
                ttService.ProviderSystems = ttProviderSystems;
                ttService.Request = strRequest;

                switch (Service)
                {
                    case ttServices.CreateSession:
                        {
                            strResponse = ttService.CreateSession();
                            break;
                        }
                    case ttServices.CloseSession:
                        {
                            strResponse = ttService.CloseSession();
                            break;
                        }
                    case ttServices.ShowMileage:
                        {
                            strResponse = ttService.ShowMileage();
                            break;
                        }
                    case ttServices.CCValid:
                        {
                            strResponse = ttService.CreditCardValid();
                            break;
                        }
                    case ttServices.CurConv:
                        {
                            strResponse = ttService.CurrencyConvertion();
                            break;
                        }
                    case ttServices.TimeDiff:
                        {
                            strResponse = ttService.TimeDifference();
                            break;
                        }
                    case ttServices.Cryptic:
                        {
                            strResponse = ttService.Cryptic();
                            break;
                        }
                    case ttServices.Native:
                        {
                            strResponse = ttService.Native();
                            break;
                        }
                    case ttServices.ETicketVerify:
                        {
                            strResponse = ttService.ETicketVerify();
                            break;
                        }
                    case ttServices.MultiMessage:
                        {
                            strResponse = ttService.MultiMessage();
                            break;
                        }
                    case ttServices.ProfileRead:
                        {
                            strResponse = ttService.ProfileRead();
                            break;
                        }
                    case ttServices.ProfileCreate:
                        {
                            strResponse = ttService.ProfileCreate();
                            break;
                        }

                    default:
                        {
                            throw new Exception(string.Format("{0} Message is not supported by Galileo.", Service.ToString()));
                        }

                }

                return strResponse;
            }

            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                GC.Collect();
            }

        }

        public static string SendPaymentRequestAmadeusWS(ttServices Service, ref TravelTalkCredential ttCredential, ref TripXMLProviderSystems ttProviderSystems, ref string strRequest, string Version = "")
        {
            AmadeusWS.PaymentServices ttService;
            string strResponse = "";

            try
            {
                ttService = new AmadeusWS.PaymentServices();

                ttService.Version = Version;
                ttService.XslPath = XslPath;
                ttService.ttProviderSystems = ttProviderSystems;
                ttService.Request = strRequest;

                switch (Service)
                {
                    case ttServices.GenerateVirtualCard:
                        {
                            strResponse = ttService.CreateVirtualCard();
                            break;
                        }
                    // Case ttServices.CancelVirtualCardLoad
                    // strResponse = .CancelVirtualCardLoad()
                    case ttServices.DeleteVirtualCard:
                        {
                            strResponse = ttService.DeleteVirtualCard();
                            break;
                        }
                    case ttServices.GetVirtualCardDetails:
                        {
                            strResponse = ttService.GetVirtualCardDetails();
                            break;
                        }
                    case ttServices.ListVirtualCards:
                        {
                            // Case ttServices.ManageDBIData
                            // strResponse = .ManageDBIData()
                            // Case ttServices.ScheduleVirtualCardLoad
                            // strResponse = .ScheduleVirtualCardLoad()
                            // Case ttServices.UpdateVirtualCard
                            // strResponse = .UpdateVirtualCard()
                            strResponse = ttService.ListVirtualCards();
                            break;
                        }

                    default:
                        {
                            throw new Exception(string.Format("{0} Message is not supported by Amadeus.", Service.ToString()));
                        }

                        // ttAA = .ttAPIAdapter
                }

                return strResponse;
            }

            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                GC.Collect();
            }

        }

        public static T SendPaymentRequest<T>(ttServices Service, ref TravelTalkCredential ttCredential, ref TripXMLProviderSystems ttProviderSystems, ref VirtualCardRQBase request)
        {
            string strResponse = "";
            T responseObj = default;

            try
            {
                var paymentServices = new VirtualCardPaymentService();

                paymentServices.Provider = ttProviderSystems.Provider;
                paymentServices.UUID = ttProviderSystems.LogUUID;
                paymentServices.Request = request;

                switch (Service)
                {
                    case ttServices.GenerateVirtualCard:
                        {
                            responseObj = (T)(object)paymentServices.CreateVirtualCard(); // .CreateVirtualCard()
                            break;
                        }
                    // Case ttServices.CancelVirtualCardLoad
                    // strResponse = .CancelVirtualCardLoad()
                    case ttServices.DeleteVirtualCard:
                        {
                            responseObj = (T)(object)paymentServices.DeleteVirtualCard(); // .DeleteVirtualCard()
                            break;
                        }
                    case ttServices.GetVirtualCardDetails:
                        {
                            responseObj = (T)(object)paymentServices.GetVirtualCardDetails(); // .GetVirtualCardDetails()
                            break;
                        }
                    case ttServices.ListVirtualCards:
                        {
                            // Case ttServices.ManageDBIData
                            // strResponse = .ManageDBIData()
                            // Case ttServices.ScheduleVirtualCardLoad
                            // strResponse = .ScheduleVirtualCardLoad()
                            // Case ttServices.UpdateVirtualCard
                            // strResponse = .UpdateVirtualCard()
                            responseObj = (T)(object)paymentServices.ListVirtualCards(); // .ListVirtualCards()
                            break;
                        }

                    default:
                        {
                            throw new Exception(string.Format("{0} Message is not supported by Amadeus.", Service.ToString()));
                        }

                        // ttAA = .ttAPIAdapter
                }

                return responseObj;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                GC.Collect();
            }

        }

        public static string SendPaymentRequestSabre(ttServices Service, ref TravelTalkCredential ttCredential, ref TripXMLProviderSystems ttProviderSystems, ref string strRequest, string Version = "")
        {
            string strResponse = "";
            try
            {
                // Dim ttService As New Sabre.PaymentServices
                // With ttService
                // .Version = Version
                // .XslPath = XslPath
                // .ttProviderSystems = ttProviderSystems
                // .Request = strRequest
                // Select Case Service
                // Case ttServices.GenerateVirtualCard
                // strResponse = .CreateVirtualCard()
                // 'Case ttServices.CancelVirtualCardLoad
                // '    strResponse = .CancelVirtualCardLoad()
                // Case ttServices.DeleteVirtualCard
                // strResponse = .DeleteVirtualCard()
                // Case ttServices.GetVirtualCardDetails
                // strResponse = .GetVirtualCardDetails()
                // Case ttServices.ListVirtualCards
                // strResponse = .ListVirtualCards()
                // 'Case ttServices.ManageDBIData
                // '    strResponse = .ManageDBIData()
                // 'Case ttServices.ScheduleVirtualCardLoad
                // '    strResponse = .ScheduleVirtualCardLoad()
                // 'Case ttServices.UpdateVirtualCard
                // '    strResponse = .UpdateVirtualCard()
                // Case Else
                // Throw New Exception(String.Format("{0} Message is not supported by Amadeus.", Service.ToString()))
                // End Select
                // 'ttAA = .ttAPIAdapter
                // End With

                return strResponse;
            }

            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                GC.Collect();
            }

        }

        public static string SendPaymentRequestWorldspan(ttServices Service, ref TravelTalkCredential ttCredential, ref TripXMLProviderSystems ttProviderSystems, ref string strRequest, string Version = "")
        {
            string strResponse = "";
            Worldspan.OtherServices ttService;

            try
            {
                ttService = new Worldspan.OtherServices();

                // With ttService
                // .Version = Version
                // .XslPath = XslPath
                // .ProviderSystems = ttProviderSystems
                // .Request = strRequest

                // Select Case Service
                // Case ttServices.CreateSession
                // strResponse = .CreateSession()
                // Case ttServices.CloseSession
                // strResponse = .CloseSession()
                // Case ttServices.Native
                // strResponse = .Native()
                // Case ttServices.Cryptic
                // strResponse = .Cryptic()
                // Case Else
                // Throw New Exception(String.Format("{0} Message is not supported by Worldspan.", Service.ToString()))
                // End Select

                // End With

                return strResponse;
            }

            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                GC.Collect();
            }

        }

        public static string SendPaymentRequestTravelport(ttServices Service, ref TravelTalkCredential ttCredential, ref TripXMLProviderSystems ttProviderSystems, ref string strRequest, string Version = "")
        {
            string strResponse = "";
            Travelport.OtherServices ttService;

            try
            {
                ttService = new Travelport.OtherServices();

                // With ttService
                // '.Version = Version
                // '.XslPath = XslPath
                // .ProviderSystems = ttProviderSystems
                // .Request = strRequest

                // Select Case Service
                // Case ttServices.CreateSession
                // strResponse = .CreateSession()
                // Case ttServices.CloseSession
                // strResponse = .CloseSession()
                // Case ttServices.Cryptic
                // strResponse = .Cryptic()
                // Case ttServices.Native
                // strResponse = .Native()
                // Case Else
                // Throw New Exception(String.Format("{0} Message is not supported by Travelport.", Service.ToString()))
                // End Select

                // End With

                return strResponse;
            }

            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                GC.Collect();
            }

        }

        public static string SendPaymentRequestiTravelInsured(ttServices Service, ref TravelTalkCredential ttCredential, ref TripXMLProviderSystems ttProviderSystems, ref string strRequest, string Version = "")
        {
            string strResponse = "";
            // Dim ttService As ttiTravelInsuredService.OtherServices = Nothing

            try
            {
                // ttService = New ttiTravelInsuredService.OtherServices

                // With ttService
                // .Version = Version
                // .XslPath = XslPath
                // .ProviderSystems = ttProviderSystems
                // .Request = strRequest

                // Select Case Service
                // Case ttServices.InsuranceBook
                // strResponse = .InsuranceBook()
                // Case ttServices.InsuranceQuote
                // strResponse = .InsuranceQuote()
                // Case ttServices.Native
                // strResponse = .Native()
                // End Select

                // End With

                return strResponse;
            }

            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                GC.Collect();
            }

        }


        #endregion

        #endregion

        #region  CreateUUID 

        public static string CreateUUID()
        {
            string strGUID;

            strGUID = Strings.UCase(Guid.NewGuid().ToString()).Replace("-", "");

            while (strGUID.LastIndexOf("-", StringComparison.Ordinal) < 20)
                strGUID = strGUID.Insert(strGUID.LastIndexOf("-", StringComparison.Ordinal) + 9, "-");

            return strGUID;

        }

        #endregion

        #region  Log to File 

        public static void LogMessageToFile(int LogType, ref string UUID, ref string WebServer, ref string Customer, ref string UserName, ref string Provider, int MessageID, ref string Message, DateTime MessageDate, int ResponseTime, string ExError)
        {

            int fileNumber;
            string strLine;
            var sb = new StringBuilder();

            try
            {
                sb.Append("<Line>");
                sb.Append("<LogType>").Append(LogType).Append("</LogType>");
                sb.Append("<UUID>").Append(UUID).Append("</UUID>");
                sb.Append("<WebServer>").Append(WebServer).Append("</WebServer>");
                sb.Append("<Customer>").Append(Customer).Append("</Customer>");
                sb.Append("<UserName>").Append(UserName).Append("</UserName>");
                sb.Append("<Provider>").Append(Provider).Append("</Provider>");
                sb.Append("<MessageID>").Append(MessageID).Append("</MessageID>");
                sb.Append("<Message>").Append(Message.Replace(Constants.vbCr, "").Replace(Constants.vbLf, "").Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", "")).Append("</Message>");
                sb.Append("<MessageDate>").Append(MessageDate).Append("</MessageDate>");
                sb.Append("<ResponseTime>").Append(ResponseTime).Append("</ResponseTime>");
                sb.Append("<ExError>").Append(ExError).Append("</ExError>");
                sb.Append("</Line>");
                strLine = sb.ToString();
                sb.Remove(0, sb.Length);

                fileNumber = FileSystem.FreeFile();

                FileSystem.FileOpen(fileNumber, sb.Append(LogPath).Append(string.Format("{0}_Log.txt", DateTime.Now.ToShortDateString())).ToString(), OpenMode.Append);
                sb.Remove(0, sb.Length);
                FileSystem.PrintLine(fileNumber, strLine);
                FileSystem.FileClose(fileNumber);
            }
            catch (Exception ex)
            {
                // 
            }

        }

        public static void LogSoapExceptionToFile(ref string SoapException, ref string SoapEnvelope, string ExError)
        {

            int fileNumber;
            string strLine;
            var sb = new StringBuilder();

            try
            {
                sb.Append("<Line>");
                sb.Append("<LogType>").Append(1).Append("</LogType>");
                sb.Append("<UUID></UUID>");
                sb.Append("<WebServer></WebServer>");
                sb.Append("<Customer></Customer>");
                sb.Append("<UserName></UserName>");
                sb.Append("<Provider></Provider>");
                sb.Append("<MessageID>SoapException</MessageID>");
                sb.Append("<Message>").Append(SoapEnvelope).Append("</Message>");
                sb.Append("<MessageDate>").Append(DateTime.Now).Append("</MessageDate>");
                sb.Append("<ResponseTime></ResponseTime>");
                sb.Append("<ExError>").Append(SoapException).Append(" ").Append(ExError).Append("</ExError>");
                sb.Append("</Line>");
                strLine = sb.ToString();
                sb.Remove(0, sb.Length);

                fileNumber = FileSystem.FreeFile();

                FileSystem.FileOpen(fileNumber, sb.Append(LogPath).Append("SoapException.txt").ToString(), OpenMode.Append);
                sb.Remove(0, sb.Length);
                FileSystem.PrintLine(fileNumber, strLine);
                FileSystem.FileClose(fileNumber);
            }
            catch (Exception ex)
            {
                // 
            }
        }

        #endregion

        #region  Load Variables and Tables in Memory. Create Amadeus Factories. 

        // Initial Block creation for all PCCs
        public static void CreatInitialSessionPool(ref HttpApplicationState oApplication, ref TripXMLProviderSystems ttProviderSystems, string Provider)
        {

            // Dim i As Integer
            Thread iBlockThread;
            AmadeusWSAdapter ttAA;
            Sabre.SabreAdapter ttSA;
            Galileo.GalileoAdapter ttGA;
            int sessionCount = 0;
            cDA oDA;

            try
            {

                oDA = new cDA("ConnectionString");


                if (oDA.CheckInitialPool(ttProviderSystems.PCC, ttProviderSystems.UserID))
                {
                    // If oDA.CheckInitialPool(ttProviderSystems.PCC, ttProviderSystems.UserID, ttProviderSystems.System) Then

                    if (Provider.ToLower() == "amadeusws")
                    {

                        if (ttProviderSystems.System == "Test")
                        {
                            ttProviderSystems.URL = "https://test.webservices.amadeus.com";
                        }
                        else if (ttProviderSystems.System == "Training")
                        {
                            ttProviderSystems.URL = "https://production.webservices.amadeus.com";
                        }
                        else
                        {
                            ttProviderSystems.URL = "https://production.webservices.amadeus.com";
                        }

                        ttAA = new AmadeusWSAdapter(ttProviderSystems, "V1");

                        ttAA.isSOAP2 = ttProviderSystems.SOAP2;
                        ttAA.isSOAP4 = ttProviderSystems.SOAP4;
                        ttAA.GetStoredFares = ttProviderSystems.GetStoredFares;

                        for (int i = 0, loopTo = ttAA.InitialBlock - 1; i <= loopTo; i++)
                        {
                            ttAA.CreateSessionV2();
                            // iBlockThread = New Thread(New ThreadStart(AddressOf ttAA.CreateSessionV2))
                            // iBlockThread.Start()
                            sessionCount += 1;
                        }
                    }
                    else if (Provider.ToLower() == "sabre")
                    {

                        ttSA = new Sabre.SabreAdapter(ttProviderSystems, "V1");

                        for (int i = 0, loopTo1 = ttSA.InitialBlockSize - 1; i <= loopTo1; i++)
                        {
                            iBlockThread = new Thread(new ThreadStart(ttSA.CreateSessionV2));
                            iBlockThread.Start();
                            sessionCount += 1;
                        }
                    }
                    else if (Provider.ToLower() == "galileo" | Provider.ToLower() == "apollo")
                    {

                        ttGA = new Galileo.GalileoAdapter(ttProviderSystems, "V1");

                        for (int i = 0, loopTo2 = ttGA.InitialBlockSize - 1; i <= loopTo2; i++)
                        {
                            iBlockThread = new Thread(new ThreadStart(ttGA.CreateSessionV2));
                            iBlockThread.Start();
                            sessionCount += 1;
                        }


                    }
                    oDA.UpdatePCCSessions(ttProviderSystems.PCC, sessionCount, ttProviderSystems.UserID);
                    // oDA.UpdatePCCSessions(ttProviderSystems.PCC, SessionCount, ttProviderSystems.UserID, ttProviderSystems.System)
                }
            }

            catch (Exception ex)
            {
                throw;
            }
        }

        public static void TripXMLStartUp(ref HttpApplicationState oApplication)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                var start = DateTime.Now;

                TripXMLTools.TripXMLLoad.TripXMLLoadObject();
                TripXMLTools.TripXMLLoad.GetDecodingTables();

                var loadTime = DateTime.Now - start;

                CoreLib.SendTrace("", "TripXMLLoad", $"TripXML was loaded in {string.Format("{0:0.##}", loadTime.TotalSeconds)} seconds", "", "");

                modCore.Trace = true;   // TODO Remove this line once TravelTalkUserSettings is working.
            }

            catch (Exception ex)
            {
                throw new Exception($"Error Starting up Application.{ex.Message}");
            }
            finally
            {
                GC.Collect();
            }

        }

        #endregion

    }
}