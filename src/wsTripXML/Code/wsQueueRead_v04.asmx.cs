using System;
using System.Diagnostics;
using System.Text;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml;
using System.Xml.Serialization;
using CompressionExtension;
using Microsoft.VisualBasic.CompilerServices;
using TripXMLMain;
using static TripXMLMain.modCore;
using TripXMLTools;

namespace wsTripXML.wsTravelTalk
{

    [SoapDocumentService(RoutingStyle = SoapServiceRoutingStyle.RequestElement)]
    [WebService(Namespace = "http://tripxml.downtowntravel.com/tripxml/wsQueueRead", Name = "wsQueueRead_v04", Description = "A TripXML Web Service to Process QueueRead Messages Request version v04.")]
    public class wsQueueRead_v04 : WebService
    {
        public TripXML tXML;

        #region  Web Services Designer Generated Code 

        public wsQueueRead_v04() : base()
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

        private string DecodeQueueRead(string strResponse, string UserID)
        {
            try
            {

                var oDoc = new XmlDocument();
                oDoc.LoadXml(strResponse);
                var oRoot = oDoc.DocumentElement;
                foreach (XmlNode oNode in oRoot.SelectNodes("TravelItinerary/ItineraryInfo/ReservationItems/Item/Air"))
                {
                    try
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
                            oNode.SelectSingleNode("ArrivalAirport").InnerText = TripXMLLoad.DecodeValue(TripXMLLoad.DecodingType.Airport, oNode.SelectSingleNode("DepartureAirport").Attributes["LocationCode"].Value);
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
                            oNode.SelectSingleNode("MarketingAirline").InnerText = TripXMLLoad.DecodeValue(TripXMLLoad.DecodingType.Airline, oNode.SelectSingleNode("MarketingAirline").Attributes["Code"].Value);
                            // GetDecodeValue(ttAirlines, oNode.SelectSingleNode("MarketingAirline").Attributes("Code").Value)
                        }

                        // *******************
                        // Decode Equipments   *
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
                    catch (Exception e)
                    {
                        CoreLib.SendTrace(UserID, "wsPNRRead", "Error *** Decoding AirAvail Response", e.Message, string.Empty);
                    }

                }

                strResponse = oDoc.OuterXml;
            }

            catch (Exception ex)
            {
                CoreLib.SendTrace(UserID, "wsPNRRead", "Error *** Decoding AirAvail Response", ex.Message, string.Empty);
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
            var StartTime = default(DateTime);
            string UUID = "";

            try
            {
                StartTime = DateTime.Now;

                strRequest = strRequest.Replace(" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", "");
                strRequest = strRequest.Replace(" xmlns=\"http://tripxml.downtowntravel.com/tripxml/wsQueueRead\"", "");
                var argoApp = Application;
                modMain.PreServiceRequest(ref strRequest, ref argoApp, ref ttCredential, ref ttProviderSystems, StartTime, (int)ttServiceID, Server.MachineName, ref UUID);
                ValidateXSDOut = Conversions.ToBoolean(Application.Get(sb.Append("XSD").Append(ttCredential.UserID).Append("Out").ToString()));
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
                    // strResponse = SendPNRRequestAmadeus(ttServiceID, ttCredential, ttAA, strRequest, "v04")
                    // Application.Set(sb.Append("API").Append(ttCredential.UserID).Append(ttCredential.System).ToString(), ttAA)
                    // sb.Remove(0, sb.Length())

                    case "amadeusws":
                        {

                            strResponse = modMain.SendPNRRequestAmadeusWS(ttServiceID, ref ttCredential, ref ttProviderSystems, ref strRequest, "v04");
                            break;
                        }

                    case "apollo":
                    case "galileo":
                        {

                            strResponse = modMain.SendPNRRequestGalileo(ttServiceID, ref ttCredential, ref ttProviderSystems, ref strRequest, "v04");
                            break;
                        }

                    case "sabre":
                        {

                            // ttProviderSystems = Application.Get(sb.Append("PS").Append(ttCredential.Providers(0).Name).Append(ttCredential.UserID).Append(ttCredential.System).Append(ttCredential.Providers(0).PCC).ToString())
                            // sb.Remove(0, sb.Length())
                            if (ttProviderSystems.System is null)
                            {
                                FormatErrorMessage(ttServiceID, sb.Append("Access denied to ").Append(ttCredential.Providers[0].Name).Append(" - ").Append(ttCredential.System).Append(" system. Or invalid provider.").ToString(), ttCredential.Providers[0].Name);
                                sb.Remove(0, sb.Length);
                                break;
                            }

                            ttProviderSystems.AAAPCC = ttCredential.Providers[0].PCC;

                            strResponse = modMain.SendPNRRequestSabre(ttServiceID, ref ttCredential, ref ttProviderSystems, ref strRequest, "v04");
                            break;
                        }

                    default:
                        {
                            throw new Exception(sb.Append("Provider ").Append(ttCredential.Providers[0].Name).Append(" Not Currently Supported.").ToString());
                            sb.Remove(0, sb.Length);
                            break;
                        }
                }

                strResponse = DecodeQueueRead(strResponse, ttCredential.UserID);

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
                    CoreLib.SendTrace(ttCredential.UserID, "wsQueueRead", "============= OTA Response ============= ", strResponse, UUID);
            }
            sb = null;
            return strResponse;

        }

        #endregion

        #region  Web Methods 

        [CompressionExtension()]
        [WebMethod(Description = "Process QueueRead Messages Request.")]
        [SoapHeader("tXML")]
        public wmTravelItineraryOut_v04.OTA_TravelItineraryRS wmQueueRead(wmQueueReadIn.OTA_QueueReadRQ OTA_QueueReadRQ)
        {
            string xmlMessage = "";
            wmTravelItineraryOut_v04.OTA_TravelItineraryRS oQueueReadRS = null;
            XmlSerializer oSerializer = null;
            System.IO.StringWriter oWriter = null;
            System.IO.StringReader oReader = null;

            oSerializer = new XmlSerializer(typeof(wmQueueReadIn.OTA_QueueReadRQ));
            oWriter = new System.IO.StringWriter(new StringBuilder());
            oSerializer.Serialize(oWriter, OTA_QueueReadRQ);
            xmlMessage = oWriter.ToString();
            xmlMessage = xmlMessage.Replace(" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", "");

            xmlMessage = ServiceRequest(xmlMessage, ttServices.QueueRead);

            if (xmlMessage.IndexOf("Object reference not set to an instance of an object") != -1)
            {
                // CoreLib.SendEmail("QueueRead message", "Object reference.")
            }

            try
            {
                oSerializer = new XmlSerializer(@type: typeof(wmTravelItineraryOut_v04.OTA_TravelItineraryRS));
                oReader = new System.IO.StringReader(xmlMessage);
                oQueueReadRS = (wmTravelItineraryOut_v04.OTA_TravelItineraryRS)oSerializer.Deserialize(oReader);
            }
            catch (Exception ex)
            {
                CoreLib.SendTrace("", "wsQueueRead", "Error Deserialing OTA Response", ex.Message, string.Empty);
            }

            return oQueueReadRS;

        }

        [WebMethod(Description = "Process QueueRead well format Xml Messages Request.")]
        public string wmQueueReadXml(string xmlRequest)
        {
            return ServiceRequest(xmlRequest, ttServices.QueueRead);
        }

        #endregion

    }
}