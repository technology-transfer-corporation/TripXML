using System;
using System.Diagnostics;
using System.Text;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Serialization;
using CompressionExtension;
using Microsoft.VisualBasic.CompilerServices;
using TripXMLMain;
using static TripXMLMain.modCore;

namespace wsTripXML.wsTravelTalk
{

    [SoapDocumentService(RoutingStyle = SoapServiceRoutingStyle.RequestElement)]
    [WebService(Namespace = "http://tripxml.downtowntravel.com/tripxml/wsIssueTicket", Name = "wsIssueTicket", Description = "A TripXML Web Service to Process Issue Ticket Messages Request.")]
    public class wsIssueTicket : WebService
    {
        public wsTravelTalk.TripXML tXML;

        #region  Web Services Designer Generated Code 

        public wsIssueTicket() : base()
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
            bool ValidateXSDOut;
            var StartTime = default(DateTime);
            string UUID = "";

            try
            {
                StartTime = DateTime.Now;

                var argoApp = Application;
                wsTravelTalk.modMain.PreServiceRequest(ref strRequest, ref argoApp, ref ttCredential, ref ttProviderSystems, StartTime, ttServiceID, Server.MachineName, ref UUID);
                ValidateXSDOut = Conversions.ToBoolean(Application.Get(sb.Append("XSD").Append(ttCredential.UserID).Append("Out").ToString()));

                sb.Remove(0, sb.Length);

                switch (ttCredential.Providers[0].Name.ToLower() ?? "")
                {
                    case "amadeusws":
                        {
                            strResponse = wsTravelTalk.modMain.SendTravelRequestAmadeusWS((ttServices)ttServiceID, ref ttCredential, ref ttProviderSystems, ref strRequest);
                            break;
                        }
                    case "apollo":
                    case "galileo":
                        {
                            strResponse = wsTravelTalk.modMain.SendTravelRequestGalileo((ttServices)ttServiceID, ref ttCredential, ref ttProviderSystems, ref strRequest);
                            break;
                        }
                    case "sabre":
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

                    default:
                        {
                            throw new Exception(sb.Append("Provider ").Append(ttCredential.Providers[0].Name).Append(" Not Currently Supported.").ToString());
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
                wsTravelTalk.modMain.LogResponse(ref strResponse, ref ttCredential, StartTime, ttServiceID, Server.MachineName, ref UUID);
                if (modCore.Trace)
                    CoreLib.SendTrace(ttCredential.UserID, "wsIssueTicket", "============= OTA Response ============= ", strResponse, UUID);
            }

            return strResponse;
        }

        #endregion

        #region  Web Methods 

        [CompressionExtension()]
        [WebMethod(Description = "Process Issue Ticket Messages Request.")]
        [SoapHeader("tXML")]
        public wsTravelTalk.wmIssueTicketOut.TT_IssueTicketRS wmIssueTicket(wsTravelTalk.wmIssueTicketIn.TT_IssueTicketRQ TT_IssueTicketRQ)
        {
            string xmlMessage;
            wsTravelTalk.wmIssueTicketOut.TT_IssueTicketRS oIssueTicketRS = (wsTravelTalk.wmIssueTicketOut.TT_IssueTicketRS)null;
            XmlSerializer oSerializer;
            System.IO.StringWriter oWriter;
            System.IO.StringReader oReader;

            oSerializer = new XmlSerializer(typeof(wsTravelTalk.wmIssueTicketIn.TT_IssueTicketRQ));
            oWriter = new System.IO.StringWriter(new StringBuilder());
            oSerializer.Serialize(oWriter, TT_IssueTicketRQ);
            xmlMessage = oWriter.ToString();
            xmlMessage = xmlMessage.Replace(" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", "");

            xmlMessage = ServiceRequest(xmlMessage, (int)ttServices.IssueTicket);

            try
            {
                oSerializer = new XmlSerializer(type: typeof(wsTravelTalk.wmIssueTicketOut.TT_IssueTicketRS));
                oReader = new System.IO.StringReader(xmlMessage);
                oIssueTicketRS = (wsTravelTalk.wmIssueTicketOut.TT_IssueTicketRS)oSerializer.Deserialize(oReader);
            }
            catch (Exception ex)
            {
                CoreLib.SendTrace("", "wsIssueTicket", "Error Deserialing OTA Response", ex.Message, string.Empty);
            }

            return oIssueTicketRS;

        }

        [WebMethod(Description = "Process Issue Ticket Xml Messages Request.")]
        public string wmIssueTicketXml(string xmlRequest)
        {
            return ServiceRequest(xmlRequest, (int)ttServices.IssueTicket);
        }

        #endregion

    }

}