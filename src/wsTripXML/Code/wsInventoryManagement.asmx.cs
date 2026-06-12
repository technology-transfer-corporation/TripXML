using System;
using System.Diagnostics;
using System.Text;
using System.Web.Services;
using System.Xml;
using System.Xml.Serialization;
using CompressionExtension;
using Microsoft.VisualBasic.CompilerServices;
using TripXMLMain;
using static TripXMLMain.modCore;
using TripXMLTools;

namespace wsTripXML.wsTravelTalk
{

    [System.Web.Services.Protocols.SoapDocumentService(RoutingStyle = System.Web.Services.Protocols.SoapServiceRoutingStyle.RequestElement)]
    [WebService(Namespace = "http://tripxml.downtowntravel.com/tripxml/wsInventoryManagement", Name = "wsInventoryManagement", Description = "A TripXML Web Service to work with inventory management.")]
    public class wsInventoryManagement : WebService
    {
        public TripXML tXML;

        #region  Web Services Designer Generated Code 

        public wsInventoryManagement() : base()
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

        #region  Decode Function 

        private string DecodeTXMLInventoryManagement(string strResponse, string UserID)
        {
            try
            {

                var oDoc = new XmlDocument();
                oDoc.LoadXml(strResponse);

                // Dim ttAirports As DataView = CType(Application.Get("ttAirports"), DataView)
                // Dim ttAirlines As DataView = CType(Application.Get("ttAirlines"), DataView)

                var oRoot = oDoc.DocumentElement;
                foreach (XmlNode oNode in oRoot.SelectNodes("Deals/Deal"))
                {
                    foreach (XmlNode oFlightNode in oNode.SelectNodes("OriginDestinationOption"))
                    {
                        // *******************
                        // *******************
                        // Decode Airports   *
                        // *******************
                        oFlightNode.SelectSingleNode("OriginLocation").InnerText = TripXMLLoad.DecodeValue(TripXMLLoad.DecodingType.Airport, oFlightNode.SelectSingleNode("OriginLocation").Attributes["LocationCode"].Value);
                        // GetDecodeValue(ttAirports, oFlightNode.SelectSingleNode("OriginLocation").Attributes("LocationCode").Value)
                        oFlightNode.SelectSingleNode("DestinationLocation").InnerText = TripXMLLoad.DecodeValue(TripXMLLoad.DecodingType.Airport, oFlightNode.SelectSingleNode("DestinationLocation").Attributes["LocationCode"].Value);
                        // GetDecodeValue(ttAirports, oFlightNode.SelectSingleNode("DestinationLocation").Attributes("LocationCode").Value)

                        // *******************
                        // Decode Airlines   *
                        // *******************
                        if (oFlightNode.SelectSingleNode("MarketingAirline") is not null)
                        {
                            oFlightNode.SelectSingleNode("MarketingAirline").InnerText = TripXMLLoad.DecodeValue(TripXMLLoad.DecodingType.Airline, oNode.SelectSingleNode("MarketingAirline").Attributes["Code"].Value);
                            // GetDecodeValue(ttAirlines, oFlightNode.SelectSingleNode("MarketingAirline").Attributes("Code").Value)
                        }
                    }
                }

                strResponse = oDoc.OuterXml;
            }

            catch (Exception ex)
            {
                CoreLib.SendTrace(UserID, "wsTXMLInventoryManagement", "Error *** Decoding InventoryManagement Response", ex.Message, string.Empty);
            }
            return strResponse;
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
            DateTime StartTime;
            string UUID = "";

            try
            {
                StartTime = DateTime.Now;
                var argoApp = Application;
                modMain.PreServiceRequest(ref strRequest, ref argoApp, ref ttCredential, ref ttProviderSystems, StartTime, (int)ttServiceID, Server.MachineName, ref UUID);
                ValidateXSDOut = Conversions.ToBoolean(Application.Get(sb.Append("XSD").Append(ttCredential.UserID).Append("Out").ToString()));
                sb.Remove(0, sb.Length);

                switch (ttCredential.Providers[0].Name ?? "")
                {


                    // 'strResponse = SendOtherRequestSITA(ttServiceID, ttCredential, ttProviderSystems, strRequest)

                    case "SITA":
                        {
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
                    CoreLib.SendTrace(ttCredential.UserID, "wsTXMLInventoryManagement", "============= TXML Response ============= ", strResponse, UUID);
            }

            return strResponse;
            sb = null;
        }

        #endregion

        #region  Web Methods 

        [CompressionExtension()]
        [WebMethod(Description = "Process Inventory Management Messages Request.")]
        [System.Web.Services.Protocols.SoapHeader("tXML")]
        public wmInventoryManagementOut.TXML_InventoryManagementRS wmInventoryManagement(wmInventoryManagementIn.TXML_InventoryManagementRQ TXML_InventoryManagementRQ)
        {
            string xmlMessage = "";
            wmInventoryManagementOut.TXML_InventoryManagementRS oInventoryManagementRS = null;
            XmlSerializer oSerializer = null;
            System.IO.StringWriter oWriter = null;
            System.IO.StringReader oReader = null;

            oSerializer = new XmlSerializer(typeof(wmInventoryManagementIn.TXML_InventoryManagementRQ));
            oWriter = new System.IO.StringWriter(new StringBuilder());
            oSerializer.Serialize(oWriter, TXML_InventoryManagementRQ);
            xmlMessage = oWriter.ToString();
            xmlMessage = xmlMessage.Replace(" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", "");

            xmlMessage = ServiceRequest(xmlMessage, ttServices.InventoryManagement);

            try
            {
                oSerializer = null;
                oSerializer = new XmlSerializer(@type: typeof(wmInventoryManagementOut.TXML_InventoryManagementRS));
                oReader = new System.IO.StringReader(xmlMessage);
                oInventoryManagementRS = (wmInventoryManagementOut.TXML_InventoryManagementRS)oSerializer.Deserialize(oReader);
            }
            catch (Exception ex)
            {
                CoreLib.SendTrace("", "wsInventoryManagement", "Error Deserialing TXML Response", ex.Message, string.Empty);
            }

            return oInventoryManagementRS;

        }

        [WebMethod(Description = "Process Inventory Management Xml Messages Request.")]
        public string wmTXMLInventoryManagementXml(string xmlRequest)
        {
            return ServiceRequest(xmlRequest, ttServices.InventoryManagement);
        }

        #endregion

    }

}