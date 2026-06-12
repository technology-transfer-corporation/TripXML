using System;
using System.Diagnostics;
using System.Text;
using System.Web.Services;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.VisualBasic.CompilerServices;
using TripXMLMain;
using static TripXMLMain.modCore;
using TripXMLTools;

namespace wsTripXML.wsTravelTalk
{

    [System.Web.Services.Protocols.SoapDocumentService(RoutingStyle = System.Web.Services.Protocols.SoapServiceRoutingStyle.RequestElement)]
    [WebService(Namespace = "http://tripxml.downtowntravel.com/tripxml/wsSalesReport", Name = "wsSalesReport", Description = "A TripXML Web Service to Process SalesReport Messages Request.")]
    public class wsSalesReport : WebService
    {
        public TripXML tXML;

        #region  Web Services Designer Generated Code 

        public wsSalesReport() : base()
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

        // Not Implemented

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

                switch (ttCredential.Providers[0].Name ?? "")
                {
                    case "Amadeus":
                        {
                            break;
                        }
                    // Dim ttAA As AmadeusAPIAdapter

                    // ttAA = Application.Get(sb.Append("API").Append(ttCredential.UserID).Append(ttCredential.System).Append(ttCredential.Providers(0).PCC).ToString())
                    // sb.Remove(0, sb.Length())
                    // If ttAA Is Nothing Then
                    // Throw New Exception(sb.Append("Access denied to Amadeus - ").Append(ttCredential.System).Append(" system. Or invalid provider.").ToString())
                    // sb.Remove(0, sb.Length())
                    // End If

                    // If ttCredential.Providers(0).PCC.Trim.Length > 0 Then
                    // ttAA.SourcePCC = ttCredential.Providers(0).PCC
                    // End If

                    // strResponse = SendOtherRequestAmadeus(ttServiceID, ttCredential, ttAA, strRequest)
                    // Application.Set(sb.Append("API").Append(ttCredential.UserID).Append(ttCredential.System).ToString(), ttAA)
                    // sb.Remove(0, sb.Length())

                    case "AmadeusWS":
                        {
                            strResponse = modMain.SendOtherRequestAmadeusWS(ttServiceID, ref ttCredential, ref ttProviderSystems, ref strRequest, "v03");
                            break;
                        }
                    case "Sabre":
                        {
                            strResponse = modMain.SendOtherRequestSabre(ttServiceID, ref ttCredential, ref ttProviderSystems, ref strRequest);
                            // DecodeSalesReport(strResponse) Not Implemented.
                            strResponse = DecodeSalesReport(strResponse, ttCredential.UserID);
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
                modMain.LogResponse(ref strResponse, ref ttCredential, StartTime, (int)ttServiceID, Server.MachineName, ref UUID);
                if (modCore.Trace)
                    CoreLib.SendTrace(ttCredential.UserID, "wsSalesReport", "============= OTA Response ============= ", strResponse, UUID);
            }
            sb = null;
            return strResponse;

        }

        #endregion

        #region  Decode Function 

        private string DecodeSalesReport(string strResponse, string UserID)
        {

            try
            {

                var oDoc = new XmlDocument();
                oDoc.LoadXml(strResponse);

                // Dim ttAirlines As DataView = CType(Application.Get("ttAirlines"), DataView)

                var oRoot = oDoc.DocumentElement;
                foreach (XmlNode oNode in oRoot.SelectNodes("JournalEntries/JournalEntry"))
                {
                    try
                    {

                        // *******************
                        // Decode Airlines   *
                        // *******************
                        if (oNode.SelectSingleNode("Airline") is not null & oNode.SelectSingleNode("Airline").Attributes["Code"] is not null)
                        {
                            if (!string.IsNullOrEmpty(oNode.SelectSingleNode("Airline").Attributes["Code"].Value))
                            {
                                oNode.SelectSingleNode("Airline").InnerText = TripXMLLoad.DecodeValue(TripXMLLoad.DecodingType.Airline, oNode.SelectSingleNode("Airline").Attributes["Code"].Value);
                                // GetCodeValue(ttAirlines, oNode.SelectSingleNode("Airline").Attributes("Code").Value)
                            }
                        }
                    }

                    catch (Exception e)
                    {
                        CoreLib.SendTrace(UserID, "wsSalesReport", "Error *** Decoding Airline Response", e.Message, string.Empty);
                    }

                }

                strResponse = oDoc.OuterXml;
            }

            catch (Exception ex)
            {
                CoreLib.SendTrace(UserID, "wsSalesReport", "Error *** Decoding Airline Response", ex.Message, string.Empty);
            }
            return strResponse;
        }

        #endregion

        #region  Web Methods 

        [CompressionExtension.CompressionExtension()]
        [WebMethod(Description = "Process SalesReport Messages Request.")]
        [System.Web.Services.Protocols.SoapHeader("tXML")]
        public wmSalesReportOut.SalesReportRS wmSalesReport(wmSalesReportIn.SalesReportRQ SalesReportRQ)
        {
            string xmlMessage = "";
            wmSalesReportOut.SalesReportRS oSalesReportRS = null;
            XmlSerializer oSerializer = null;
            System.IO.StringWriter oWriter = null;
            System.IO.StringReader oReader = null;

            oSerializer = new XmlSerializer(typeof(wmSalesReportIn.SalesReportRQ));
            oWriter = new System.IO.StringWriter(new StringBuilder());
            oSerializer.Serialize(oWriter, SalesReportRQ);
            xmlMessage = oWriter.ToString();
            xmlMessage = xmlMessage.Replace(" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", "");

            xmlMessage = ServiceRequest(xmlMessage, ttServices.SalesReport);

            try
            {
                oSerializer = null;
                oSerializer = new XmlSerializer(@type: typeof(wmSalesReportOut.SalesReportRS));
                oReader = new System.IO.StringReader(xmlMessage);
                oSalesReportRS = (wmSalesReportOut.SalesReportRS)oSerializer.Deserialize(oReader);
            }

            catch (Exception ex)
            {
                CoreLib.SendTrace("", "wsSalesReport", "Error Deserialing OTA Response", ex.Message, string.Empty);
            }

            return oSalesReportRS;

        }

        [WebMethod(Description = "Process SalesReport Xml Messages Request.")]
        public string wmSalesReportXml(string xmlRequest)
        {
            return ServiceRequest(xmlRequest, ttServices.SalesReport);
        }

        #endregion

    }

}