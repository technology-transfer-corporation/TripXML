using System;
using System.Diagnostics;
using System.Text;
using System.Web.Services;
using System.Xml.Serialization;
using Microsoft.VisualBasic.CompilerServices;
using TripXMLMain;
using static TripXMLMain.modCore;
using wsTripXML.wsTravelTalk.wmAuthorizationIn;
using wsTripXML.wsTravelTalk.wmAuthorizationOut;

namespace wsTripXML.wsTravelTalk
{

    [System.Web.Services.Protocols.SoapDocumentService(RoutingStyle = System.Web.Services.Protocols.SoapServiceRoutingStyle.RequestElement)]
    [WebService(Namespace = "http://tripxml.downtowntravel.com/tripxml/wsAuthorization", Name = "wsAuthorization", Description = "A TripXML Web Service to Process Authorization command.")]
    public class wsAuthorization : WebService
    {
        public TripXML tXML;


        #region  Web Services Designer Generated Code 

        public wsAuthorization() : base()
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
                            strResponse = modMain.SendTravelRequestAmadeusWS(ttServiceID, ref ttCredential, ref ttProviderSystems, ref strRequest);
                            break;
                        }

                    case "worldspan":
                        {
                            strResponse = modMain.SendTravelRequestWorldspan(ttServiceID, ref ttCredential, ref ttProviderSystems, ref strRequest);
                            break;
                        }

                    default:
                        {
                            throw new Exception(sb.Append("Provider ").Append(ttCredential.Providers[0].Name).Append(" Not Currently Supported.").ToString());
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
                    CoreLib.SendTrace(ttCredential.UserID, "wsAuthorization", "============= OTA Response ============= ", strResponse, UUID);
            }

            return strResponse;
        }

        #endregion

        #region  Web Methods 

        [CompressionExtension.CompressionExtension]
        [WebMethod(Description = "Process Authorization Messages Request.")]
        [System.Web.Services.Protocols.SoapHeader("tXML")]
        public OTA_AuthorizationRS wmAuthorization(OTA_AuthorizationRQ OTA_AuthorizationRQ)
        {
            string xmlMessage = "";
            OTA_AuthorizationRS oAuthorizationRS = null;
            XmlSerializer oSerializer;
            System.IO.StringWriter oWriter;
            System.IO.StringReader oReader;

            oSerializer = new XmlSerializer(typeof(OTA_AuthorizationRQ));
            oWriter = new System.IO.StringWriter(new StringBuilder());
            oSerializer.Serialize(oWriter, OTA_AuthorizationRQ);
            xmlMessage = oWriter.ToString();
            xmlMessage = xmlMessage.Replace(" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", "");
            xmlMessage = xmlMessage.Replace(" xmlns=\"http://www.opentravel.org/OTA/2003/05\"", "");

            xmlMessage = ServiceRequest(xmlMessage, ttServices.Authorization);

            try
            {
                oSerializer = new XmlSerializer(@type: typeof(OTA_AuthorizationRS));
                oReader = new System.IO.StringReader(xmlMessage);
                oAuthorizationRS = (OTA_AuthorizationRS)oSerializer.Deserialize(oReader);
                oAuthorizationRS.Authorization.AuthorizationDetail.BookingReferenceID = new BookingReferenceID();
                oAuthorizationRS.Authorization.AuthorizationDetail.BookingReferenceID.ID = OTA_AuthorizationRQ.AuthorizationDetail.BookingReferenceID.ID;
                oAuthorizationRS.Authorization.AuthorizationDetail.BookingReferenceID.Type = OTA_AuthorizationRQ.AuthorizationDetail.BookingReferenceID.Type;

                if (!string.IsNullOrEmpty(oAuthorizationRS.Authorization.AuthorizationResult.AuthorizationCode))
                {
                    oAuthorizationRS.Authorization.AuthorizationResult.Result = Detail.Approved;
                }
            }

            catch (Exception ex)
            {
                CoreLib.SendTrace("", "wsAuthorization", "Error Deserialing OTA Response", ex.Message, string.Empty);
            }

            return oAuthorizationRS;

        }

        [WebMethod(Description = "Process Authorization Xml Messages Request.")]
        public string wmAuthorizationXml(string xmlRequest)
        {
            return ServiceRequest(xmlRequest, ttServices.Authorization);
        }

        #endregion

    }
}