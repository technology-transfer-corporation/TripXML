using System;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Web.Services;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.VisualBasic.CompilerServices;
using TripXMLMain;
using static TripXMLMain.modCore;
using static TripXMLTools.TripXMLLoad;

namespace wsTripXML.wsTravelTalk
{

    [System.Web.Services.Protocols.SoapDocumentService(RoutingStyle = System.Web.Services.Protocols.SoapServiceRoutingStyle.RequestElement)]
    [WebService(Namespace = "http://tripxml.downtowntravel.com/tripxml/wsQueueRead", Name = "wsQueueRead", Description = "A TripXML Web Service to Process QueueRead Messages Request.")]
    public class wsQueueRead : WebService
    {
        public TripXML tXML;

        #region  Web Services Designer Generated Code 

        public wsQueueRead() : base()
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
            XmlDocument oDoc;
            XmlElement oRoot;

            try
            {

                oDoc = new XmlDocument();
                oDoc.LoadXml(strResponse);
                oRoot = oDoc.DocumentElement;

                foreach (XmlNode oNode in oRoot.SelectNodes("TravelItinerary/ItineraryInfo/ReservationItems/Item/Air"))
                {
                    try
                    {
                        // *******************
                        // Decode Airports   *
                        // *******************
                        if (oNode.SelectSingleNode("DepartureAirport") is not null)
                        {
                            oNode.SelectSingleNode("DepartureAirport").InnerText = DecodeValue(DecodingType.Airport, oNode.SelectSingleNode("DepartureAirport").Attributes["LocationCode"].Value);
                            // GetDecodeValue(ttAirports, oNode.SelectSingleNode("DepartureAirport").Attributes("LocationCode").Value)
                        }
                        if (oNode.SelectSingleNode("ArrivalAirport") is not null)
                        {
                            oNode.SelectSingleNode("ArrivalAirport").InnerText = DecodeValue(DecodingType.Airport, oNode.SelectSingleNode("ArrivalAirport").Attributes["LocationCode"].Value);
                            // GetDecodeValue(ttAirports, oNode.SelectSingleNode("ArrivalAirport").Attributes("LocationCode").Value)
                        }

                        // *******************
                        // Decode Airlines   *
                        // *******************
                        if (oNode.SelectSingleNode("OperatingAirline") is not null)
                        {
                            if (oNode.SelectSingleNode("OperatingAirline").Attributes["Code"] is not null)
                            {
                                if (!string.IsNullOrEmpty(oNode.SelectSingleNode("OperatingAirline").Attributes["Code"].Value))
                                {
                                    oNode.SelectSingleNode("OperatingAirline").InnerText = DecodeValue(DecodingType.Airline, oNode.SelectSingleNode("OperatingAirline").Attributes["Code"].Value);
                                }
                                else if (oNode.SelectSingleNode("OperatingAirline") is not null)
                                {
                                    XmlAttribute attCode;
                                    attCode = oDoc.CreateAttribute("Code");
                                    attCode.Value = EncodeValue(DecodingType.Airline, oNode.SelectSingleNode("OperatingAirline").InnerText);
                                    oNode.SelectSingleNode("OperatingAirline").Attributes.Append(attCode);
                                    oNode.SelectSingleNode("OperatingAirline").InnerText = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(oNode.SelectSingleNode("OperatingAirline").InnerText.ToLower());
                                }
                            }
                            else
                            {
                                XmlAttribute attCode;
                                attCode = oDoc.CreateAttribute("Code");
                                attCode.Value = EncodeValue(DecodingType.Airline, oNode.SelectSingleNode("OperatingAirline").InnerText);
                                oNode.SelectSingleNode("OperatingAirline").Attributes.Append(attCode);
                                oNode.SelectSingleNode("OperatingAirline").InnerText = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(oNode.SelectSingleNode("OperatingAirline").InnerText.ToLower());
                            }
                        }
                        else if (oNode.SelectSingleNode("OperatingAirline") is not null)
                        {
                            XmlAttribute attCode;
                            attCode = oDoc.CreateAttribute("Code");
                            attCode.Value = EncodeValue(DecodingType.Airline, oNode.SelectSingleNode("OperatingAirline").InnerText);
                            oNode.SelectSingleNode("OperatingAirline").Attributes.Append(attCode);
                            oNode.SelectSingleNode("OperatingAirline").InnerText = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(oNode.SelectSingleNode("OperatingAirline").InnerText.ToLower());
                        }

                        if (oNode.SelectSingleNode("MarketingAirline") is not null)
                        {
                            oNode.SelectSingleNode("MarketingAirline").InnerText = DecodeValue(DecodingType.Airline, oNode.SelectSingleNode("MarketingAirline").Attributes["Code"].Value);
                            // GetDecodeValue(ttAirlines, oNode.SelectSingleNode("MarketingAirline").Attributes("Code").Value)
                        }

                        // *******************
                        // Decode Equipments   *
                        // *******************
                        if (oNode.SelectSingleNode("Equipment") is not null)
                        {
                            if (oNode.SelectSingleNode("Equipment").Attributes["AirEquipType"] is not null)
                            {
                                oNode.SelectSingleNode("Equipment").InnerText = DecodeValue(DecodingType.Equipment, oNode.SelectSingleNode("Equipment").Attributes["AirEquipType"].Value);
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
            bool validateXSDOut;
            var startTime = default(DateTime);
            string uuid = "";

            try
            {
                startTime = DateTime.Now;

                strRequest = strRequest.Replace(" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", "");
                strRequest = strRequest.Replace(" xmlns=\"http://tripxml.downtowntravel.com/tripxml/wsQueueRead\"", "");
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
                    // strResponse = SendPNRRequestAmadeus(ttServiceID, ttCredential, ttAA, strRequest)
                    // Application.Set(sb.Append("API").Append(ttCredential.UserID).Append(ttCredential.System).ToString(), ttAA)
                    // sb.Remove(0, sb.Length())

                    case "amadeusws":
                        {
                            strResponse = modMain.SendPNRRequestAmadeusWS(ttServiceID, ref ttCredential, ref ttProviderSystems, ref strRequest);
                            break;
                        }
                    case "apollo":
                    case "galileo":
                        {
                            ttProviderSystems.AAAPCC = ttCredential.Providers[0].PCC;
                            strResponse = modMain.SendPNRRequestGalileo(ttServiceID, ref ttCredential, ref ttProviderSystems, ref strRequest);
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
                            strResponse = modMain.SendPNRRequestSabre(ttServiceID, ref ttCredential, ref ttProviderSystems, ref strRequest);
                            break;
                        }

                    default:
                        {
                            throw new Exception(sb.Append("Provider ").Append(ttCredential.Providers[0].Name).Append(" Not Currently Supported.").ToString());
                        }
                }

                strResponse = DecodeQueueRead(strResponse, ttCredential.UserID);

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
                    CoreLib.SendTrace(ttCredential.UserID, "wsQueueRead", "============= OTA Response ============= ", strResponse, uuid);
            }
            sb = null;
            return strResponse;

        }

        #endregion

        #region  Web Methods 

        [CompressionExtension.CompressionExtension()]
        [WebMethod(Description = "Process QueueRead Messages Request.")]
        [System.Web.Services.Protocols.SoapHeader("tXML")]
        public wmTravelItineraryOut_v03.OTA_TravelItineraryRS wmQueueRead(wmQueueReadIn.OTA_QueueReadRQ OTA_QueueReadRQ)
        {
            string xmlMessage;
            wmTravelItineraryOut_v03.OTA_TravelItineraryRS oQueueReadRS;
            XmlSerializer oSerializer;
            System.IO.StringWriter oWriter;
            System.IO.StringReader oReader;

            oSerializer = new XmlSerializer(typeof(wmQueueReadIn.OTA_QueueReadRQ));
            oWriter = new System.IO.StringWriter(new StringBuilder());
            oSerializer.Serialize(oWriter, OTA_QueueReadRQ);
            xmlMessage = oWriter.ToString();
            xmlMessage = xmlMessage.Replace(" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", "");

            xmlMessage = ServiceRequest(xmlMessage, ttServices.QueueRead);

            if (xmlMessage.Contains("Object reference not set to an instance of an object"))
            {
                CoreLib.SendTrace("Platform", "wsTripXML", "Object reference returned on Queue Read.", oWriter.ToString(), string.Empty);
            }

            try
            {
                oSerializer = new XmlSerializer(@type: typeof(wmTravelItineraryOut_v03.OTA_TravelItineraryRS));
                oReader = new System.IO.StringReader(xmlMessage);
                oQueueReadRS = (wmTravelItineraryOut_v03.OTA_TravelItineraryRS)oSerializer.Deserialize(oReader);
            }
            catch (Exception ex)
            {

                XmlDocument oDoc;
                XmlElement oRoot;
                oDoc = new XmlDocument();
                oDoc.LoadXml(xmlMessage);
                oRoot = oDoc.DocumentElement;
                string sessionID = "";
                if (oRoot.SelectSingleNode("ConversationID") is not null)
                {
                    sessionID = oRoot.SelectSingleNode("ConversationID").OuterXml.Replace("&amp;", "&");
                }

                string itinRefXmlList;
                if (oRoot.SelectSingleNode("TravelItinerary/ItineraryRef")?.OuterXml is null)
                {
                    itinRefXmlList = string.Empty;
                }
                else
                {
                    itinRefXmlList = oRoot.SelectSingleNode("TravelItinerary/ItineraryRef").OuterXml;
                }

                string custInfoXmlList;
                if (oRoot.SelectSingleNode("TravelItinerary/CustomerInfos")?.OuterXml is null)
                {
                    custInfoXmlList = string.Empty;
                }
                else
                {
                    custInfoXmlList = oRoot.SelectSingleNode("TravelItinerary/CustomerInfos").OuterXml;
                }

                string tpaInfoXmlList;
                if (oRoot.SelectSingleNode("TravelItinerary/TPA_Extensions")?.OuterXml is null)
                {
                    tpaInfoXmlList = string.Empty;
                }
                else
                {
                    tpaInfoXmlList = oRoot.SelectSingleNode("TravelItinerary/TPA_Extensions").OuterXml;
                }
                string errMessage = string.Format("<Errors><Error>{0}</Error><Error>{1}</Error></Errors>", ex.InnerException.Message.ToString(), ex.Message.ToString());

                xmlMessage = string.Format("<OTA_TravelItineraryRS Version=\"v03\" xmlns:stl=\"http://services.sabre.com/STL/v01\">{0}<TravelItinerary>{1}{2}{3}{4}</TravelItinerary>{5}</OTA_TravelItineraryRS>", errMessage, itinRefXmlList, custInfoXmlList, "<ItineraryInfo></ItineraryInfo>", tpaInfoXmlList, sessionID);

                oReader = new System.IO.StringReader(xmlMessage);
                oQueueReadRS = (wmTravelItineraryOut_v03.OTA_TravelItineraryRS)oSerializer.Deserialize(oReader);
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