using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Web.Configuration;
using System.Web.Services;
using System.Xml.Serialization;
using Microsoft.VisualBasic.CompilerServices;
using TripXMLMain;
using static TripXMLMain.modCore;

namespace wsTripXML.wsTravelTalk
{

    [System.Web.Services.Protocols.SoapDocumentService(RoutingStyle = System.Web.Services.Protocols.SoapServiceRoutingStyle.RequestElement)]
    [WebService(Namespace = "http://tripxml.downtowntravel.com/tripxml/wsPNRReprice", Name = "wsPNRReprice", Description = "A TripXML Web Service to Process PNR Reprice Request.")]
    public class wsPNRReprice : WebService
    {
        public TripXML TXML;

        #region  Web Services Designer Generated Code 

        public wsPNRReprice() : base()
        {

            // This call is required by the Web Services Designer.
            InitializeComponent();

            // Add your own initialization code after the InitializeComponent() call

        }

        // Required by the Web Services Designer
        private System.ComponentModel.IContainer components;

        // NOTE: The following procedure is required by the Web Services Designer
        // It can be modified using the Web Services Designer.  
        // Do not modify it using the code editor.
        [DebuggerStepThrough()]
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
        }

        protected override void Dispose(bool disposing)
        {
            // CODEGEN: This procedure is required by the Web Services Designer
            // Do not modify it using the code editor.
            if (disposing)
            {
                if (components is not null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #endregion

        #region  Process Service Request All GDS 
        private readonly StringBuilder sb = new StringBuilder();

        private string StoredFareServiceRequest(string request, ttServices ttServiceID, string sessionID = "")
        {
            string response = "";
            TravelTalkCredential ttCredential = default;
            TripXMLProviderSystems ttProviderSystems = default;
            bool validateXSDOut;
            var startTime = default(DateTime);
            string UUID = "";

            try
            {
                startTime = DateTime.Now;

                var argoApp = Application;
                modMain.PreServiceRequest(ref request, ref argoApp, ref ttCredential, ref ttProviderSystems, startTime, (int)ttServiceID, Server.MachineName, ref UUID);
                validateXSDOut = Conversions.ToBoolean(Application.Get(sb.Append("XSD").Append(ttCredential.UserID).Append("Out").ToString()));
                sb.Remove(0, sb.Length);

                switch (ttCredential.Providers[0].Name ?? "")
                {
                    case "AmadeusWS":
                        {
                            response = modMain.SendPNRRequestAmadeusWS(ttServiceID, ref ttCredential, ref ttProviderSystems, ref request);
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
                            response = modMain.SendPNRRequestSabre(ttServiceID, ref ttCredential, ref ttProviderSystems, ref request);
                            break;
                        }
                    case "Worldspan":
                        {
                            if (Conversions.ToBoolean(WebConfigurationManager.AppSettings["IsTravelportWorldspan"]))
                            {
                                var ttDefProvider = new TripXMLProviderSystems();
                                if (!string.IsNullOrEmpty(sessionID))
                                {
                                    request = request.Replace(sessionID, "");
                                }
                                var argoApp1 = Application;
                                modMain.PreServiceRequest(ref request, ref argoApp1, ref ttCredential, ref ttDefProvider, startTime, (int)ttServiceID, Server.MachineName, ref UUID, "", true);
                                response = modMain.SendPNRRequestTravelPort(ttServiceID, ref ttCredential, ref ttDefProvider, ref request);
                                response = response.Replace("</OTA_PNRRepriceRS>", $"<ConversationID>{sessionID}</ConversationID></OTA_PNRRepriceRS>");
                            }
                            else
                            {
                                response = modMain.SendPNRRequestWorldspan(ttServiceID, ref ttCredential, ref ttProviderSystems, ref request);
                            }

                            break;
                        }
                    case "Galileo":
                        {
                            if (Conversions.ToBoolean(WebConfigurationManager.AppSettings["IsTravelportReprice"]))
                            {
                                var ttDefProvider = new TripXMLProviderSystems();
                                if (!string.IsNullOrEmpty(sessionID))
                                {
                                    request = request.Replace(sessionID, "");
                                }

                                string aaapcc = ttDefProvider.AAAPCC;
                                string _pcc = ttCredential.Providers[0].PCC;

                                ttDefProvider.AAAPCC = ttCredential.Providers[0].PCC;
                                ttCredential.Providers[0].PCC = "3M2Y";
                                var argoApp2 = Application;
                                modMain.PreServiceRequest(ref request, ref argoApp2, ref ttCredential, ref ttDefProvider, startTime, (int)ttServiceID, Server.MachineName, ref UUID, "", true);
                                response = modMain.SendPNRRequestTravelPort(ttServiceID, ref ttCredential, ref ttDefProvider, ref request);
                                response = response.Replace("</OTA_PNRRepriceRS>", $"<ConversationID>{sessionID}</ConversationID></OTA_PNRRepriceRS>");

                                // ttDefProvider.AAAPCC = aaapcc
                                ttCredential.Providers[0].PCC = _pcc;
                            }
                            else
                            {
                                response = modMain.SendPNRRequestGalileo(ttServiceID, ref ttCredential, ref ttProviderSystems, ref request);
                            }

                            break;
                        }
                    case "Travelport":
                        {
                            response = modMain.SendPNRRequestTravelPort(ttServiceID, ref ttCredential, ref ttProviderSystems, ref request);
                            break;
                        }

                    default:
                        {
                            throw new Exception(sb.Append("Provider ").Append(ttCredential.Providers[0].Name).Append(" Not Currently Supported.").ToString());
                        }
                }

                modMain.PostServiceRequest(ref response, validateXSDOut, (int)ttServiceID, ttCredential.UserID);
            }

            catch (Exception ex)
            {
                response = FormatErrorMessage(ttServiceID, ex.Message, ttCredential.Providers[0].Name);
            }
            finally
            {
                modMain.LogResponse(ref response, ref ttCredential, startTime, (int)ttServiceID, Server.MachineName, ref UUID);
                if (modCore.Trace)
                    CoreLib.SendTrace(ttCredential.UserID, "wsPNRReprice", "============= OTA Response ============= ", response, UUID);
            }

            return response;
        }

        private string ServiceRequest(string request, ttServices ttServiceID)
        {
            string response = "";
            TravelTalkCredential ttCredential = default;
            TripXMLProviderSystems ttProviderSystems = default;
            bool validateXSDOut;
            var startTime = default(DateTime);
            string UUID = "";

            try
            {
                startTime = DateTime.Now;

                var argoApp = Application;
                modMain.PreServiceRequest(ref request, ref argoApp, ref ttCredential, ref ttProviderSystems, startTime, (int)ttServiceID, Server.MachineName, ref UUID);
                validateXSDOut = Conversions.ToBoolean(Application.Get(sb.Append("XSD").Append(ttCredential.UserID).Append("Out").ToString()));
                sb.Remove(0, sb.Length);

                switch (ttCredential.Providers[0].Name ?? "")
                {
                    case "AmadeusWS":
                        {
                            response = modMain.SendPNRRequestAmadeusWS(ttServiceID, ref ttCredential, ref ttProviderSystems, ref request);
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
                            response = modMain.SendPNRRequestSabre(ttServiceID, ref ttCredential, ref ttProviderSystems, ref request);
                            break;
                        }
                    case "Worldspan":
                        {
                            response = modMain.SendPNRRequestWorldspan(ttServiceID, ref ttCredential, ref ttProviderSystems, ref request);
                            break;
                        }
                    case "Galileo":
                        {
                            response = modMain.SendPNRRequestGalileo(ttServiceID, ref ttCredential, ref ttProviderSystems, ref request);
                            break;
                        }
                    case "Travelport":
                        {
                            response = modMain.SendPNRRequestTravelPort(ttServiceID, ref ttCredential, ref ttProviderSystems, ref request);
                            break;
                        }

                    default:
                        {
                            throw new Exception(sb.Append("Provider ").Append(ttCredential.Providers[0].Name).Append(" Not Currently Supported.").ToString());
                        }
                }

                modMain.PostServiceRequest(ref response, validateXSDOut, (int)ttServiceID, ttCredential.UserID);
            }

            catch (Exception ex)
            {
                response = FormatErrorMessage(ttServiceID, ex.Message, ttCredential.Providers[0].Name);
            }
            finally
            {
                modMain.LogResponse(ref response, ref ttCredential, startTime, (int)ttServiceID, Server.MachineName, ref UUID);
                if (modCore.Trace)
                    CoreLib.SendTrace(ttCredential.UserID, "wsPNRReprice", "============= OTA Response ============= ", response, UUID);
            }

            return response;
        }

        #endregion

        #region  Web Methods 

        [CompressionExtension.CompressionExtension()]
        [WebMethod(Description = "Process PNR Reprice Messages Request.")]
        [System.Web.Services.Protocols.SoapHeader("TXML")]
        public wmPNRRepriceOut.OTA_PNRRepriceRS wmPNRReprice(wmPNRRepriceIn.OTA_PNRRepriceRQ OTA_PNRRepriceRQ)
        {
            wmPNRRepriceOut.OTA_PNRRepriceRS oPNRRepriceRS = null;

            var oSerializer = new XmlSerializer(typeof(wmPNRRepriceIn.OTA_PNRRepriceRQ));
            var oWriter = new StringWriter(new StringBuilder());
            oSerializer.Serialize(oWriter, OTA_PNRRepriceRQ);
            string xmlMessage = oWriter.ToString();
            xmlMessage = xmlMessage.Replace(" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", "");
            string sid = OTA_PNRRepriceRQ.ConversationID;

            // If OTA_PNRRepriceRQ.StoreFare Then
            xmlMessage = StoredFareServiceRequest(xmlMessage, ttServices.PNRReprice, sid);
            // Else
            // xmlMessage = ServiceRequest(xmlMessage, ttServices.PNRReprice)
            // End If

            try
            {
                oSerializer = new XmlSerializer(@type: typeof(wmPNRRepriceOut.OTA_PNRRepriceRS));
                var oReader = new StringReader(xmlMessage);
                oPNRRepriceRS = (wmPNRRepriceOut.OTA_PNRRepriceRS)oSerializer.Deserialize(oReader);
            }
            catch (Exception ex)
            {
                CoreLib.SendTrace("", "wsPNRReprice", "Error Deserialing OTA Response", ex.Message, string.Empty);
            }

            return oPNRRepriceRS;

        }

        [WebMethod(Description = "Process PNR Reprice Xml Messages Request.")]
        public string wmPNRRepriceXml(string xmlRequest)
        {
            return ServiceRequest(xmlRequest, ttServices.PNRReprice);
        }

        #endregion

    }

}