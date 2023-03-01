using System;
using System.ComponentModel;
using System.Text;
using System.Web.Services;
using Microsoft.VisualBasic.CompilerServices;
using TripXMLMain;
using static TripXMLMain.modCore;

namespace wsTripXML.wsTravelTalk
{

    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
    // <System.Web.Script.Services.ScriptService()> _
    [WebService(Namespace = "http://tripxml.com/wsAdmin")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    public class wsAdmin : WebService
    {
        private StringBuilder sb = new StringBuilder();

        private string ServiceRequest(string strRequest, int ttServiceID)
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

                var argoApp = Application;
                wsTravelTalk.modMain.PreServiceRequest(ref strRequest, ref argoApp, ref ttCredential, ref ttProviderSystems, StartTime, ttServiceID, Server.MachineName, ref UUID);
                ValidateXSDOut = Conversions.ToBoolean(Application.Get(sb.Append("XSD").Append(ttCredential.UserID).Append("Out").ToString()));
                sb.Remove(0, sb.Length);

                switch (ttCredential.Providers[0].Name ?? "")
                {
                    case "Amadeus":
                        {
                            break;
                        }
                    case "AmadeusWS":
                        {
                            if (ttServiceID == (int)ttServices.AddPNRToAdmin)
                            {
                                strResponse = wsTravelTalk.modMain.SendTravelRequestAmadeusWS((ttServices)ttServiceID, ref ttCredential, ref ttProviderSystems, ref strRequest);
                            }
                            else
                            {
                                strResponse = wsTravelTalk.modMain.SendPNRRequestAmadeusWS((ttServices)ttServiceID, ref ttCredential, ref ttProviderSystems, ref strRequest);
                            }

                            break;
                        }
                    case "Apollo":
                    case "Galileo":
                        {
                            strResponse = wsTravelTalk.modMain.SendTravelRequestGalileo((ttServices)ttServiceID, ref ttCredential, ref ttProviderSystems, ref strRequest);
                            break;
                        }
                    case "Sabre":
                        {

                            if (ttProviderSystems.System is null)
                            {
                                FormatErrorMessage((ttServices)ttServiceID, sb.Append("Access denied to ").Append(ttCredential.Providers[0].Name).Append(" - ").Append(ttCredential.System).Append(" system. Or invalid provider.").ToString(), ttCredential.Providers[0].Name);
                                sb.Remove(0, sb.Length);
                                break;
                            }

                            ttProviderSystems.AAAPCC = ttCredential.Providers[0].PCC;
                            strResponse = wsTravelTalk.modMain.SendTravelRequestSabre((ttServices)ttServiceID, ref ttCredential, ref ttProviderSystems, ref strRequest);
                            break;
                        }
                    case "Worldspan":
                        {
                            strResponse = wsTravelTalk.modMain.SendTravelRequestWorldspan((ttServices)ttServiceID, ref ttCredential, ref ttProviderSystems, ref strRequest);
                            break;
                        }
                    case "Travelport":
                        {
                            strResponse = wsTravelTalk.modMain.SendTravelRequestTravelport((ttServices)ttServiceID, ref ttCredential, ref ttProviderSystems, ref strRequest);
                            break;
                        }

                    default:
                        {
                            throw new Exception(sb.Append("Provider ").Append(ttCredential.Providers[0].Name).Append(" Not Currently Supported.").ToString());
                            sb.Remove(0, sb.Length);
                            break;
                        }
                }

                wsTravelTalk.modMain.PostServiceRequest(ref strResponse, ValidateXSDOut, ttServiceID, ttCredential.UserID);
            }

            catch (Exception ex)
            {
                strResponse = FormatErrorMessage((ttServices)ttServiceID, ex.Message, ttCredential.Providers[0].Name);
            }
            finally
            {
                if (modCore.Trace)
                    CoreLib.SendTrace(ttCredential.UserID, "wsAddPNRToAdmin", "============= Response ============= ", strResponse, ttProviderSystems.LogUUID);
            }

            return strResponse;
            sb = null;
        }

        [WebMethod(Description = "Add a PNR to the Admin by TravelBuild response XML.")]
        public string AddPNRToAdmin(string xmlRequest)
        {
            return ServiceRequest(xmlRequest, (int)ttServices.AddPNRToAdmin);
        }

        [WebMethod(Description = "Add a PNR to the Admin by record locator.")]
        public string AddRecLocToAdmin(string xmlRequest)
        {
            return ServiceRequest(xmlRequest, (int)ttServices.AddRecLocToAdmin);
        }

        [WebMethod(Description = "Add a PNR to the Admin by record locator.")]
        public string AddRecLocToNewAdminOnly(string xmlRequest)
        {
            return ServiceRequest(xmlRequest, (int)ttServices.AddRecLocToNewAdminOnly);
        }

        [WebMethod(Description = "Update Markups.")]
        public string UpdateMarkups(string xmlRequest)
        {
            var markUp = new wsTravelTalk.wsUpdateMarkups();
            return markUp.UpdateMarkups(xmlRequest);
        }

        [WebMethod(Description = "Admin status management.")]
        public string CreateTicketInvoice(string xmlRequest)
        {
            var tktInvoice = new wsTravelTalk.wsCreateTicketInvoice();
            return tktInvoice.CreateTicketInvoice(xmlRequest);
        }

    }
}