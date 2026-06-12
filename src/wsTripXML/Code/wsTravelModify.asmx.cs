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
    [WebService(Namespace = "http://tripxml.downtowntravel.com/tripxml/wsTravelModify", Name = "wsTravelModify", Description = "A TripXML Web Service to Process Travel Build Request.")]
    public class wsTravelModify : WebService
    {
        public TripXML tXML;

        #region  Web Services Designer Generated Code 

        public wsTravelModify() : base()
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

        private string DecodeTravelModify(string strResponse, string UserID)
        {
            XmlDocument oDoc = null;
            XmlElement oRoot = null;

            try
            {

                oDoc = new XmlDocument();
                oDoc.LoadXml(strResponse);
                oRoot = oDoc.DocumentElement;

                foreach (XmlNode oNode in oRoot.SelectNodes("TravelItinerary/ItineraryInfo/ReservationItems/Item/Air"))
                {
                    // *******************
                    // Decode Airports   *
                    // *******************
                    if (oNode.SelectSingleNode("DepartureAirport") is not null)
                    {
                        oNode.SelectSingleNode("DepartureAirport").InnerText = TripXMLLoad.DecodeValue(TripXMLLoad.DecodingType.Airport, oNode.SelectSingleNode("DepartureAirport").Attributes["LocationCode"].Value);
                        // GetDecodeValue(ttAirports, oNode.SelectSingleNode("DepartureAirport").Attributes("LocationCode").Value)
                    }
                    if (oNode.SelectSingleNode("ArrivalAirport") is not null)
                    {
                        oNode.SelectSingleNode("ArrivalAirport").InnerText = TripXMLLoad.DecodeValue(TripXMLLoad.DecodingType.Airport, oNode.SelectSingleNode("ArrivalAirport").Attributes["LocationCode"].Value);
                        // GetDecodeValue(ttAirports, oNode.SelectSingleNode("ArrivalAirport").Attributes("LocationCode").Value)
                    }

                    // *******************
                    // Decode Airlines   *
                    // *******************
                    if (oNode.SelectSingleNode("OperatingAirline") is not null)
                    {
                        if (oNode.SelectSingleNode("OperatingAirline").Attributes["Code"] is not null)
                        {
                            oNode.SelectSingleNode("OperatingAirline").InnerText = TripXMLLoad.DecodeValue(TripXMLLoad.DecodingType.Airline, oNode.SelectSingleNode("OperatingAirline").Attributes["Code"].Value);
                            // GetDecodeValue(ttAirlines, oNode.SelectSingleNode("OperatingAirline").Attributes("Code").Value)
                        }
                    }
                    if (oNode.SelectSingleNode("MarketingAirline") is not null)
                    {
                        if (oNode.SelectSingleNode("MarketingAirline").Attributes["Code"] is not null)
                        {
                            oNode.SelectSingleNode("MarketingAirline").InnerText = TripXMLLoad.DecodeValue(TripXMLLoad.DecodingType.Airline, oNode.SelectSingleNode("MarketingAirline").Attributes["Code"].Value);
                            // GetDecodeValue(ttAirlines, oNode.SelectSingleNode("MarketingAirline").Attributes("Code").Value)
                        }
                    }
                    // *******************
                    // Decode Equipments *
                    // *******************
                    if (oNode.SelectSingleNode("Equipment") is not null)
                    {
                        if (oNode.SelectSingleNode("Equipment").Attributes["AirEquipType"] is not null)
                        {
                            oNode.SelectSingleNode("Equipment").InnerText = TripXMLLoad.DecodeValue(TripXMLLoad.DecodingType.Equipment, oNode.SelectSingleNode("Equipment").Attributes["AirEquipType"].Value);
                            // GetDecodeValue(ttEquipments, oNode.SelectSingleNode("Equipment").Attributes("AirEquipType").Value)
                        }
                    }
                }

                strResponse = oDoc.OuterXml;
            }

            catch (Exception ex)
            {
                CoreLib.SendTrace(UserID, "wsTravelServices", "Error *** Decoding TravelModify Response", ex.Message, string.Empty);
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
            bool validateXSDOut;
            var startTime = default(DateTime);
            string uuid = "";

            try
            {
                startTime = DateTime.Now;

                var argoApp = Application;
                modMain.PreServiceRequest(ref strRequest, ref argoApp, ref ttCredential, ref ttProviderSystems, startTime, (int)ttServiceID, Server.MachineName, ref uuid);
                validateXSDOut = Conversions.ToBoolean(Application.Get(sb.Append("XSD").Append(ttCredential.UserID).Append("Out").ToString()));
                sb.Remove(0, sb.Length);

                switch (ttCredential.Providers[0].Name.ToLower() ?? "")
                {
                    case "amadeus":
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

                    // strResponse = SendTravelRequestAmadeus(ttServiceID, ttCredential, ttAA, strRequest)
                    // Application.Set(sb.Append("API").Append(ttCredential.UserID).Append(ttCredential.System).ToString(), ttAA)
                    // sb.Remove(0, sb.Length())

                    case "apollo":
                    case "galileo":
                        {

                            strResponse = modMain.SendTravelRequestGalileo(ttServiceID, ref ttCredential, ref ttProviderSystems, ref strRequest);
                            break;
                        }

                    case "sabre":
                        {


                            // Case "outriggerr"

                            // strResponse = SendTravelRequestOutriggerR(ttServiceID, ttCredential, ttProviderSystems, strRequest)

                            // Case "worldspan"

                            // strResponse = SendTravelRequestWorldspan(ttServiceID, ttCredential, ttProviderSystems, strRequest)

                            // Case "agentware"

                            // strResponse = SendTravelRequestAgentWare(ttServiceID, ttCredential, ttProviderSystems, strRequest)

                            // Case "sentient"

                            // strResponse = SendTravelRequestSentient(ttServiceID, ttCredential, ttProviderSystems, strRequest)

                            strResponse = modMain.SendTravelRequestSabre(ttServiceID, ref ttCredential, ref ttProviderSystems, ref strRequest);
                            break;
                        }

                    default:
                        {
                            throw new Exception(sb.Append("Provider ").Append(ttCredential.Providers[0].Name).Append(" Not Currently Supported.").ToString());
                            sb.Remove(0, sb.Length);
                            break;
                        }
                }

                strResponse = DecodeTravelModify(strResponse, ttCredential.UserID);

                modMain.PostServiceRequest(ref strResponse, validateXSDOut, (int)ttServiceID, ttCredential.UserID);
            }

            catch (Exception ex)
            {
                strResponse = FormatErrorMessage(ttServiceID, ex.Message, ttCredential.Providers[0].Name);
            }
            finally
            {
                modMain.LogResponse(ref strResponse, ref ttCredential, startTime, (int)ttServiceID, Server.MachineName, ref uuid);
                if (modCore.Trace)
                    CoreLib.SendTrace(ttCredential.UserID, "wsTravelModify", "============= OTA Response ============= ", strResponse, uuid);
            }
            sb = null;
            return strResponse;

        }

        #endregion

        #region  Web Methods 

        [CompressionExtension.CompressionExtension()]
        [WebMethod(Description = "Process Travel Modify Messages Request.")]
        [System.Web.Services.Protocols.SoapHeader("tXML")]
        public wmTravelItineraryOut.OTA_TravelItineraryRS wmTravelModify(wmTravelModifyIn.OTA_TravelModifyRQ OTA_TravelModifyRQ)
        {
            string xmlMessage = "";
            wmTravelItineraryOut.OTA_TravelItineraryRS oTravelModifyRS = null;
            XmlSerializer oSerializer = null;
            System.IO.StringWriter oWriter = null;
            System.IO.StringReader oReader = null;

            oSerializer = new XmlSerializer(typeof(wmTravelModifyIn.OTA_TravelModifyRQ));
            oWriter = new System.IO.StringWriter(new StringBuilder());
            oSerializer.Serialize(oWriter, OTA_TravelModifyRQ);
            xmlMessage = oWriter.ToString();
            xmlMessage = xmlMessage.Replace(" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", "");

            xmlMessage = ServiceRequest(xmlMessage, ttServices.TravelModify);

            try
            {
                oSerializer = null;
                oSerializer = new XmlSerializer(@type: typeof(wmTravelItineraryOut.OTA_TravelItineraryRS));
                oReader = new System.IO.StringReader(xmlMessage);
                oTravelModifyRS = (wmTravelItineraryOut.OTA_TravelItineraryRS)oSerializer.Deserialize(oReader);
            }
            catch (Exception ex)
            {
                CoreLib.SendTrace("", "wsTravelModify", "Error Deserialing OTA Response", ex.Message, string.Empty);
            }

            return oTravelModifyRS;

        }

        [CompressionExtension.CompressionExtension()]
        [WebMethod(Description = "Process Travel Modify MCO Messages Request.")]
        [System.Web.Services.Protocols.SoapHeader("tXML")]
        public wmTravelItineraryOut.OTA_TravelItineraryRS wmIssueMCO(wmTravelModifyIn.OTA_TravelModifyRQ OTA_TravelModifyRQ)
        {
            string xmlMessage = "";
            wmTravelItineraryOut.OTA_TravelItineraryRS oTravelModifyRS = null;
            XmlSerializer oSerializer;
            System.IO.StringWriter oWriter;
            System.IO.StringReader oReader;

            oSerializer = new XmlSerializer(typeof(wmTravelModifyIn.OTA_TravelModifyRQ));
            oWriter = new System.IO.StringWriter(new StringBuilder());
            oSerializer.Serialize(oWriter, OTA_TravelModifyRQ);
            xmlMessage = oWriter.ToString();
            xmlMessage = xmlMessage.Replace(" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", "");

            xmlMessage = ServiceRequest(xmlMessage, ttServices.PNRSplit);

            try
            {
                oSerializer = new XmlSerializer(@type: typeof(wmTravelItineraryOut.OTA_TravelItineraryRS));
                oReader = new System.IO.StringReader(xmlMessage);
                oTravelModifyRS = (wmTravelItineraryOut.OTA_TravelItineraryRS)oSerializer.Deserialize(oReader);
            }
            catch (Exception ex)
            {
                CoreLib.SendTrace("", "wmIssueMCO", "Error Deserialing OTA Response", ex.Message, string.Empty);
            }

            return oTravelModifyRS;

        }

        [WebMethod(Description = "Process Travel Modify Xml Messages Request.")]
        public string wmTravelModifyXml(string xmlRequest)
        {
            return ServiceRequest(xmlRequest, ttServices.TravelModify);
        }

        #endregion

    }

}