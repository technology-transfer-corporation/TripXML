using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Xml;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using PaymentServices;
using TripXML.Core.Models.Base;
using TripXMLMain;
using static TripXMLMain.modCore;
using Microsoft.Extensions.Configuration;

namespace wsTripXML.wsTravelTalk
{
    // ✅ No longer static — converted to injectable service
    // Register in Program.cs:
    //   builder.Services.AddHttpContextAccessor();
    //   builder.Services.AddScoped<modMain>();
    public class modMain
    {
        private readonly IMemoryCache _cache;        // replaces HttpApplicationState
        private readonly IHttpContextAccessor _http; // replaces HttpContext.Current
        private readonly IConfiguration _configuration;

        public modMain(IConfiguration configuration, IMemoryCache cache, IHttpContextAccessor http)
        {
            _configuration = configuration;
            _cache = cache;
            _http = http;
        }

        // ---------------------------------------------------------------
        // Types / Constants (unchanged)
        // ---------------------------------------------------------------

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

        // ---------------------------------------------------------------
        // Message Request
        // ---------------------------------------------------------------

        public void LogResponse(
            ref string strResponse,
            ref TravelTalkCredential ttCredential,
            DateTime StartTime,
            int ttServiceID,
            string ServerName,
            ref string UUID)
        {
            string strResp = strResponse;
            var sb = new StringBuilder();
            try
            {
                if (ttServiceID != 2 & ttServiceID != 6 & ttServiceID != 7 &
                    ttServiceID != 24 & ttServiceID != 25 & ttServiceID != 81 & ttServiceID != 85)
                {
                    var oLog = new cLog();
                    oLog.LogResponse(
                        UUID, ref ServerName, TravelTalkCredential.RequestorID, ttCredential.UserID,
                        sb.Append(ttCredential.Providers[0].Name).Append(" ").Append(ttCredential.System).ToString(),
                        ttServiceID, ref strResp, StartTime);
                    sb.Clear();
                }
            }
            catch (Exception ex)
            {
                // Just ignore — log class will log error to log file
            }
            // ✅ GC.Collect() removed — harmful to .NET Core GC
        }

        // ✅ ref HttpApplicationState oApp removed (was commented out but still used — bug fixed)
        public void PreServiceRequest(
            ref string strRequest,
            ref TravelTalkCredential ttCredential,
            ref TripXMLProviderSystems ttProviderSystems,
            DateTime StartTime,
            int ttServiceID,
            string ServerName,
            ref string UUID,
            string Version = "",
            bool isDefault = false)
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
                            ttCredential = GetTravelTalkDefaultTravelportCredential(ref strRequest, ttServiceID, ttCredential);
                            break;
                        case (int)ttServices.PNRRead:
                            ttCredential = GetTravelTalkDefaultTravelportCredential(ref strRequest, ttServiceID, ttCredential);
                            break;
                        case (int)ttServices.ShowMileage:
                            ttCredential = GetTravelTalkDefaultSabreCredential(ref strRequest, ttServiceID, ttCredential);
                            break;
                        default:
                            ttCredential = GetTravelTalkDefaultAmadeusCredential(ref strRequest, ttServiceID);
                            break;
                    }
                }
                else
                {
                    ttCredential = GetTravelTalkCredential(ref strRequest, ttServiceID);
                }

                // ✅ oApp.Get("ttACL") → _cache.Get<XmlDocument>
                oDoc = _cache.Get<XmlDocument>("ttACL");

                // ✅ Conversions.ToBoolean(oApp.Get(...)) → _cache.Get<bool>
                validateXSDIn = _cache.Get<bool>($"XSD{ttCredential.UserID}In");

                // SQL Message Log
                try
                {
                    if (ttServiceID != 81)
                    {
                        oLog = new cLog();
                        UUID = oLog.LogRequest(
                            ServerName, TravelTalkCredential.RequestorID, ttCredential.UserID,
                            $"{ttCredential.Providers[0].Name} {ttCredential.System}",
                            ttServiceID, ref strRequest, StartTime);
                    }
                    logged = true;
                }
                catch (Exception e)
                {
                    // Just ignore — log class will log error to log file
                }

                if (modCore.Trace)
                    CoreLib.SendTrace(ttCredential.UserID, $"ttMain {ttServiceID}",
                        "============= OTA Request ============= ", strRequest, UUID);

                TripXMLTools.TripXMLLoad.GetProviderSystem(ref ttProviderSystems, ttCredential);

                try
                {
                    if (validateXSDIn)
                        CoreLib.ValidateXML(strRequest, ttServiceID, (int)enSchemaType.Request, ttCredential.UserID, Version);
                }
                catch (Exception exx)
                {
                    throw new Exception($"Invalid Request. Schema Validation Failed.{Environment.NewLine}{exx.Message}");
                }

                ttProviderSystems.LogUUID = UUID;

                if (ttProviderSystems.AmadeusWS == true)
                {
                    ttCredential.Providers[0].Name = "AmadeusWS";
                    // ✅ Collapsed redundant if/else if/else into ternary
                    ttProviderSystems.URL = ttCredential.System == "Test"
                        ? "https://test.webservices.amadeus.com"
                        : "https://production.webservices.amadeus.com";
                }

                if (ttCredential.Providers[0].Name != "Amadeus")
                {
                    if (ttProviderSystems.System is null)
                        throw new Exception(
                            $"Access denied to {ttCredential.Providers[0].Name} - {ttCredential.System} " +
                            $"system. Or invalid provider or PCC ({ttCredential.Providers[0].PCC}).");

                    if (ttCredential.Providers[0].PCC.Trim().Length > 0 &&
                        ttCredential.Providers[0].Name != "Sabre" &&
                        ttCredential.Providers[0].Name != "Galileo")
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
                        CoreLib.SendTrace(ttCredential.UserID, $"ttMain {ttServiceID}",
                            "============= OTA Request ============= ", strRequest, ttProviderSystems.LogUUID);

                    if (UUID is not null)
                    {
                        string argProvider = $"{ttCredential.Providers[0].Name} {ttCredential.System}";
                        LogMessageToFile((int)enLogType.Request, ref UUID, ref ServerName,
                            ref TravelTalkCredential.RequestorID, ref ttCredential.UserID,
                            ref argProvider, ttServiceID, ref strRequest, StartTime, 0, ex.Message);
                    }
                }
                throw;
            }
        }

        // ✅ ref HttpApplicationState oApp removed, replaced by _cache
        public void PreServiceRequestPool(
            ref string strRequest,
            ref TravelTalkCredential ttCredential,
            ref TripXMLProviderSystems ttProviderSystems,
            DateTime StartTime,
            int ttServiceID,
            string ServerName,
            ref string UUID,
            string Version = "")
        {
            XmlDocument oDoc;
            bool validateXSDIn;
            cLog oLog;
            bool logged = false;
            var sb = new StringBuilder();

            try
            {
                ttCredential = GetTravelTalkCredential(ref strRequest, ttServiceID);

                // ✅ oApp.Get → _cache.Get
                oDoc = _cache.Get<XmlDocument>("ttACL");
                validateXSDIn = _cache.Get<bool>($"XSD{ttCredential.UserID}In");

                // SQL Message Log
                try
                {
                    oLog = new cLog();
                    if (ttServiceID != 2 & ttServiceID != 6 & ttServiceID != 7 &
                        ttServiceID != 24 & ttServiceID != 25 & ttServiceID != 81 & ttServiceID != 85)
                    {
                        UUID = oLog.LogRequest(
                            ServerName, TravelTalkCredential.RequestorID, ttCredential.UserID,
                            sb.Append(ttCredential.Providers[0].Name).Append(" ").Append(ttCredential.System).ToString(),
                            ttServiceID, ref strRequest, StartTime);
                        sb.Clear();
                    }
                    else
                    {
                        // Request set to empty — avoid sending data over network for nothing
                        string argMessage = "";
                        UUID = oLog.LogRequest(
                            ServerName, TravelTalkCredential.RequestorID, ttCredential.UserID,
                            sb.Append(ttCredential.Providers[0].Name).Append(" ").Append(ttCredential.System).ToString(),
                            ttServiceID, ref argMessage, StartTime);
                        sb.Clear();
                    }
                    logged = true;
                    ttProviderSystems.AddLog = logged;
                }
                catch (Exception e)
                {
                    // Just ignore — log class will log error to log file
                }

                if (modCore.Trace)
                    CoreLib.SendTrace(ttCredential.UserID,
                        sb.Append("ttMain ").Append(ttServiceID).ToString(),
                        "============= OTA Request ============= ", strRequest, UUID);
                sb.Clear();

                try
                {
                    if (validateXSDIn)
                        CoreLib.ValidateXML(strRequest, ttServiceID, (int)enSchemaType.Request, ttCredential.UserID, Version);
                }
                catch (Exception exx)
                {
                    throw new Exception(sb.Append("Invalid Request. Schema Validation Failed.")
                        .Append(Environment.NewLine).Append(exx.Message).ToString());
                }

                TripXMLTools.TripXMLLoad.GetProviderSystem(ref ttProviderSystems, ttCredential);
                ttProviderSystems.LogUUID = UUID;
            }
            catch (Exception ex)
            {
                if (!logged)
                {
                    if (modCore.Trace)
                        CoreLib.SendTrace(ttCredential.UserID,
                            sb.Append("ttMain ").Append(ttServiceID).ToString(),
                            "============= OTA Request ============= ", strRequest, UUID);
                    sb.Clear();

                    if (UUID is not null)
                    {
                        string argProvider = sb.Append(ttCredential.Providers[0].Name)
                            .Append(" ").Append(ttCredential.System).ToString();
                        LogMessageToFile((int)enLogType.Request, ref UUID, ref ServerName,
                            ref TravelTalkCredential.RequestorID, ref ttCredential.UserID,
                            ref argProvider, ttServiceID, ref strRequest, StartTime, 0, ex.Message);
                        sb.Clear();
                    }
                }
                throw;
            }
        }

        // ✅ No ASP.NET deps — stays static
        public static void PostServiceRequest(
            ref string strResponse,
            bool ValidateXSDOut,
            int ttServiceID,
            string UserID,
            string Version = "")
        {
            var sb = new StringBuilder();
            try
            {
                if (ValidateXSDOut)
                    CoreLib.ValidateXML(strResponse, ttServiceID, (int)enSchemaType.Response, UserID, Version);
            }
            catch (Exception ex)
            {
                throw new Exception(sb.Append("Invalid Response. Schema Validation Failed.")
                    .Append(Environment.NewLine).Append(ex.Message).ToString());
            }
            // ✅ GC.Collect() removed
        }

        // ✅ ref HttpApplicationState oApp removed
        public void ndcPreServiceRequest(
            ref string strRequest,
            ref TravelTalkCredential ttCredential,
            ref TripXMLProviderSystems ttProviderSystems,
            DateTime StartTime,
            int ttServiceID,
            string ServerName,
            ref string UUID,
            POS POS,
            string Version = "")
        {
            XmlDocument oDoc = null;
            bool ValidateXSDIn;
            cLog oLog = null;
            bool Logged = false;
            var sb = new StringBuilder();

            try
            {
                ttCredential = ndcGetTravelTalkCredential(ref strRequest, ttServiceID, POS);

                // ✅ oApp.Get → _cache.Get
                oDoc = _cache.Get<XmlDocument>("ttACL");
                ValidateXSDIn = _cache.Get<bool>(
                    sb.Append("XSD").Append(ttCredential.UserID).Append("In").ToString());
                sb.Clear();

                if (modCore.Trace)
                    CoreLib.SendTrace(ttCredential.UserID,
                        sb.Append("ttMain ").Append(ttServiceID).ToString(),
                        "============= OTA Request ============= ", strRequest, ttProviderSystems.LogUUID);
                sb.Clear();

                // SQL Message Log
                try
                {
                    oLog = new cLog();
                    if (ttServiceID != 2 & ttServiceID != 6 & ttServiceID != 7 &
                        ttServiceID != 24 & ttServiceID != 25 & ttServiceID != 81 & ttServiceID != 85)
                    {
                        UUID = oLog.LogRequest(
                            ServerName, TravelTalkCredential.RequestorID, ttCredential.UserID,
                            sb.Append(ttCredential.Providers[0].Name).Append(" ").Append(ttCredential.System).ToString(),
                            ttServiceID, ref strRequest, StartTime);
                        sb.Clear();
                    }
                    else
                    {
                        string argMessage = "";
                        UUID = oLog.LogRequest(
                            ServerName, TravelTalkCredential.RequestorID, ttCredential.UserID,
                            sb.Append(ttCredential.Providers[0].Name).Append(" ").Append(ttCredential.System).ToString(),
                            ttServiceID, ref argMessage, StartTime);
                        sb.Clear();
                    }
                    Logged = true;
                }
                catch (Exception e)
                {
                    // Just ignore
                }

                try
                {
                    if (ValidateXSDIn)
                        CoreLib.ValidateXML(strRequest, ttServiceID, (int)enSchemaType.Request, ttCredential.UserID, Version);
                }
                catch (Exception exx)
                {
                    // ✅ Removed unreachable sb.Clear() after throw
                    throw new Exception(sb.Append("Invalid Request. Schema Validation Failed.")
                        .Append(Environment.NewLine).Append(exx.Message).ToString());
                }

                ndcAuthenticateUser(ref oDoc, ttCredential);
            }
            catch (Exception ex)
            {
                if (!Logged)
                {
                    if (modCore.Trace)
                        CoreLib.SendTrace(ttCredential.UserID,
                            sb.Append("ttMain ").Append(ttServiceID).ToString(),
                            "============= OTA Request ============= ", strRequest, ttProviderSystems.LogUUID);
                    sb.Clear();

                    if (UUID is not null)
                    {
                        string argProvider = sb.Append(ttCredential.Providers[0].Name)
                            .Append(" ").Append(ttCredential.System).ToString();
                        LogMessageToFile((int)enLogType.Request, ref UUID, ref ServerName,
                            ref TravelTalkCredential.RequestorID, ref ttCredential.UserID,
                            ref argProvider, ttServiceID, ref strRequest, StartTime, 0, ex.Message);
                        sb.Clear();
                    }
                }
                throw; // ✅ throw (not throw ex) — preserves original stack trace
            }
        }

        public void LogDeals(ref string strRequest, ref string strResponse)
        {
            try
            {
                var oLogData = new cLogData(_configuration);
                oLogData.LogDataDeals(strRequest, strResponse);
            }
            catch (Exception ex)
            {
                // Just ignore
            }
        }

        public string GetDeals(ref string strRequest)
        {
            string strResponse = "";
            try
            {
                var oLogData = new cLogData(_configuration);
                strResponse = oLogData.GetDataDeals(strRequest);
            }
            catch (Exception ex)
            {
                // Just ignore
            }
            return strResponse;
        }

        // ---------------------------------------------------------------
        // Authentication
        // ---------------------------------------------------------------

        public TravelTalkCredential GetTravelTalkDefaultTravelportCredential(
            ref string strRequest, int ttServiceID, TravelTalkCredential ttOldCredential)
        {
            TravelTalkCredential ttCredential = default;
            XmlDocument oReqDoc;
            XmlElement oRoot;
            XmlNode oNodePOS;
            int i, count;
            bool customErr = false;
            string strError;
            var sb = new StringBuilder();

            // ✅ HttpContext.Current.Request.UserHostName → IHttpContextAccessor
            var hostName = _http.HttpContext?.Request.Host.Host ?? Environment.MachineName;
            strRequest = strRequest.Replace("URL=\"\"",
                sb.Append("URL=\"").Append(hostName).Append("\"").ToString());
            sb.Clear();

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
                    throw new Exception(sb.Append("Invalid or empty request.")
                        .Append(Environment.NewLine).Append(strRequest).ToString());
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

                if (ttCredential.UserID == "FlightSite" &&
                    !strRequest.Contains("<VendorPref Code=") &&
                    (ttServiceID == 6 | ttServiceID == 7))
                {
                    strRequest = strRequest.Replace("<System>", "<Name>Amadeus</Name><System>");
                    oReqDoc.LoadXml(strRequest);
                    oRoot = oReqDoc.DocumentElement;
                    oNodePOS = oRoot.SelectSingleNode("POS");
                }

                count = oNodePOS.SelectNodes("TPA_Extensions/Provider/Name").Count - 1;
                ttCredential.Providers = new modCore.Provider[count + 1];
                for (i = 0; i <= count; i++)
                    ttCredential.Providers[i].PCC = "";

                ttCredential.Providers[0].PCC = ttOldCredential.Providers[0].PCC;
                ttCredential.Providers[0].Name = "Travelport";
                oNodePOS = oNodePOS.SelectSingleNode("TPA_Extensions/Provider");
            }
            catch (Exception ex)
            {
                strError = customErr ? ex.Message : "Error Loading User Credentials. POS node is missing or incomplete.";
                throw new Exception(strError);
            }

            return ttCredential;
        }

        public TravelTalkCredential GetTravelTalkDefaultSabreCredential(
            ref string request, int ttServiceID, TravelTalkCredential ttOldCredential)
        {
            TravelTalkCredential ttCredential = default;
            XmlDocument oReqDoc;
            XmlElement oRoot;
            XmlNode oNodePOS;
            int i, count;
            bool customErr = false;
            string strError;
            var sb = new StringBuilder();

            var hostName = _http.HttpContext?.Request.Host.Host ?? Environment.MachineName;
            request = request.Replace("URL=\"\"",
                sb.Append("URL=\"").Append(hostName).Append("\"").ToString());
            sb.Clear();

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
                    throw new Exception(sb.Append("Invalid or empty request.")
                        .Append(Environment.NewLine).Append(request).ToString());
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
                for (i = 0; i <= count; i++)
                    ttCredential.Providers[i].PCC = "";

                ttCredential.Providers[0].PCC = ttOldCredential.Providers[0].PCC;
                ttCredential.Providers[0].Name = "Sabre";
                oNodePOS = oNodePOS.SelectSingleNode("TPA_Extensions/Provider");
            }
            catch (Exception ex)
            {
                strError = customErr ? ex.Message : "Error Loading User Credentials. POS node is missing or incomplete.";
                throw new Exception(strError);
            }

            return ttCredential;
        }

        public TravelTalkCredential GetTravelTalkDefaultAmadeusCredential(
            ref string strRequest, int ttServiceID)
        {
            TravelTalkCredential ttCredential = default;
            XmlDocument oReqDoc;
            XmlElement oRoot;
            XmlNode oNodePOS;
            int i, count;
            bool customErr = false;
            string strError;
            var sb = new StringBuilder();

            var hostName = _http.HttpContext?.Request.Host.Host ?? Environment.MachineName;
            strRequest = strRequest.Replace("URL=\"\"",
                sb.Append("URL=\"").Append(hostName).Append("\"").ToString());
            sb.Clear();

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
                    throw new Exception(sb.Append("Invalid or empty request.")
                        .Append(Environment.NewLine).Append(strRequest).ToString());
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

                if (ttCredential.UserID == "FlightSite" &&
                    !strRequest.Contains("<VendorPref Code=") &&
                    (ttServiceID == 6 | ttServiceID == 7))
                {
                    strRequest = strRequest.Replace("<System>", "<Name>Amadeus</Name><System>");
                    oReqDoc.LoadXml(strRequest);
                    oRoot = oReqDoc.DocumentElement;
                    oNodePOS = oRoot.SelectSingleNode("POS");
                }

                count = oNodePOS.SelectNodes("TPA_Extensions/Provider/Name").Count - 1;
                ttCredential.Providers = new modCore.Provider[count + 1];
                for (i = 0; i <= count; i++)
                    ttCredential.Providers[i].PCC = "";

                ttCredential.Providers[0].PCC = "NYC1S211F";
                ttCredential.Providers[0].Name = "Amadeus";
                oNodePOS = oNodePOS.SelectSingleNode("TPA_Extensions/Provider");
            }
            catch (Exception ex)
            {
                strError = customErr ? ex.Message : "Error Loading User Credentials. POS node is missing or incomplete.";
                throw new Exception(strError);
            }

            return ttCredential;
        }

        public TravelTalkCredential GetTravelTalkCredential(ref string strRequest, int ttServiceID)
        {
            TravelTalkCredential ttCredential = default;
            XmlDocument oReqDoc;
            XmlElement oRoot;
            XmlNode oNodePOS;
            int i, count;
            bool customErr = false;
            string strError;

            var hostName = _http.HttpContext?.Request.Host.Host ?? Environment.MachineName;
            strRequest = strRequest.Replace("URL=\"\"", $"URL=\"{hostName}\"");

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

                if (ttCredential.UserID == "FlightSite" &&
                    !strRequest.Contains("<VendorPref Code=") &&
                    (ttServiceID == 6 | ttServiceID == 7))
                {
                    strRequest = strRequest.Replace("<System>", "<Name>Amadeus</Name><System>");
                    oReqDoc.LoadXml(strRequest);
                    oRoot = oReqDoc.DocumentElement;
                    oNodePOS = oRoot.SelectSingleNode("POS");
                }

                count = oNodePOS.SelectNodes("TPA_Extensions/Provider/Name").Count - 1;
                ttCredential.Providers = new modCore.Provider[count + 1];
                for (i = 0; i <= count; i++)
                    ttCredential.Providers[i].PCC = "";

                if (oNodePOS.SelectSingleNode("Source").Attributes["PseudoCityCode"] is null)
                    ttCredential.Providers[0].PCC = "";
                else
                    ttCredential.Providers[0].PCC = oNodePOS.SelectSingleNode("Source").Attributes["PseudoCityCode"].Value;

                oNodePOS = oNodePOS.SelectSingleNode("TPA_Extensions/Provider");
                for (i = 0; i <= count; i++)
                {
                    ttCredential.Providers[i].Name = oNodePOS.SelectNodes("Name").Item(i).InnerText;
                    if (oNodePOS.SelectNodes("Name").Item(i).Attributes["PseudoCityCode"] is not null)
                    {
                        if (oNodePOS.SelectNodes("Name").Item(i).Attributes["PseudoCityCode"].Value.Trim().Length > 0)
                            ttCredential.Providers[i].PCC = oNodePOS.SelectNodes("Name").Item(i)
                                .Attributes["PseudoCityCode"].Value.Trim();
                    }
                    else if (i > 0)
                    {
                        ttCredential.Providers[i].PCC = "*" + ttCredential.Providers[i - 1].PCC;
                    }
                }
            }
            catch (Exception ex)
            {
                strError = customErr ? ex.Message : "Error Loading User Credentials. POS node is missing or incomplete.";
                throw new Exception(strError);
            }

            return ttCredential;
        }

        public TravelTalkCredential ndcGetTravelTalkCredential(
            ref string strRequest, int ttServiceID, POS POS)
        {
            TravelTalkCredential ttCredential = default;
            int i, Count;
            bool CustomErr = false;
            string strError = "";
            var sb = new StringBuilder();

            var hostName = _http.HttpContext?.Request.Host.Host ?? Environment.MachineName;
            strRequest = strRequest.Replace("URL=\"\"",
                sb.Append("URL=\"").Append(hostName).Append("\"").ToString());
            sb.Clear();

            try
            {
                if (POS.Source.First()?.RequestorID.ID is not null)
                    TravelTalkCredential.RequestorID = POS.Source.First().RequestorID.ID.ToString();

                if (POS.TPA_Extensions.Provider.GDSSystem is not null)
                    ttCredential.System = POS.TPA_Extensions.Provider.GDSSystem.ToString();

                if (POS.TPA_Extensions.Provider.Userid is not null)
                    ttCredential.UserID = POS.TPA_Extensions.Provider.Userid.ToString();

                if (POS.TPA_Extensions.Provider.Password is not null)
                    ttCredential.Password = POS.TPA_Extensions.Provider.Password.ToString();

                Count = POS.TPA_Extensions.Provider.Name.Length;
                ttCredential.Providers = new modCore.Provider[Count];
                ttCredential.Providers[0].PCC = POS.Source.First().PseudoCityCode.ToString();

                for (i = 0; i <= Count - 1; i++)
                {
                    ttCredential.Providers[i].Name = POS.TPA_Extensions.Provider.Name[i].Value;
                    if (POS.TPA_Extensions.Provider.Name[i].PseudoCityCode is not null)
                    {
                        if (!string.IsNullOrEmpty(POS.TPA_Extensions.Provider.Name[i].PseudoCityCode.ToString()))
                            ttCredential.Providers[i].PCC = POS.TPA_Extensions.Provider.Name[i].PseudoCityCode.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                strError = CustomErr ? ex.Message : "Error Loading User Credentials. POS node Is missing Or incomplete.";
                throw new Exception(strError);
            }
            // ✅ sb = null removed

            return ttCredential;
        }

        public static void AuthenticateUser(ref XmlDocument oDoc, TravelTalkCredential ttCredential)
        {
            XmlElement oRoot;
            XmlNode oNode;
            var sb = new StringBuilder();

            oRoot = oDoc.DocumentElement;
            oNode = oRoot.SelectSingleNode(
                sb.Append("Customer[@RequestorID='").Append(TravelTalkCredential.RequestorID).Append("']").ToString());
            sb.Clear();

            if (oNode is null)
            {
                throw new Exception(sb.Append("Customer ").Append(TravelTalkCredential.RequestorID)
                    .Append(" is not valid.").ToString());
            }
            else
            {
                oNode = oNode.SelectSingleNode(
                    sb.Append("User[Username='").Append(ttCredential.UserID).Append("']").ToString());
                sb.Clear();
                if (oNode is null)
                    throw new Exception(sb.Append("User  ").Append(ttCredential.UserID)
                        .Append(" is not valid for Customer ").Append(TravelTalkCredential.RequestorID).ToString());
            }

            // ✅ Strings.StrComp → string.Equals
            if (!string.Equals(ttCredential.Password, oNode["Password"].InnerText, StringComparison.Ordinal))
            {
                throw new Exception(sb.Append("Password ").Append(ttCredential.Password)
                    .Append(" is not valid for User ").Append(ttCredential.UserID).ToString());
            }
            // ✅ DateAndTime.DateDiff + Conversions.ToDate → DateTime arithmetic
            else if ((DateTime.Today - DateTime.Parse(oNode.SelectSingleNode("Services/Start").InnerText)).TotalDays < 0)
            {
                throw new Exception(sb.Append("Access Denied. Services will start on ")
                    .Append(oNode.SelectSingleNode("Services/Start").InnerText).ToString());
            }
            else if ((DateTime.Today - DateTime.Parse(oNode.SelectSingleNode("Services/End").InnerText)).TotalDays > 0)
            {
                throw new Exception(sb.Append("Access Denied. Services expired on ")
                    .Append(oNode.SelectSingleNode("Services/End").InnerText).ToString());
            }
        }

        public static void ndcAuthenticateUser(ref XmlDocument oDoc, TravelTalkCredential ttCredential)
        {
            XmlElement oRoot = null;
            XmlNode oNode = null;
            var sb = new StringBuilder();

            oRoot = oDoc.DocumentElement;
            oNode = oRoot.SelectSingleNode(
                sb.Append("Customer[@RequestorID='").Append(TravelTalkCredential.RequestorID).Append("']").ToString());
            sb.Clear();

            if (oNode is null)
            {
                // ✅ Removed all unreachable sb.Clear() calls after throw
                throw new Exception(sb.Append("Customer ").Append(TravelTalkCredential.RequestorID)
                    .Append(" is not valid.").ToString());
            }
            else
            {
                oNode = oNode.SelectSingleNode(
                    sb.Append("User[Username='").Append(ttCredential.UserID).Append("']").ToString());
                sb.Clear();
                if (oNode is null)
                    throw new Exception(sb.Append("User  ").Append(ttCredential.UserID)
                        .Append(" is not valid for Customer ").Append(TravelTalkCredential.RequestorID).ToString());
            }

            if (!string.Equals(ttCredential.Password, oNode["Password"].InnerText, StringComparison.Ordinal))
            {
                throw new Exception(sb.Append("Password ").Append(ttCredential.Password)
                    .Append(" is not valid for User ").Append(ttCredential.UserID).ToString());
            }
            else if ((DateTime.Today - DateTime.Parse(oNode.SelectSingleNode("Services/Start").InnerText)).TotalDays < 0)
            {
                throw new Exception(sb.Append("Access Denied. Services will start on ")
                    .Append(oNode.SelectSingleNode("Services/Start").InnerText).ToString());
            }
            else if ((DateTime.Today - DateTime.Parse(oNode.SelectSingleNode("Services/End").InnerText)).TotalDays > 0)
            {
                throw new Exception(sb.Append("Access Denied. Services expired on ")
                    .Append(oNode.SelectSingleNode("Services/End").InnerText).ToString());
            }
        }

        // ---------------------------------------------------------------
        // Get Decode Values  (no ASP.NET deps — all stay static)
        // ---------------------------------------------------------------

        public static string GetDecodeValue(ref DataView oDV, ref string strCode)
        {
            try
            {
                if (oDV is null) return string.Empty;

                foreach (DataRow row in oDV.Table.Rows)
                {
                    if (row["Code"].ToString().Trim().ToUpper().Equals(strCode.Trim().ToUpper()))
                        return row["Name"].ToString();
                    if (row["Code"].ToString().Trim().ToUpper().Contains(strCode.Trim().ToUpper()))
                        return row["Name"].ToString();
                }

                var elems = strCode.Split(' ').ToList();
                foreach (string word in elems)
                {
                    foreach (DataRow row in oDV.Table.Rows)
                    {
                        if (row["Code"].ToString().Trim().ToUpper().Contains(strCode.Trim().ToUpper()))
                            return row["Name"].ToString();
                    }
                }

                return string.Empty;
            }
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
                        return row["Code"].ToString();
                    if (row["ID"].ToString().Trim().ToUpper().Contains(strID.Trim().ToUpper()))
                        return row["Code"].ToString();
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
                if (string.IsNullOrEmpty(strName)) return string.Empty;

                if (strName.Contains("OPERATED BY"))
                    strName = strName.Replace("OPERATED BY ", "");

                foreach (DataRow row in oDV.Table.Rows)
                {
                    if (row["Name"].ToString().Trim().ToUpper().Equals(strName.Trim().ToUpper()))
                        return row["Code"].ToString();
                }

                foreach (DataRow row in oDV.Table.Rows)
                {
                    if (row["Name"].ToString().Trim().ToUpper().Contains(strName.Trim().ToUpper()))
                        return row["Code"].ToString();
                }

                strName = strName.ToUpper().Replace("AIR LINES", "AIRLINES");
                string[] lstAirName = strName.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                string shortName = string.Empty;

                if (strName.ToUpper().Contains(" AS "))
                {
                    foreach (string word in lstAirName)
                    {
                        if (word.Equals("AS")) break;
                        foreach (DataRow row in oDV.Table.Rows)
                        {
                            if (row["Name"].ToString().Trim().ToUpper().Equals(word.Trim().ToUpper()))
                                return row["Code"].ToString();
                            if (row["Name"].ToString().Trim().ToUpper().Contains(word.Trim().ToUpper()))
                                return row["Code"].ToString();
                        }
                    }
                }
                else
                {
                    int lastIndex = lstAirName.Length - 1;
                    // ✅ Information.IsNumeric → double.TryParse
                    if (double.TryParse(lstAirName.Last(), out _) && lstAirName[lastIndex - 1].Length == 2)
                        return lstAirName[lastIndex - 1];
                    if (!double.TryParse(lstAirName[lastIndex], out _) && lstAirName[lastIndex].Length == 2)
                        return lstAirName.Last();
                }

                foreach (string word in lstAirName)
                {
                    if (double.TryParse(word, out _)) continue;
                    if (word.Length > 2) shortName += $" {word}";
                    if (word.ToUpper().Equals("AIRLINES")) break;
                }

                foreach (DataRow row in oDV.Table.Rows)
                {
                    if (row["Name"].ToString().Trim().ToUpper().Equals(shortName.Trim().ToUpper()))
                        return row["Code"].ToString();
                    if (row["Name"].ToString().Trim().ToUpper().Contains(shortName.Trim().ToUpper()))
                        return row["Code"].ToString();
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public static bool IsDecodeValue(ref DataView oDV, ref string strCode) =>
            oDV.Find(strCode) > -1;

        public static bool IsCruiseFilterValue(ref DataView oDV, string strCruise, string strCode)
        {
            var oVals = new object[] { strCruise, strCode };
            return oDV.Find(oVals) > -1;
        }

        public static string GetCruiseFilterValue(ref DataView oDV, string strCruise, string strCode)
        {
            var oVals = new object[] { strCruise, strCode };
            int i = oDV.Find(oVals);
            return i > -1 ? oDV[i]["value"].ToString() : "";
        }

        public static object IsNothing(object Item, object Replace)
        {
            if (Item is null) return Replace;
            if (Item is XmlNode node) return node.Value;
            return Item;
        }

        // ---------------------------------------------------------------
        // Send Request to Providers — Air Services
        // (no ASP.NET deps — all stay static, GC.Collect() removed)
        // ---------------------------------------------------------------

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
                    case ttServices.AirFlifo: strResponse = ttService.AirFlifo(); break;
                    case ttServices.AirPrice: strResponse = ttService.AirPrice(); break;
                    case ttServices.AirRules: strResponse = ttService.AirRules(); break;
                    case ttServices.AirSeatMap: strResponse = ttService.AirSeatMap(); break;
                    case ttServices.LowFarePlus: strResponse = ttService.LowFarePlus(); break;
                    case ttServices.FareDisplay: strResponse = ttService.FareDisplay(); break;
                    default: throw new Exception($"{Service} Message is not supported by Galileo.");
                }
                return strResponse;
            }
            catch (Exception ex) { throw; }
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
                        if (!ttProviderSystems.AmadeusWS | ttProviderSystems.AmadeusWS &
                            !string.IsNullOrEmpty(ttProviderSystems.AmadeusWSSchema[enAmadeusWSSchema.Air_FlightInfo]))
                            strResponse = ttService.AirFlifo();
                        else
                            throw new Exception("Air_FlightInfo not authorized");
                        break;
                    case ttServices.AirPrice: strResponse = ttService.AirPrice(); break;
                    case ttServices.AirRules: strResponse = ttService.AirRules(); break;
                    case ttServices.AirSeatMap: strResponse = ttService.AirSeatMap(); break;
                    case ttServices.FareInfo: strResponse = ttService.FareInfo(); break;
                    case ttServices.FareDisplay: strResponse = ttService.FareDisplay(); break;
                    case ttServices.AirSchedule: strResponse = ttService.AirSchedule(); break;
                    default: throw new Exception($"{Service} Message is not supported by Amadeus.");
                }
                return strResponse;
            }
            catch (Exception ex) { throw; }
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
                    case ttServices.AirFlifo: strResponse = ttService.AirFlifo(); break;
                    case ttServices.AirPrice: strResponse = ttService.AirPrice(); break;
                    case ttServices.AirRules: strResponse = ttService.AirRules(); break;
                    case ttServices.AirSeatMap: strResponse = ttService.AirSeatMap(); break;
                    case ttServices.FareDisplay: strResponse = ttService.FareDisplay(); break;
                    default: throw new Exception($"{Service} Message is not supported by Sabre.");
                }
                return strResponse;
            }
            catch (Exception ex) { throw; }
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
                    case ttServices.AirPrice: strResponse = ttService.AirPrice(); break;
                    case ttServices.AirRules: strResponse = ttService.AirRules(); break;
                    case ttServices.AirSeatMap: strResponse = ttService.AirSeatMap(); break;
                    case ttServices.FareDisplay: strResponse = ttService.FareDisplay(); break;
                    default: throw new Exception($"{Service} Message is not supported by Worldspan.");
                }
                return strResponse;
            }
            catch (Exception ex) { throw; }
        }

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
                    case ttServices.AirFlifo: strResponse = ttService.AirFlifo(); break;
                    case ttServices.AirPrice: strResponse = ttService.AirPrice(); break;
                    case ttServices.AirRules: strResponse = ttService.AirRules(); break;
                    case ttServices.AirSeatMap: strResponse = ttService.AirSeatMap(); break;
                    case ttServices.LowFarePlus: strResponse = ttService.LowFarePlus(); break;
                    case ttServices.FareDisplay: strResponse = ttService.FareDisplay(); break;
                    default: throw new Exception($"{Service} Message is not supported by Galileo.");
                }
                return strResponse;
            }
            catch (Exception ex) { throw; }
        }

        // ---------------------------------------------------------------
        // Send Request to Providers — Car Services
        // ---------------------------------------------------------------

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
                    case ttServices.CarInfo: strResponse = ttService.CarInfo(); break;
                }
                return strResponse;
            }
            catch (Exception ex) { throw; }
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
                    case ttServices.CarInfo: strResponse = ttService.CarInfo(); break;
                    case ttServices.CarRules: strResponse = ttService.CarRules(); break;
                    case ttServices.CarList: strResponse = ttService.CarList(); break;
                }
                return strResponse;
            }
            catch (Exception ex) { throw; }
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
                    case ttServices.CarInfo: strResponse = ttService.CarInfo(); break;
                }
                return strResponse;
            }
            catch (Exception ex) { throw; }
        }

        // ---------------------------------------------------------------
        // Send Request to Providers — Hotel Services
        // ---------------------------------------------------------------

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
                    case ttServices.HotelInfo: strResponse = ttService.HotelInfo(); break;
                }
                return strResponse;
            }
            catch (Exception ex) { throw; }
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
                    case ttServices.HotelInfo: strResponse = ttService.HotelInfo(); break;
                }
                return strResponse;
            }
            catch (Exception ex) { throw; }
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
                    case ttServices.HotelInfo: strResponse = ttService.HotelInfo(); break;
                }
                return strResponse;
            }
            catch (Exception ex) { throw; }
        }

        // ---------------------------------------------------------------
        // Send Request to Providers — PNR Services
        // ---------------------------------------------------------------

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
                    case ttServices.PNRRead: strResponse = ttService.PNRRead(); break;
                    case ttServices.PNRCancel: strResponse = ttService.PNRCancel(); break;
                    case ttServices.Queue: strResponse = ttService.Queue(); break;
                    case ttServices.QueueRead: strResponse = ttService.QueueRead(); break;
                    case ttServices.PNRReprice: strResponse = ttService.PNRReprice(); break;
                    case ttServices.PNREnd: strResponse = ttService.PNREnd(); break;
                    default: throw new Exception($"{Service} Message is not supported by Galileo.");
                }
                return strResponse;
            }
            catch (Exception ex) { throw; }
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
                    case ttServices.PNRRead: strResponse = ttService.PNRRead(); break;
                    case ttServices.PNREnd: strResponse = ttService.PNREnd(); break;
                    case ttServices.PNRCancel: strResponse = ttService.PNRCancel(); break;
                    case ttServices.Queue: strResponse = ttService.Queue(); break;
                    case ttServices.QueueRead: strResponse = ttService.QueueRead(); break;
                    case ttServices.PNRReprice: strResponse = ttService.PNRReprice(); break;
                    case ttServices.PNRSplit: strResponse = ttService.PNRSplit(); break;
                    case ttServices.SearchName: strResponse = ttService.SearchName(); break;
                    case ttServices.TransferOwnership: strResponse = ttService.TransferOwnership(); break;
                    default: throw new Exception($"{Service} Message is not supported by Amadeus.");
                }
                return strResponse;
            }
            catch (Exception ex) { throw; }
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
                    case ttServices.PNRRead: strResponse = ttService.PNRRead(); break;
                    case ttServices.PNRCancel: strResponse = ttService.PNRCancel(); break;
                    case ttServices.PNRReprice: strResponse = ttService.PNRReprice(); break;
                    case ttServices.Queue: strResponse = ttService.Queue(); break;
                    case ttServices.QueueRead: strResponse = ttService.QueueRead(); break;
                    case ttServices.PNREnd: strResponse = ttService.PNREnd(); break;
                    default: throw new Exception($"{Service} Message is not supported by Sabre.");
                }
                return strResponse;
            }
            catch (Exception ex) { throw; }
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
                    case ttServices.PNRRead: strResponse = ttService.PNRRead(); break;
                    case ttServices.PNRCancel: strResponse = ttService.PNRCancel(); break;
                    case ttServices.Queue: strResponse = ttService.Queue(); break;
                    case ttServices.PNRReprice: strResponse = ttService.PNRReprice(); break;
                    case ttServices.PNREnd: strResponse = ttService.PNREnd(); break;
                    default: throw new Exception($"{Service} Message is not supported by Worldspan.");
                }
                return strResponse;
            }
            catch (Exception ex) { throw; }
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
                switch (Service)
                {
                    case ttServices.PNRRead: strResponse = ttService.PNRRead(); break;
                    case ttServices.PNRReprice: strResponse = ttService.PNRReprice(); break;
                    case ttServices.Queue: strResponse = ttService.Queue(); break;
                }
                return strResponse;
            }
            catch (Exception ex) { throw; }
        }

        // ---------------------------------------------------------------
        // Send Request to Providers — Travel Services
        // ---------------------------------------------------------------

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
                    case ttServices.TravelBuild: strResponse = ttService.TravelBuild(); break;
                    case ttServices.TravelModify: strResponse = ttService.TravelModify(); break;
                    case ttServices.IssueTicket: strResponse = ttService.IssueTicket(); break;
                    case ttServices.IssueTicketSessioned: strResponse = ttService.IssueTicketSessioned(); break;
                    case ttServices.Update: strResponse = ttService.Update(); break;
                    case ttServices.UpdateSessioned: strResponse = ttService.UpdateSessioned(); break;
                    case ttServices.TicketVoid: strResponse = ttService.VoidTicket(); break;
                    case ttServices.IssueMCO: strResponse = ttService.IssueMCO(); break;
                    default: throw new Exception($"{Service} Message is not supported by Galileo.");
                }
                return strResponse;
            }
            catch (Exception ex) { throw; }
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
                    case ttServices.TravelBuild: strResponse = ttService.TravelBuild(); break;
                    case ttServices.IssueTicket: strResponse = ttService.IssueTicket(); break;
                    case ttServices.IssueTicketSessioned: strResponse = ttService.IssueTicketSessioned(); break;
                    case ttServices.StoredFareBuild: strResponse = ttService.StoredFareBuild(); break;
                    case ttServices.StoredFareUpdate: strResponse = ttService.StoredFareUpdate(); break;
                    case ttServices.Update: strResponse = ttService.Update(); break;
                    case ttServices.UpdateSessioned: strResponse = ttService.UpdateSessioned(); break;
                    case ttServices.TicketVoid: strResponse = ttService.VoidTicket(); break;
                    case ttServices.TicketDisplay: strResponse = ttService.DisplayTicket(); break;
                    case ttServices.RefundTicket: strResponse = ttService.RefundTicket(); break;
                    case ttServices.ReissueTicket: strResponse = ttService.ReissueTicket(); break;
                    default: throw new Exception($"{Service} Message is not supported by Amadeus.");
                }
                return strResponse;
            }
            catch (Exception ex) { throw; }
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
                        if (Version == "v04") strResponse = ttService.TravelBuild_V4();
                        break;
                    case ttServices.IssueTicket: strResponse = ttService.IssueTicket(); break;
                    case ttServices.IssueTicketSessioned: strResponse = ttService.IssueTicketSessioned(); break;
                    case ttServices.Update: strResponse = ttService.Update(); break;
                    case ttServices.UpdateSessioned: strResponse = ttService.UpdateSessioned(); break;
                    case ttServices.IssueMCO: strResponse = ttService.IssueMCO(); break;
                    default: throw new Exception($"{Service} Message is not supported by Sabre.");
                }
                return strResponse;
            }
            catch (Exception ex) { throw; }
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
                    case ttServices.TravelBuild: strResponse = ttService.TravelBuild(); break;
                    case ttServices.Update: strResponse = ttService.Update(); break;
                    case ttServices.UpdateSessioned: strResponse = ttService.UpdateSessioned(); break;
                    case ttServices.IssueTicketSessioned: strResponse = ttService.IssueTicketSessioned(); break;
                    case ttServices.Authorization: strResponse = ttService.Authorization(); break;
                    default: throw new Exception($"{Service} Message is not supported by Worldspan.");
                }
                return strResponse;
            }
            catch (Exception ex) { throw; }
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
                    case ttServices.TravelBuild: strResponse = ttService.TravelBuild(); break;
                    case ttServices.IssueTicket: break; // not yet implemented
                    case ttServices.IssueTicketSessioned: break; // not yet implemented
                    case ttServices.Update: break; // not yet implemented
                    case ttServices.UpdateSessioned: strResponse = ttService.UpdateSessioned(); break;
                    default: throw new Exception($"{Service} Message is not supported by Travelport.");
                }
                return strResponse;
            }
            catch (Exception ex) { throw; }
        }

        // ---------------------------------------------------------------
        // Send Request to Providers — Other Services
        // ---------------------------------------------------------------

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
                    case ttServices.CreateSession: strResponse = ttService.CreateSession(); break;
                    case ttServices.CloseSession: strResponse = ttService.CloseSession(); break;
                    case ttServices.ShowMileage: strResponse = ttService.ShowMileage(); break;
                    case ttServices.CCValid: strResponse = ttService.CreditCardValid(); break;
                    case ttServices.CurConv: strResponse = ttService.CurrencyConvertion(); break;
                    case ttServices.TimeDiff: strResponse = ttService.TimeDifference(); break;
                    case ttServices.Cryptic: strResponse = ttService.Cryptic(); break;
                    case ttServices.Native: strResponse = ttService.Native(); break;
                    case ttServices.ETicketVerify: strResponse = ttService.ETicketVerify(); break;
                    case ttServices.MultiMessage: strResponse = ttService.MultiMessage(); break;
                    case ttServices.ProfileRead: strResponse = ttService.ProfileRead(); break;
                    case ttServices.ProfileCreate: strResponse = ttService.ProfileCreate(); break;
                    default: throw new Exception($"{Service} Message is not supported by Galileo.");
                }
                return strResponse;
            }
            catch (Exception ex) { throw; }
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
                    case ttServices.CreateSession: strResponse = ttService.CreateSession(); break;
                    case ttServices.CloseSession: strResponse = ttService.CloseSession(); break;
                    case ttServices.ShowMileage: strResponse = ttService.ShowMileage(); break;
                    case ttServices.CCValid: strResponse = ttService.CreditCardValid(); break;
                    case ttServices.CurConv: strResponse = ttService.CurrencyConvertion(); break;
                    case ttServices.TimeDiff: strResponse = ttService.TimeDifference(); break;
                    case ttServices.Cryptic: strResponse = ttService.Cryptic(); break;
                    case ttServices.SalesReport: strResponse = ttService.SalesReport(); break;
                    case ttServices.Native: strResponse = ttService.Native(); break;
                    case ttServices.TripXMLNative: strResponse = ttService.TripXMLNative(); break;
                    default: throw new Exception($"{Service} Message is not supported by Amadeus.");
                }
                return strResponse;
            }
            catch (Exception ex) { throw; }
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
                    case ttServices.CreateSession: strResponse = ttService.CreateSession(); break;
                    case ttServices.CloseSession: strResponse = ttService.CloseSession(); break;
                    case ttServices.ShowMileage: strResponse = ttService.ShowMileage(); break;
                    case ttServices.CCValid: strResponse = ttService.CreditCardValid(); break;
                    case ttServices.CurConv: strResponse = ttService.CurrencyConvertion(); break;
                    case ttServices.TimeDiff: strResponse = ttService.TimeDifference(); break;
                    case ttServices.Cryptic: strResponse = ttService.Cryptic(); break;
                    case ttServices.SalesReport: strResponse = ttService.SalesReport(); break;
                    case ttServices.Native: strResponse = ttService.Native(); break;
                    default: throw new Exception($"{Service} Message is not supported by Sabre.");
                }
                return strResponse;
            }
            catch (Exception ex) { throw; }
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
                    case ttServices.CreateSession: strResponse = ttService.CreateSession(); break;
                    case ttServices.CloseSession: strResponse = ttService.CloseSession(); break;
                    case ttServices.Native: strResponse = ttService.Native(); break;
                    case ttServices.Cryptic: strResponse = ttService.Cryptic(); break;
                    default: throw new Exception($"{Service} Message is not supported by Worldspan.");
                }
                return strResponse;
            }
            catch (Exception ex) { throw; }
        }

        public static string SendOtherRequestTravelport(ttServices Service, ref TravelTalkCredential ttCredential, ref TripXMLProviderSystems ttProviderSystems, ref string strRequest, string Version = "")
        {
            string strResponse = "";
            Travelport.OtherServices ttService;
            try
            {
                ttService = new Travelport.OtherServices();
                ttService.ProviderSystems = ttProviderSystems;
                ttService.Request = strRequest;
                switch (Service)
                {
                    case ttServices.CreateSession: strResponse = ttService.CreateSession(); break;
                    case ttServices.CloseSession: strResponse = ttService.CloseSession(); break;
                    case ttServices.Cryptic: strResponse = ttService.Cryptic(); break;
                    case ttServices.Native: strResponse = ttService.Native(); break;
                    default: throw new Exception($"{Service} Message is not supported by Travelport.");
                }
                return strResponse;
            }
            catch (Exception ex) { throw; }
        }

        public static string SendOtherRequestiTravelInsured(ttServices Service, ref TravelTalkCredential ttCredential, ref TripXMLProviderSystems ttProviderSystems, ref string strRequest, string Version = "")
        {
            string strResponse = "";
            try
            {
                return strResponse;
            }
            catch (Exception ex) { throw; }
        }

        // ---------------------------------------------------------------
        // Send Request to Providers — Cruise Services
        // ---------------------------------------------------------------

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
                    case ttServices.CruiseSailAvail: strResponse = ttService.CruiseSailAvail(); break;
                    case ttServices.CruiseFareAvail: strResponse = ttService.CruiseFareAvail(); break;
                    case ttServices.CruiseCategoryAvail: strResponse = ttService.CruiseCategoryAvail(); break;
                    case ttServices.CruiseCabinAvail: strResponse = ttService.CruiseCabinAvail(); break;
                    case ttServices.CruiseCabinHold: strResponse = ttService.CruiseCabinHold(); break;
                    case ttServices.CruiseCabinUnhold: strResponse = ttService.CruiseCabinUnhold(); break;
                    case ttServices.CruisePriceBooking: strResponse = ttService.CruisePriceBooking(); break;
                    case ttServices.CruiseCreateBooking: strResponse = ttService.CruiseCreateBooking(); break;
                    case ttServices.CruiseRead: strResponse = ttService.CruiseRead(); break;
                    case ttServices.CruiseCancelBooking: strResponse = ttService.CruiseCancelBooking(); break;
                    case ttServices.CruiseModifyBooking: strResponse = ttService.CruiseModifyBooking(); break;
                    case ttServices.CruisePackageAvail: strResponse = ttService.CruisePackageAvail(); break;
                    case ttServices.CruisePackageDesc: strResponse = ttService.CruisePackageDesc(); break;
                    case ttServices.CruiseTransferAvail: strResponse = ttService.CruiseTransferAvail(); break;
                    case ttServices.CruiseItineraryDesc: strResponse = ttService.CruiseItineraryDesc(); break;
                    default: throw new Exception($"{Service} Message is not supported by Amadeus.");
                }
                return strResponse;
            }
            catch (Exception ex) { throw; }
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
                    case ttServices.CruiseSailAvail: strResponse = ttService.CruiseSailAvail(); break;
                    case ttServices.CruiseFareAvail: strResponse = ttService.CruiseFareAvail(); break;
                    case ttServices.CruiseCategoryAvail: strResponse = ttService.CruiseCategoryAvail(); break;
                    case ttServices.CruiseCabinAvail: strResponse = ttService.CruiseCabinAvail(); break;
                    case ttServices.CruiseCabinHold: strResponse = ttService.CruiseCabinHold(); break;
                    case ttServices.CruiseCabinUnhold: strResponse = ttService.CruiseCabinUnhold(); break;
                    case ttServices.CruisePriceBooking: strResponse = ttService.CruisePriceBooking(); break;
                    case ttServices.CruiseCreateBooking: strResponse = ttService.CruiseCreateBooking(); break;
                    case ttServices.CruiseRead: strResponse = ttService.CruiseRead(); break;
                    case ttServices.CruiseCancelBooking: strResponse = ttService.CruiseCancelBooking(); break;
                    case ttServices.CruiseModifyBooking: strResponse = ttService.CruiseModifyBooking(); break;
                    case ttServices.CruisePackageAvail: strResponse = ttService.CruisePackageAvail(); break;
                    case ttServices.CruisePackageDesc: strResponse = ttService.CruisePackageDesc(); break;
                    case ttServices.CruiseTransferAvail: strResponse = ttService.CruiseTransferAvail(); break;
                    default: throw new Exception($"{Service} Message is not supported by Galileo.");
                }
                return strResponse;
            }
            catch (Exception ex) { throw; }
        }

        // ---------------------------------------------------------------
        // Send Request to Providers — Payment Services
        // ---------------------------------------------------------------

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
                    case ttServices.CreateSession: strResponse = ttService.CreateSession(); break;
                    case ttServices.CloseSession: strResponse = ttService.CloseSession(); break;
                    case ttServices.ShowMileage: strResponse = ttService.ShowMileage(); break;
                    case ttServices.CCValid: strResponse = ttService.CreditCardValid(); break;
                    case ttServices.CurConv: strResponse = ttService.CurrencyConvertion(); break;
                    case ttServices.TimeDiff: strResponse = ttService.TimeDifference(); break;
                    case ttServices.Cryptic: strResponse = ttService.Cryptic(); break;
                    case ttServices.Native: strResponse = ttService.Native(); break;
                    case ttServices.ETicketVerify: strResponse = ttService.ETicketVerify(); break;
                    case ttServices.MultiMessage: strResponse = ttService.MultiMessage(); break;
                    case ttServices.ProfileRead: strResponse = ttService.ProfileRead(); break;
                    case ttServices.ProfileCreate: strResponse = ttService.ProfileCreate(); break;
                    default: throw new Exception($"{Service} Message is not supported by Galileo.");
                }
                return strResponse;
            }
            catch (Exception ex) { throw; }
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
                    case ttServices.GenerateVirtualCard: strResponse = ttService.CreateVirtualCard(); break;
                    case ttServices.DeleteVirtualCard: strResponse = ttService.DeleteVirtualCard(); break;
                    case ttServices.GetVirtualCardDetails: strResponse = ttService.GetVirtualCardDetails(); break;
                    case ttServices.ListVirtualCards: strResponse = ttService.ListVirtualCards(); break;
                    default: throw new Exception($"{Service} Message is not supported by Amadeus.");
                }
                return strResponse;
            }
            catch (Exception ex) { throw; }
        }

        public static T SendPaymentRequest<T>(ttServices Service, ref TravelTalkCredential ttCredential, ref TripXMLProviderSystems ttProviderSystems, ref VirtualCardRQBase request)
        {
            T responseObj = default;
            try
            {
                var paymentServices = new VirtualCardPaymentService();
                paymentServices.Provider = ttProviderSystems.Provider;
                paymentServices.UUID = ttProviderSystems.LogUUID;
                paymentServices.Request = request;
                switch (Service)
                {
                    case ttServices.GenerateVirtualCard: responseObj = (T)(object)paymentServices.CreateVirtualCard(); break;
                    case ttServices.DeleteVirtualCard: responseObj = (T)(object)paymentServices.DeleteVirtualCard(); break;
                    case ttServices.GetVirtualCardDetails: responseObj = (T)(object)paymentServices.GetVirtualCardDetails(); break;
                    case ttServices.ListVirtualCards: responseObj = (T)(object)paymentServices.ListVirtualCards(); break;
                    default: throw new Exception($"{Service} Message is not supported by Amadeus.");
                }
                return responseObj;
            }
            catch (Exception ex) { throw; }
        }

        public static string SendPaymentRequestSabre(ttServices Service, ref TravelTalkCredential ttCredential, ref TripXMLProviderSystems ttProviderSystems, ref string strRequest, string Version = "")
        {
            string strResponse = "";
            try { return strResponse; }
            catch (Exception ex) { throw; }
        }

        public static string SendPaymentRequestWorldspan(ttServices Service, ref TravelTalkCredential ttCredential, ref TripXMLProviderSystems ttProviderSystems, ref string strRequest, string Version = "")
        {
            string strResponse = "";
            Worldspan.OtherServices ttService;
            try
            {
                ttService = new Worldspan.OtherServices();
                return strResponse;
            }
            catch (Exception ex) { throw; }
        }

        public static string SendPaymentRequestTravelport(ttServices Service, ref TravelTalkCredential ttCredential, ref TripXMLProviderSystems ttProviderSystems, ref string strRequest, string Version = "")
        {
            string strResponse = "";
            Travelport.OtherServices ttService;
            try
            {
                ttService = new Travelport.OtherServices();
                return strResponse;
            }
            catch (Exception ex) { throw; }
        }

        public static string SendPaymentRequestiTravelInsured(ttServices Service, ref TravelTalkCredential ttCredential, ref TripXMLProviderSystems ttProviderSystems, ref string strRequest, string Version = "")
        {
            string strResponse = "";
            try { return strResponse; }
            catch (Exception ex) { throw; }
        }

        // ---------------------------------------------------------------
        // CreateUUID
        // ---------------------------------------------------------------

        // ✅ Original had an infinite loop bug: after .Replace("-","") there are no dashes left,
        //    so LastIndexOf("-") == -1, which is always < 20 → infinite loop.
        //    Fixed: Guid("N") directly produces 32 uppercase hex chars with no dashes.
        public static string CreateUUID() => Guid.NewGuid().ToString("N").ToUpper();

        // ---------------------------------------------------------------
        // Log to File
        // ---------------------------------------------------------------

        public static void LogMessageToFile(
            int LogType, ref string UUID, ref string WebServer,
            ref string Customer, ref string UserName, ref string Provider,
            int MessageID, ref string Message, DateTime MessageDate,
            int ResponseTime, string ExError)
        {
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
                sb.Append("<Message>")
                    // ✅ "\r" → "\r"
                    .Append(Message
                        .Replace("\r", "")
                        .Replace("\n", "")
                        .Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", ""))
                    .Append("</Message>");
                sb.Append("<MessageDate>").Append(MessageDate).Append("</MessageDate>");
                sb.Append("<ResponseTime>").Append(ResponseTime).Append("</ResponseTime>");
                sb.Append("<ExError>").Append(ExError).Append("</ExError>");
                sb.Append("</Line>");

                // ✅ FileSystem.FreeFile/FileOpen/PrintLine/FileClose → File.AppendAllText
                var logFilePath = Path.Combine(LogPath, $"{DateTime.Now:d}_Log.txt");
                File.AppendAllText(logFilePath, sb.ToString() + Environment.NewLine);
            }
            catch (Exception ex)
            {
                // Ignore
            }
        }

        public static void LogSoapExceptionToFile(
            ref string SoapException, ref string SoapEnvelope, string ExError)
        {
            var sb = new StringBuilder();
            try
            {
                sb.Append("<Line>");
                sb.Append("<LogType>").Append(1).Append("</LogType>");
                sb.Append("<UUID></UUID><WebServer></WebServer><Customer></Customer>");
                sb.Append("<UserName></UserName><Provider></Provider>");
                sb.Append("<MessageID>SoapException</MessageID>");
                sb.Append("<Message>").Append(SoapEnvelope).Append("</Message>");
                sb.Append("<MessageDate>").Append(DateTime.Now).Append("</MessageDate>");
                sb.Append("<ResponseTime></ResponseTime>");
                sb.Append("<ExError>").Append(SoapException).Append(" ").Append(ExError).Append("</ExError>");
                sb.Append("</Line>");

                var logFilePath = Path.Combine(LogPath, "SoapException.txt");
                File.AppendAllText(logFilePath, sb.ToString() + Environment.NewLine);
            }
            catch (Exception ex)
            {
                // Ignore
            }
        }

        // ---------------------------------------------------------------
        // Load Variables and Tables in Memory / Session Pool
        // ---------------------------------------------------------------

        // ✅ ref HttpApplicationState oApplication removed — was not used in body
        public static void CreatInitialSessionPool(
            ref TripXMLProviderSystems ttProviderSystems, string Provider)
        {
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
                    if (Provider.ToLower() == "amadeusws")
                    {
                        // ✅ Collapsed redundant if/else if/else
                        ttProviderSystems.URL = ttProviderSystems.System == "Test"
                            ? "https://test.webservices.amadeus.com"
                            : "https://production.webservices.amadeus.com";

                        ttAA = new AmadeusWSAdapter(ttProviderSystems, "V1");
                        ttAA.isSOAP2 = ttProviderSystems.SOAP2;
                        ttAA.isSOAP4 = ttProviderSystems.SOAP4;
                        ttAA.GetStoredFares = ttProviderSystems.GetStoredFares;
                        for (int i = 0; i < ttAA.InitialBlock; i++)
                        {
                            ttAA.CreateSessionV2();
                            sessionCount++;
                        }
                    }
                    else if (Provider.ToLower() == "sabre")
                    {
                        ttSA = new Sabre.SabreAdapter(ttProviderSystems, "V1");
                        for (int i = 0; i < ttSA.InitialBlockSize; i++)
                        {
                            iBlockThread = new Thread(new ThreadStart(ttSA.CreateSessionV2));
                            iBlockThread.Start();
                            sessionCount++;
                        }
                    }
                    else if (Provider.ToLower() == "galileo" || Provider.ToLower() == "apollo")
                    {
                        ttGA = new Galileo.GalileoAdapter(ttProviderSystems, "V1");
                        for (int i = 0; i < ttGA.InitialBlockSize; i++)
                        {
                            iBlockThread = new Thread(new ThreadStart(ttGA.CreateSessionV2));
                            iBlockThread.Start();
                            sessionCount++;
                        }
                    }
                    oDA.UpdatePCCSessions(ttProviderSystems.PCC, sessionCount, ttProviderSystems.UserID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        // ✅ ref HttpApplicationState oApplication removed — was not used in body
        public static void TripXMLStartUp()
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                var start = DateTime.Now;
                TripXMLTools.TripXMLLoad.TripXMLLoadObject();
                TripXMLTools.TripXMLLoad.GetDecodingTables();
                var loadTime = DateTime.Now - start;
                CoreLib.SendTrace("", "TripXMLLoad",
                    $"TripXML was loaded in {loadTime.TotalSeconds:0.##} seconds", "", "");
                modCore.Trace = true; // TODO: Remove once TravelTalkUserSettings is working
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Starting up Application.{ex.Message}");
            }
            // ✅ GC.Collect() removed
        }
    }
    public class TripXML : System.Web.Services.Protocols.SoapHeader
    {

        public string userName;
        public string password;
        public bool compressed;
    }

}