using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
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
    [WebService(Namespace = "http://tripxml.downtowntravel.com/tripxml/wsPNRRead", Name = "wsPNRRead", Description = "A TripXML Web Service to Process PNR Read Request - version 03.")]
    public class wsPNRRead_v03 : WebService
    {
        public wsTravelTalk.TripXML tXML;

        #region  Web Services Designer Generated Code 

        public wsPNRRead_v03() : base()
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

        private string DecodePNRRead(string strResponse, string UserID, string UUID)
        {
            XmlDocument oDoc;
            XmlElement oRoot;

            try
            {

                oDoc = new XmlDocument();
                oDoc.LoadXml(strResponse);
                oRoot = oDoc.DocumentElement;

                var testNode = oRoot.SelectSingleNode("TravelItinerary/ItineraryInfo/ReservationItems/Item/Air");

                if (testNode is null)
                {
                    CoreLib.SendTrace(UserID, "wsPNRRead", "Error *** No Air Segments in PNR", "", UUID);
                }
                else
                {

                    foreach (XmlNode oNode in oRoot.SelectNodes("TravelItinerary/ItineraryInfo/ReservationItems/Item/Air"))
                    {
                        try
                        {
                            var arnkElem = oNode.SelectSingleNode("TPA_Extensions/Arnk");
                            if (arnkElem is not null)
                            {
                                continue;
                            }
                            // *******************
                            // Decode Airports   *
                            // *******************
                            if (oNode.SelectSingleNode("DepartureAirport") is not null)
                            {
                                // oNode.SelectSingleNode("DepartureAirport").InnerText = DecodeValue(ttAirports, oNode.SelectSingleNode("DepartureAirport").Attributes("LocationCode").Value)
                                oNode.SelectSingleNode("DepartureAirport").InnerText = DecodeValue(DecodingType.Airport, oNode.SelectSingleNode("DepartureAirport").Attributes["LocationCode"].Value);
                                // DecodeValue(DecodingType.Airport, oNode.SelectSingleNode("DepartureAirport").Attributes("LocationCode").Value)
                            }
                            if (oNode.SelectSingleNode("ArrivalAirport") is not null)
                            {
                                oNode.SelectSingleNode("ArrivalAirport").InnerText = DecodeValue(DecodingType.Airport, oNode.SelectSingleNode("ArrivalAirport").Attributes["LocationCode"].Value);
                                // DecodeValue(DecodingType.Airport, oNode.SelectSingleNode("ArrivalAirport").Attributes("LocationCode").Value)
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
                                        // GetEncodeValue(ttAirlinesNames, oNode.SelectSingleNode("OperatingAirline").InnerText)
                                        oNode.SelectSingleNode("OperatingAirline").Attributes.Append(attCode);

                                        oNode.SelectSingleNode("OperatingAirline").InnerText = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(oNode.SelectSingleNode("OperatingAirline").InnerText.ToLower());
                                    }
                                }
                                else if (oNode.SelectSingleNode("OperatingAirline") is not null)
                                {
                                    XmlAttribute attCode;
                                    attCode = oDoc.CreateAttribute("Code");
                                    if (oNode.SelectSingleNode("OperatingAirline").Attributes["Code"] is not null)
                                    {
                                        attCode.Value = DecodeValue(DecodingType.Airline, oNode.SelectSingleNode("OperatingAirline").Attributes["Code"].Value);
                                        // GetEncodeValue(ttAirlines, oNode.SelectSingleNode("OperatingAirline").InnerText)

                                        if (!string.IsNullOrEmpty(attCode.Value))
                                        {
                                            oNode.SelectSingleNode("OperatingAirline").Attributes.Append(attCode);
                                            oNode.SelectSingleNode("OperatingAirline").InnerText = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(oNode.SelectSingleNode("OperatingAirline").InnerText.ToLower());
                                        }
                                    }
                                }
                            }
                            else if (oNode.SelectSingleNode("OperatingAirline") is not null)
                            {
                                XmlAttribute attCode;
                                attCode = oDoc.CreateAttribute("Code");
                                attCode.Value = EncodeValue(DecodingType.Airline, oNode.SelectSingleNode("OperatingAirline").InnerText);
                                // GetEncodeValue(ttAirlinesNames, oNode.SelectSingleNode("OperatingAirline").InnerText)
                                oNode.SelectSingleNode("OperatingAirline").Attributes.Append(attCode);

                                oNode.SelectSingleNode("OperatingAirline").InnerText = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(oNode.SelectSingleNode("OperatingAirline").InnerText.ToLower());
                            }

                            if (oNode.SelectSingleNode("MarketingAirline") is not null)
                            {
                                oNode.SelectSingleNode("MarketingAirline").InnerText = DecodeValue(DecodingType.Airline, oNode.SelectSingleNode("MarketingAirline").Attributes["Code"].Value);
                                // DecodeValue(DecodingType.Airline, oNode.SelectSingleNode("MarketingAirline").Attributes("Code").Value)
                            }

                            // *******************
                            // Decode Equipments   *
                            // *******************
                            if (oNode.SelectSingleNode("Equipment") is not null)
                            {
                                if (oNode.SelectSingleNode("Equipment").Attributes["AirEquipType"] is not null)
                                {
                                    oNode.SelectSingleNode("Equipment").InnerText = DecodeValue(DecodingType.Equipment, oNode.SelectSingleNode("Equipment").Attributes["AirEquipType"].Value);
                                    // DecodeValue(DecodingType.Equipment, oNode.SelectSingleNode("Equipment").Attributes("AirEquipType").Value)
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            CoreLib.SendTrace(UserID, "wsPNRRead", "Error *** Decoding AirAvail Response", e.Message, UUID);
                        }

                    }
                }

                strResponse = oDoc.OuterXml;
            }

            catch (Exception ex)
            {
                CoreLib.SendTrace(UserID, "wsPNRRead", "Error *** Decoding AirAvail Response", ex.Message, UUID);
            }
            return strResponse;
        }

        #endregion

        #region  Process Service Request All GDS 
        private StringBuilder sb = new StringBuilder();

        private string ServiceRequest(string strRequest, int ttServiceID)
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
                wsTravelTalk.modMain.PreServiceRequest(ref strRequest, ref argoApp, ref ttCredential, ref ttProviderSystems, startTime, ttServiceID, Server.MachineName, ref uuid);
                validateXSDOut = Conversions.ToBoolean(Application.Get(sb.Append("XSD").Append(ttCredential.UserID).Append("Out").ToString()));
                sb.Remove(0, sb.Length);

                switch (ttCredential.Providers[0].Name ?? "")
                {
                    case "AmadeusWS":
                        {
                            strResponse = wsTravelTalk.modMain.SendPNRRequestAmadeusWS((ttServices)ttServiceID, ref ttCredential, ref ttProviderSystems, ref strRequest, "v03");
                            break;
                        }
                    case "Apollo":
                    case "Galileo":
                        {
                            strResponse = wsTravelTalk.modMain.SendPNRRequestGalileo((ttServices)ttServiceID, ref ttCredential, ref ttProviderSystems, ref strRequest, "v03");
                            break;
                        }
                    case "Sabre":
                        {
                            if (ttProviderSystems.System is null)
                            {
                                FormatErrorMessage((ttServices)ttServiceID, sb.Append("Access denied to ").Append(ttCredential.Providers[0].Name).Append(" - ").Append(ttCredential.System).Append(" system. Or invalid provider.").ToString(), ttCredential.Providers[0].Name);
                                sb.Remove(0, sb.Length);
                                break;
                            }

                            ttProviderSystems.AAAPCC = ttCredential.Providers[0].PCC;
                            strResponse = wsTravelTalk.modMain.SendPNRRequestSabre((ttServices)ttServiceID, ref ttCredential, ref ttProviderSystems, ref strRequest, "v03");
                            break;
                        }

                    case "Travelport":
                        {
                            strResponse = wsTravelTalk.modMain.SendPNRRequestTravelPort((ttServices)ttServiceID, ref ttCredential, ref ttProviderSystems, ref strRequest, "v03");
                            break;
                        }
                    case "Worldspan":
                        {
                            var ttDefProvider = new TripXMLProviderSystems();
                            var argoApp1 = Application;
                            wsTravelTalk.modMain.PreServiceRequest(ref strRequest, ref argoApp1, ref ttCredential, ref ttDefProvider, startTime, ttServiceID, Server.MachineName, ref uuid, "", true);
                            // strResponse = SendPNRRequestWorldspan(ttServiceID, ttCredential, ttProviderSystems, strRequest, "v03")
                            strResponse = wsTravelTalk.modMain.SendPNRRequestTravelPort((ttServices)ttServiceID, ref ttCredential, ref ttDefProvider, ref strRequest, "v03");
                            break;
                        }

                    default:
                        {
                            throw new Exception(sb.Append("Provider ").Append(ttCredential.Providers[0].Name).Append(" Not Currently Supported.").ToString());
                        }
                }

                strResponse = DecodePNRRead(strResponse, ttCredential.UserID, uuid);

                wsTravelTalk.modMain.PostServiceRequest(ref strResponse, validateXSDOut, ttServiceID, ttCredential.UserID);
            }

            catch (Exception ex)
            {
                strResponse = FormatErrorMessage((ttServices)ttServiceID, ex.Message, ttCredential.Providers[0].Name);
            }
            finally
            {
                wsTravelTalk.modMain.LogResponse(ref strResponse, ref ttCredential, startTime, ttServiceID, Server.MachineName, ref uuid);
                if (modCore.Trace)
                    CoreLib.SendTrace(ttCredential.UserID, "wsPNRRead", "============= OTA Response ============= ", strResponse, uuid);
            }
            sb = null;
            return strResponse;

        }

        #endregion

        #region  Web Methods 

        [CompressionExtension.CompressionExtension()]
        [WebMethod(Description = "Process PNR Read Messages Request.")]
        [System.Web.Services.Protocols.SoapHeader("tXML")]
        public wsTravelTalk.wmTravelItineraryOut_v03.OTA_TravelItineraryRS wmPNRRead(wsTravelTalk.wmPNRReadIn.OTA_ReadRQ OTA_ReadRQ)
        {

            wsTravelTalk.wmTravelItineraryOut_v03.OTA_TravelItineraryRS oPNRReadRS;
            string xmlMessage = string.Empty;
            try
            {
                var oSerializer = new XmlSerializer(typeof(wsTravelTalk.wmPNRReadIn.OTA_ReadRQ));
                var oWriter = new StringWriter(new StringBuilder());
                oSerializer.Serialize(oWriter, OTA_ReadRQ);

                xmlMessage = oWriter.ToString();
                xmlMessage = xmlMessage.Replace(" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", "");

                xmlMessage = ServiceRequest(xmlMessage, (int)ttServices.PNRRead);

                oSerializer = new XmlSerializer(type: typeof(wsTravelTalk.wmTravelItineraryOut_v03.OTA_TravelItineraryRS));
                var oReader = new StringReader(xmlMessage);
                oPNRReadRS = (wsTravelTalk.wmTravelItineraryOut_v03.OTA_TravelItineraryRS)oSerializer.Deserialize(oReader);
            }
            catch (Exception ex)
            {
                oPNRReadRS = GetErrorPNRObject(ex, xmlMessage);
            }

            return oPNRReadRS;

        }

        private wsTravelTalk.wmTravelItineraryOut_v03.OTA_TravelItineraryRS GetErrorPNRObject(Exception ex, string xmlMessage)
        {
            wsTravelTalk.wmTravelItineraryOut_v03.OTA_TravelItineraryRS oPNRReadRS;
            var errList = new List<Exception>();

            try
            {
                errList.Add(ex);
                StringReader oReader;
                XmlSerializer oSerializer;
                XmlDocument oDoc;
                XmlElement oRoot;
                oDoc = new XmlDocument();
                oDoc.LoadXml(xmlMessage);
                oRoot = oDoc.DocumentElement;
                string sessionID = "";

                if (oRoot.SelectSingleNode("ConversationID") is not null)
                {
                    sessionID = oRoot.SelectSingleNode("ConversationID").InnerText;
                }

                string itinRefXmlList;
                wsTravelTalk.wmTravelItineraryOut_v03.ItineraryRef oItinRef;

                var oCustInfos = new wsTravelTalk.wmTravelItineraryOut_v03.CustomerInfosRS();
                wsTravelTalk.wmTravelItineraryOut_v03.TPA_ExtensionsRS oTPA;

                try
                {
                    itinRefXmlList = oRoot.SelectSingleNode("TravelItinerary/ItineraryRef")?.OuterXml;
                    oSerializer = new XmlSerializer(type: typeof(wsTravelTalk.wmTravelItineraryOut_v03.ItineraryRef));
                    oReader = new StringReader(itinRefXmlList);
                    oItinRef = (wsTravelTalk.wmTravelItineraryOut_v03.ItineraryRef)oSerializer.Deserialize(oReader);
                }
                catch (Exception eref)
                {
                    oItinRef = new wsTravelTalk.wmTravelItineraryOut_v03.ItineraryRef();
                    errList.Add(eref);
                }

                string custInfoXmlList;
                try
                {
                    custInfoXmlList = oRoot.SelectSingleNode("TravelItinerary/CustomerInfos")?.OuterXml;
                    oSerializer = new XmlSerializer(type: typeof(wsTravelTalk.wmTravelItineraryOut_v03.CustomerInfosRS), new XmlRootAttribute("CustomerInfos"));
                    oReader = new StringReader(custInfoXmlList);
                    oCustInfos = (wsTravelTalk.wmTravelItineraryOut_v03.CustomerInfosRS)oSerializer.Deserialize(oReader);
                }

                catch (Exception ecust)
                {
                    oCustInfos = new wsTravelTalk.wmTravelItineraryOut_v03.CustomerInfosRS();
                    errList.Add(ecust);
                }

                string tpaInfoXmlList;
                try
                {
                    tpaInfoXmlList = oRoot.SelectSingleNode("TravelItinerary/TPA_Extensions")?.OuterXml;
                    oSerializer = new XmlSerializer(type: typeof(wsTravelTalk.wmTravelItineraryOut_v03.TPA_ExtensionsRS), new XmlRootAttribute("TPA_Extensions"));
                    oReader = new StringReader(tpaInfoXmlList);
                    oTPA = (wsTravelTalk.wmTravelItineraryOut_v03.TPA_ExtensionsRS)oSerializer.Deserialize(oReader);
                }
                catch (Exception etpa)
                {
                    oTPA = new wsTravelTalk.wmTravelItineraryOut_v03.TPA_ExtensionsRS();
                    errList.Add(etpa);
                }

                var travelItin = new wsTravelTalk.wmTravelItineraryOut_v03.TravelItinerary()
                {
                    ItineraryRef = oItinRef,
                    CustomerInfos = oCustInfos,
                    TPA_Extensions = oTPA
                };

                oPNRReadRS = new wsTravelTalk.wmTravelItineraryOut_v03.OTA_TravelItineraryRS()
                {
                    Errors = GetErrorObject(errList),
                    ConversationID = sessionID,
                    TravelItinerary = travelItin,
                    Success = (string)null
                };
            }

            catch (Exception exX)
            {
                errList.Add(exX);

                oPNRReadRS = new wsTravelTalk.wmTravelItineraryOut_v03.OTA_TravelItineraryRS() { Errors = GetErrorObject(errList) };
            }

            return oPNRReadRS;
        }

        private wsTravelTalk.wmTravelItineraryOut_v03.Error[] GetErrorObject(List<Exception> exs)
        {
            var errMessage = new List<wsTravelTalk.wmTravelItineraryOut_v03.Error>();
            try
            {
                foreach (Exception ex in exs)
                {
                    errMessage.Add(new wsTravelTalk.wmTravelItineraryOut_v03.Error() { Value = ex.Message });
                    if (ex.InnerException is not null)
                    {
                        errMessage.Add(new wsTravelTalk.wmTravelItineraryOut_v03.Error() { Value = ex.InnerException.Message });
                    }
                }
            }
            catch (Exception exp)
            {
                errMessage.Add(new wsTravelTalk.wmTravelItineraryOut_v03.Error() { Value = exp.Message });
                errMessage.Add(new wsTravelTalk.wmTravelItineraryOut_v03.Error() { Value = exs.FirstOrDefault().Message });
            }

            return errMessage.ToArray();
        }

        [WebMethod(Description = "Process PNR Read Xml Messages Request.")]
        public string wmPNRReadXml(string xmlRequest)
        {
            return ServiceRequest(xmlRequest, (int)ttServices.PNRRead);
        }



        #endregion

    }

}