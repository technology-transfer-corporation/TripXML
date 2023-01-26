using System;
using System.Diagnostics;
using System.Text;
using System.Web.Services;
using System.Xml.Serialization;
using Microsoft.VisualBasic.CompilerServices;
using TripXMLMain;
using static TripXMLMain.modCore;

namespace wsTripXML.wsTravelTalk
{

    [System.Web.Services.Protocols.SoapDocumentService(RoutingStyle = System.Web.Services.Protocols.SoapServiceRoutingStyle.RequestElement)]
    [WebService(Namespace = "http://tripxml.downtowntravel.com/tripxml/wsIssueTicketSessioned", Name = "wsIssueTicketSessioned", Description = "A TripXML Web Service to Process Issue ticket with open session")]
    public class wsIssueTicketSessioned : WebService
    {
        public wsTravelTalk.TripXML tXML;

        #region  Web Services Designer Generated Code 

        public wsIssueTicketSessioned() : base()
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

        #region  Decode Functions 

        #endregion

        #region  Process Service Request All GDS 
        private StringBuilder sb = new StringBuilder();

        private string ServiceRequest(string strRequest, int ttServiceID)
        {
            string strResponse = "";
            TravelTalkCredential ttCredential = default;
            TripXMLProviderSystems ttProviderSystems = default;
            bool validateXSDOut;
            var startTime = default(DateTime);
            string uuid = "";

            try
            {
                startTime = DateTime.Now;

                var argoApp = Application;
                wsTravelTalk.modMain.PreServiceRequest(ref strRequest, ref argoApp, ref ttCredential, ref ttProviderSystems, startTime, ttServiceID, Server.MachineName, ref uuid);
                validateXSDOut = Conversions.ToBoolean(Application.Get(sb.Append("XSD").Append(ttCredential.UserID).Append("Out").ToString()));
                sb.Remove(0, sb.Length);

                switch (ttCredential.Providers[0].Name.ToLower() ?? "")
                {
                    case "amadeus":
                        {
                            strResponse = wsTravelTalk.modMain.SendTravelRequestAmadeusWS((ttServices)ttServiceID, ref ttCredential, ref ttProviderSystems, ref strRequest);
                            break;
                        }

                    case "amadeusws":
                        {
                            strResponse = wsTravelTalk.modMain.SendTravelRequestAmadeusWS((ttServices)ttServiceID, ref ttCredential, ref ttProviderSystems, ref strRequest);
                            break;
                        }

                    case "sabre":
                        {
                            // ttProviderSystems = Application.Get(sb.Append("PS").Append(ttCredential.Providers(0).Name).Append(ttCredential.UserID).Append(ttCredential.System).Append(ttCredential.Providers(0).PCC).ToString())
                            // sb.Remove(0, sb.Length())
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

                    case "worldspan":
                        {
                            strResponse = wsTravelTalk.modMain.SendTravelRequestWorldspan((ttServices)ttServiceID, ref ttCredential, ref ttProviderSystems, ref strRequest);
                            break;
                        }

                    case "apollo":
                    case "galileo":
                        {

                            strResponse = wsTravelTalk.modMain.SendTravelRequestGalileo((ttServices)ttServiceID, ref ttCredential, ref ttProviderSystems, ref strRequest);
                            break;
                        }

                    default:
                        {
                            throw new Exception(sb.Append("Provider ").Append(ttCredential.Providers[0].Name).Append(" Not Currently Supported.").ToString());
                        }
                }

                wsTravelTalk.modMain.PostServiceRequest(ref strResponse, validateXSDOut, ttServiceID, ttCredential.UserID);
            }

            catch (Exception ex)
            {
                strResponse = FormatErrorMessage((ttServices)ttServiceID, ex.Message, ttCredential.Providers[0].Name);
            }
            finally
            {
                wsTravelTalk.modMain.LogResponse(ref strResponse, ref ttCredential, startTime, ttServiceID, Server.MachineName, ref uuid);
                if (modCore.Trace)
                    CoreLib.SendTrace(ttCredential.UserID, "wsIssueTicket", "============= OTA Response ============= ", strResponse, uuid);
            }

            return strResponse;

        }

        #endregion

        #region  Web Methods 

        [CompressionExtension.CompressionExtension()]
        [WebMethod(Description = "Process Issue Ticket Messages Request.")]
        [System.Web.Services.Protocols.SoapHeader("tXML")]
        public wsTravelTalk.wmIssueTicketSessionedOut.TT_IssueTicketRS wmIssueTicketSessioned(wsTravelTalk.wmIssueTicketSessionedIn.TT_IssueTicketRQ TT_IssueTicketRQ)
        {
            wsTravelTalk.wmIssueTicketSessionedOut.TT_IssueTicketRS oIssueTicketRS = null;
            XmlSerializer oSerializer;
            System.IO.StringWriter oWriter;
            System.IO.StringReader oReader;

            oSerializer = new XmlSerializer(typeof(wsTravelTalk.wmIssueTicketSessionedIn.TT_IssueTicketRQ));
            oWriter = new System.IO.StringWriter(new StringBuilder());
            oSerializer.Serialize(oWriter, TT_IssueTicketRQ);
            string xmlMessage = oWriter.ToString();
            xmlMessage = xmlMessage.Replace(" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"", "");
            xmlMessage = xmlMessage.Replace(" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", "");

            xmlMessage = ServiceRequest(xmlMessage, (int)ttServices.IssueTicketSessioned);

            try
            {
                oSerializer = null;
                oSerializer = new XmlSerializer(type: typeof(wsTravelTalk.wmIssueTicketSessionedOut.TT_IssueTicketRS));
                oReader = new System.IO.StringReader(xmlMessage);
                oIssueTicketRS = (wsTravelTalk.wmIssueTicketSessionedOut.TT_IssueTicketRS)oSerializer.Deserialize(oReader);
            }
            catch (Exception ex)
            {
                CoreLib.SendTrace("", "wsIssueTicketSessioned", "Error Deserialing OTA Response", ex.Message, string.Empty);
            }

            return oIssueTicketRS;

        }

        [WebMethod(Description = "Process Issue Ticket Xml Messages Request.")]
        public string wmIssueTicketSessionedXml(string xmlRequest)
        {
            return ServiceRequest(xmlRequest, (int)ttServices.IssueTicketSessioned);
        }

        #endregion

    }

}