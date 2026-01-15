using System;
using System.Diagnostics;
using System.Text;
using System.Web.Services;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using TripXMLMain;
using static TripXMLMain.modCore;

namespace wsTripXML.wsTravelTalk
{

    [System.Web.Services.Protocols.SoapDocumentService(RoutingStyle = System.Web.Services.Protocols.SoapServiceRoutingStyle.RequestElement)]
    [WebService(Namespace = "http://tripxml.downtowntravel.com/tripxml/wsPing", Name = "wsPing", Description = "A TripXML Web Service to Process Display Ticket Messages Request.")]
    public class wsPing : WebService
    {
        public TripXML tXML;

        #region  Web Services Designer Generated Code 

        public wsPing() : base()
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

                            XmlDocument oDoc = null;
                            XmlElement oRoot = null;
                            XmlNode oNode = null;
                            double dTime;
                            int iWait = 10;

                            oDoc = new XmlDocument();
                            oDoc.LoadXml(strRequest);
                            oRoot = oDoc.DocumentElement;

                            if (oRoot.SelectSingleNode("WaitTime") is not null)
                            {

                                iWait = Conversions.ToInteger(oRoot.SelectSingleNode("WaitTime").InnerText);

                            }

                            dTime = DateAndTime.Timer;
                            while (DateAndTime.Timer - dTime <= iWait)
                            {
                            }

                            strResponse = "<TXML_PingRS><Success/></TXML_PingRS>";
                            break;
                        }

                    default:
                        {
                            throw new Exception(sb.Append("Provider ").Append(ttCredential.Providers[0].Name).Append(" Not Currently Supported.").ToString());
                            sb.Remove(0, sb.Length);
                            break;
                        }
                }
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
            sb = null;
        }

        #endregion

        #region  Web Methods 

        [CompressionExtension.CompressionExtension()]
        [WebMethod(Description = "Process Void Ticket Messages Request.")]
        [System.Web.Services.Protocols.SoapHeader("tXML")]
        public wmPingOut.TXML_PingRS wmPing(wmPingIn.TXML_PingRQ TXML_PingRQ)
        {
            string xmlMessage = "";
            wmPingOut.TXML_PingRS oPingRS = null;
            XmlSerializer oSerializer = null;
            System.IO.StringWriter oWriter = null;
            System.IO.StringReader oReader = null;

            oSerializer = new XmlSerializer(typeof(wmPingIn.TXML_PingRQ));
            oWriter = new System.IO.StringWriter(new StringBuilder());
            oSerializer.Serialize(oWriter, TXML_PingRQ);
            xmlMessage = oWriter.ToString();
            xmlMessage = xmlMessage.Replace(" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", "");

            xmlMessage = ServiceRequest(xmlMessage, ttServices.Ping);

            try
            {
                oSerializer = null;
                oSerializer = new XmlSerializer(@type: typeof(wmPingOut.TXML_PingRS));
                oReader = new System.IO.StringReader(xmlMessage);
                oPingRS = (wmPingOut.TXML_PingRS)oSerializer.Deserialize(oReader);
            }
            catch (Exception ex)
            {
                CoreLib.SendTrace("", "wsPing", "Error Deserialing OTA Response", ex.Message, string.Empty);
            }

            return oPingRS;

        }

        [WebMethod(Description = "Process Void Ticket Xml Messages Request.")]
        public string wmPingXml(string xmlRequest)
        {
            return ServiceRequest(xmlRequest, ttServices.TicketDisplay);
        }

        #endregion

        [WebMethod(Description = "Ping get")]
        public string ping(int Delay)
        {
            double dTime;
            int iWait = 10;

            if (Delay != 0)
            {

                iWait = Delay;

            }

            dTime = DateAndTime.Timer;
            while (DateAndTime.Timer - dTime <= iWait)
            {
            }

            return "Waited " + iWait.ToString() + " seconds.";
        }

    }

    // Public Class _Default
    // Inherits Page
    // Protected Sub Page_Load(sender As Object, e As EventArgs)
    // Dim v As String = Request.QueryString("Delay")
    // If v IsNot Nothing Then
    // Response.Write("param is ")
    // Response.Write(v)
    // End If
    // End Sub
    // End Class


}