using System;
using System.ComponentModel;
using System.Text;
using Microsoft.VisualBasic.CompilerServices;
using TripXMLMain;
using static TripXMLMain.modCore;
using TripXMLTools;

namespace wsTripXML.wsTravelTalk
{

    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
    // <System.Web.Script.Services.ScriptService()> _
    [ToolboxItem(false)]
    public partial class wsAdmin
    {
        private readonly modMain _modMain;
        private readonly Microsoft.AspNetCore.Http.IHttpContextAccessor _http;

        public wsAdmin(modMain modMain, Microsoft.AspNetCore.Http.IHttpContextAccessor http)
        {
            _modMain = modMain;
            _http = http;
        }

        private StringBuilder sb = new StringBuilder();

        private string ServiceRequest(string strRequest, ttServices ttServiceID)
        {
            string strResponse = "";
            TravelTalkCredential ttCredential = default;
            TripXMLProviderSystems ttProviderSystems = default;
            bool ValidateXSDOut;
            DateTime StartTime;
            string UUID = "";

            try
            {
                StartTime = DateTime.Now;
                _modMain.PreServiceRequest(ref strRequest, ref ttCredential, ref ttProviderSystems, StartTime, (int)ttServiceID, Environment.MachineName, ref UUID);
                ValidateXSDOut = Conversions.ToBoolean(TripXMLMain.AppState.Get(sb.Append("XSD").Append(ttCredential.UserID).Append("Out").ToString()));
                sb.Remove(0, sb.Length);

                switch (ttCredential.Providers[0].Name ?? "")
                {
                    case "Amadeus":
                        {
                            break;
                        }
                    case "AmadeusWS":
                        {
                            if (ttServiceID == ttServices.AddPNRToAdmin)
                            {
                                strResponse = modMain.SendTravelRequestAmadeusWS(ttServiceID, ref ttCredential, ref ttProviderSystems, ref strRequest);
                            }
                            else
                            {
                                strResponse = modMain.SendPNRRequestAmadeusWS(ttServiceID, ref ttCredential, ref ttProviderSystems, ref strRequest);
                            }

                            break;
                        }
                    case "Apollo":
                    case "Galileo":
                        {
                            strResponse = modMain.SendTravelRequestGalileo(ttServiceID, ref ttCredential, ref ttProviderSystems, ref strRequest);
                            break;
                        }
                    case "Sabre":
                        {

                            if (ttProviderSystems.System is null)
                            {
                                FormatErrorMessage(ttServiceID, sb.Append("Access denied to ").Append(ttCredential.Providers[0].Name).Append(" - ").Append(ttCredential.System).Append(" system. Or invalid provider.").ToString(), ttCredential.Providers[0].Name);
                                sb.Remove(0, sb.Length);
                                break;
                            }

                            ttProviderSystems.AAAPCC = ttCredential.Providers[0].PCC;
                            strResponse = modMain.SendTravelRequestSabre(ttServiceID, ref ttCredential, ref ttProviderSystems, ref strRequest);
                            break;
                        }
                    case "Worldspan":
                        {
                            strResponse = modMain.SendTravelRequestWorldspan(ttServiceID, ref ttCredential, ref ttProviderSystems, ref strRequest);
                            break;
                        }
                    case "Travelport":
                        {
                            strResponse = modMain.SendTravelRequestTravelport(ttServiceID, ref ttCredential, ref ttProviderSystems, ref strRequest);
                            break;
                        }

                    default:
                        {
                            throw new Exception(sb.Append("Provider ").Append(ttCredential.Providers[0].Name).Append(" Not Currently Supported.").ToString());
                            sb.Remove(0, sb.Length);
                            break;
                        }
                }

                modMain.PostServiceRequest(ref strResponse, ValidateXSDOut, (int)ttServiceID, ttCredential.UserID);
            }

            catch (Exception ex)
            {
                strResponse = FormatErrorMessage(ttServiceID, ex.Message, ttCredential.Providers[0].Name);
            }
            finally
            {
                if (modCore.Trace)
                    CoreLib.SendTrace(ttCredential.UserID, "wsAddPNRToAdmin", "============= Response ============= ", strResponse, ttProviderSystems.LogUUID);
            }

            return strResponse;
            sb = null;
        }
        public string AddPNRToAdmin(string xmlRequest)
        {
            return ServiceRequest(xmlRequest, ttServices.AddPNRToAdmin);
        }
        public string AddRecLocToAdmin(string xmlRequest)
        {
            return ServiceRequest(xmlRequest, ttServices.AddRecLocToAdmin);
        }
        public string AddRecLocToNewAdminOnly(string xmlRequest)
        {
            return ServiceRequest(xmlRequest, ttServices.AddRecLocToNewAdminOnly);
        }
        public string UpdateMarkups(string xmlRequest)
        {
            var markUp = new wsUpdateMarkups();
            return markUp.UpdateMarkups(xmlRequest);
        }
        public string CreateTicketInvoice(string xmlRequest)
        {
            var tktInvoice = new wsCreateTicketInvoice();
            return tktInvoice.CreateTicketInvoice(xmlRequest);
        }
        public TripXmlSettings GetServerConfig()
        {
            // ASMX Context.Request.Headers was a NameValueCollection; adapt from ASP.NET Core
            var headers = new System.Collections.Specialized.NameValueCollection();
            var request = _http.HttpContext?.Request;
            if (request is not null)
            {
                foreach (var h in request.Headers)
                {
                    headers[h.Key] = h.Value.ToString();
                }
            }
            return SettingsService.GetAppSettings(headers);
        }
        public TripXmlVersion GetServerVersion()
        {
            return SettingsService.GetAppVersion();
        }
        public UpdateCacheResponse UpdateCache()
        {
            return TripXMLLoad.UpdateCachedObjects().Result;
        }

    }
}