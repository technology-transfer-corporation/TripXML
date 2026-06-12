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
    [WebService(Namespace = "http://tripxml.downtowntravel.com/tripxml/wsDisplayTicket", Name = "wsDisplayTicket_v03", Description = "A TripXML Web Service to Process Display Ticket Messages Request.")]


    public class wsDisplayTicket_v03 : WebService
    {
        public TripXML tXML;

        #region  Web Services Designer Generated Code 

        public wsDisplayTicket_v03() : base()
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

        private string ServiceRequest(string strRequest, ttServices ttServiceID)
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
                modMain.PreServiceRequest(ref strRequest, ref argoApp, ref ttCredential, ref ttProviderSystems, StartTime, (int)ttServiceID, Server.MachineName, ref UUID);
                ValidateXSDOut = Conversions.ToBoolean(Application.Get(sb.Append("XSD").Append(ttCredential.UserID).Append("Out").ToString()));
                sb.Remove(0, sb.Length);

                switch (ttCredential.Providers[0].Name.ToLower() ?? "")
                {

                    case "amadeusws":
                        {

                            strResponse = modMain.SendTravelRequestAmadeusWS(ttServiceID, ref ttCredential, ref ttProviderSystems, ref strRequest, "v03");
                            break;
                        }

                    default:
                        {
                            throw new Exception(sb.Append("Provider ").Append(ttCredential.Providers[0].Name).Append(" Not Currently Supported.").ToString());
                            sb.Remove(0, sb.Length);
                            break;
                        }
                }

                // Dim StartCounter As Date
                // StartCounter = Now
                // Not Implemented DecodeIssueTicket(strResponse, ttCredential.UserID)
                // CoreLib.SendTrace(ttCredential.UserID, "Performance", "Decoding = ").Append(CType(Now.Subtract(StartCounter).TotalMilliseconds, Integer), "")

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
                    CoreLib.SendTrace(ttCredential.UserID, "wsIssueTicket", "============= OTA Response ============= ", strResponse, ttProviderSystems.LogUUID);
            }

            return strResponse;
            sb = null;
        }

        #endregion

        #region  Web Methods 

        [CompressionExtension()]
        [WebMethod(Description = "Process Void Ticket Messages Request.")]
        [SoapHeader("tXML")]
        public wmDisplayTicketOut_v03.TT_DisplayTicketRS wmDisplayTicket(wmDisplayTicketIn.TT_DisplayTicketRQ TT_DisplayTicketRQ)
        {
            string xmlMessage = "";
            wmDisplayTicketOut_v03.TT_DisplayTicketRS oDisplayTicketRS = null;
            XmlSerializer oSerializer = null;
            System.IO.StringWriter oWriter = null;
            System.IO.StringReader oReader = null;

            oSerializer = new XmlSerializer(typeof(wmDisplayTicketIn.TT_DisplayTicketRQ));
            oWriter = new System.IO.StringWriter(new StringBuilder());
            oSerializer.Serialize(oWriter, TT_DisplayTicketRQ);
            xmlMessage = oWriter.ToString();
            xmlMessage = xmlMessage.Replace(" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", "");

            xmlMessage = ServiceRequest(xmlMessage, ttServices.TicketDisplay);

            try
            {
                oSerializer = null;
                oSerializer = new XmlSerializer(@type: typeof(wmDisplayTicketOut_v03.TT_DisplayTicketRS));
                oReader = new System.IO.StringReader(xmlMessage);
                oDisplayTicketRS = (wmDisplayTicketOut_v03.TT_DisplayTicketRS)oSerializer.Deserialize(oReader);
            }
            catch (Exception ex)
            {
                CoreLib.SendTrace("", "wsDisplayTicket", "Error Deserialing OTA Response", ex.Message, string.Empty);
                xmlMessage = "<TT_DisplayTicketRS Version=\"v03\"><Errors><Error>" + ex.InnerException.ToString() + "</Error></Errors></TT_DisplayTicketRS>";
                oReader = new System.IO.StringReader(xmlMessage);
                oDisplayTicketRS = (wmDisplayTicketOut_v03.TT_DisplayTicketRS)oSerializer.Deserialize(oReader);
            }

            return oDisplayTicketRS;

        }

        [WebMethod(Description = "Process Void Ticket Xml Messages Request.")]
        public string wmDisplayTicketXml(string xmlRequest)
        {
            return ServiceRequest(xmlRequest, ttServices.TicketDisplay);
        }

        #endregion

    }

}