using System;
using System.Diagnostics;
using System.Text;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Serialization;
using CompressionExtension;
using TripXMLMain;
using static TripXMLMain.modCore;

namespace wsTripXML.wsTravelTalk
{

    [SoapDocumentService(RoutingStyle = SoapServiceRoutingStyle.RequestElement)]
    [WebService(Namespace = "http://tripxml.downtowntravel.com/tripxml/wsIssueMCO", Name = "wsIssueMCO", Description = "A TripXML Web Service to Process Issue MCO Messages Request.")]
    public class wsIssueMCO : WebService
    {
        public TripXML tXML;

        #region  Web Services Designer Generated Code 

        public wsIssueMCO() : base()
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

        private string ServiceRequest(string strRequest, ttServices ttServiceID)
        {
            string strResponse = "";
            TravelTalkCredential ttCredential = default;
            TripXMLProviderSystems ttProviderSystems = default;
            var ValidateXSDOut = default(bool);
            var StartTime = DateTime.Now;
            string UUID = "";

            try
            {
                var argoApp = Application;
                modMain.PreServiceRequest(ref strRequest, ref argoApp, ref ttCredential, ref ttProviderSystems, StartTime, (int)ttServiceID, Server.MachineName, ref UUID);

                switch (ttCredential.Providers[0].Name.ToLower() ?? "")
                {
                    case "apollo":
                    case "galileo":
                        {
                            strResponse = modMain.SendTravelRequestGalileo(ttServiceID, ref ttCredential, ref ttProviderSystems, ref strRequest);
                            break;
                        }
                    case "sabre":
                        {
                            if (ttProviderSystems.System is null)
                            {
                                FormatErrorMessage(ttServiceID, $"Access denied to {ttCredential.Providers[0].Name} - {ttCredential.System} system. Or invalid provider.", ttCredential.Providers[0].Name);
                                break;
                            }
                            ttProviderSystems.AAAPCC = ttCredential.Providers[0].PCC;
                            strResponse = modMain.SendTravelRequestSabre(ttServiceID, ref ttCredential, ref ttProviderSystems, ref strRequest);
                            break;
                        }

                    default:
                        {
                            throw new Exception($"Provider {ttCredential.Providers[0].Name} Not Currently Supported.");
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
                modMain.LogResponse(ref strResponse, ref ttCredential, StartTime, (int)ttServiceID, Server.MachineName, ref UUID);
                if (modCore.Trace)
                    CoreLib.SendTrace(ttCredential.UserID, "wsIssueTicket", "============= OTA Response ============= ", strResponse, UUID);
            }

            return strResponse;
        }

        #endregion

        #region  Web Methods 

        [CompressionExtension()]
        [WebMethod(Description = "Process Issue MCO Messages Request.")]
        [SoapHeader("tXML")]
        public wmIssueMCOOut.TT_IssueMCORS wmIssueMCO(wmIssueMCOIn.TT_IssueMCORQ TT_IssueMCORQ)
        {
            string xmlMessage;
            wmIssueMCOOut.TT_IssueMCORS oIssueMCORS = null;
            XmlSerializer oSerializer;
            System.IO.StringWriter oWriter;
            System.IO.StringReader oReader;

            oSerializer = new XmlSerializer(typeof(wmIssueMCOIn.TT_IssueMCORQ));
            oWriter = new System.IO.StringWriter(new StringBuilder());
            oSerializer.Serialize(oWriter, TT_IssueMCORQ);
            xmlMessage = oWriter.ToString();
            xmlMessage = xmlMessage.Replace(" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", "");

            xmlMessage = ServiceRequest(xmlMessage, ttServices.IssueMCO);

            try
            {
                oSerializer = new XmlSerializer(@type: typeof(wmIssueMCOOut.TT_IssueMCORS));
                oReader = new System.IO.StringReader(xmlMessage);
                oIssueMCORS = (wmIssueMCOOut.TT_IssueMCORS)oSerializer.Deserialize(oReader);
            }
            catch (Exception ex)
            {
                CoreLib.SendTrace("", "wsIssueMCO", "Error Deserialing OTA Response", ex.Message, string.Empty);
            }

            return oIssueMCORS;

        }

        [WebMethod(Description = "Process Issue Ticket Xml Messages Request.")]
        public string wmIssueMCOXml(string xmlRequest)
        {
            return ServiceRequest(xmlRequest, ttServices.IssueTicket);
        }

        #endregion

    }

}