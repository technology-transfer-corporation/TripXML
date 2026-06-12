using System;
using System.Data;
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
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
    // <System.Web.Script.Services.ScriptService()> _
    [System.Web.Services.Protocols.SoapDocumentService(RoutingStyle = System.Web.Services.Protocols.SoapServiceRoutingStyle.RequestElement)]
    [WebService(Namespace = "http://tripxml.downtowntravel.com/tripxml/wsAirSchedule", Name = "wsAirSchedule", Description = "A TripXML Web Service to Process Air Schedule Messages Request.")]
    public class wsAirSchedule : WebService
    {
        private StringBuilder sb = new StringBuilder();
        public TripXML tXML;
        private string mstrResponse = "";
        private int mintProviders = 0;

        #region  Decode Functions 

        private string DecodeAirFlifo(string strResponse, string UserID)
        {
            XmlDocument oDoc = null;
            XmlElement oRoot = null;
            DataView ttAirports;
            DataView ttAirlines;
            DataView ttEquipments;

            try
            {

                oDoc = new XmlDocument();
                oDoc.LoadXml(strResponse);
                oRoot = oDoc.DocumentElement;

                ttAirports = (DataView)Application.Get("ttAirports");
                ttAirlines = (DataView)Application.Get("ttAirlines");
                ttEquipments = (DataView)Application.Get("ttEquipments");

                foreach (XmlNode oNode in oRoot.SelectNodes("FlightInfoDetails/FlightLegInfo"))
                {
                    // *******************
                    // Decode Airports   *
                    // *******************
                    oNode.SelectSingleNode("DepartureAirport").InnerText = DecodeValue(DecodingType.Airport, oNode.SelectSingleNode("DepartureAirport").Attributes["LocationCode"].Value);
                    oNode.SelectSingleNode("ArrivalAirport").InnerText = DecodeValue(DecodingType.Airport, oNode.SelectSingleNode("ArrivalAirport").Attributes["LocationCode"].Value);

                    // *******************
                    // Decode Airlines   *
                    // *******************
                    if (oNode.SelectSingleNode("OperatingAirline") is not null)
                    {
                        oNode.SelectSingleNode("OperatingAirline").InnerText = DecodeValue(DecodingType.Airline, oNode.SelectSingleNode("OperatingAirline").Attributes["Code"].Value);
                    }
                    if (oNode.SelectSingleNode("MarketingAirline") is not null)
                    {
                        oNode.SelectSingleNode("MarketingAirline").InnerText = DecodeValue(DecodingType.Airline, oNode.SelectSingleNode("MarketingAirline").Attributes["Code"].Value);
                    }

                    // *******************
                    // Decode Equipments *
                    // *******************
                    if (oNode.SelectSingleNode("Equipment") is not null)
                    {
                        oNode.SelectSingleNode("Equipment").InnerText = DecodeValue(DecodingType.Equipment, oNode.SelectSingleNode("Equipment").Attributes["AirEquipType"].Value);
                    }
                }

                strResponse = oDoc.OuterXml;
            }

            catch (Exception ex)
            {
                CoreLib.SendTrace(UserID, "wsAirServices", "Error *** Decoding AirFlifo Response", ex.Message, string.Empty);
            }
            return strResponse;
        }

        #endregion

        #region  Process Service Request All GDS 

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

                    // strResponse = SendAirRequestAmadeus(ttServiceID, ttCredential, ttAA, strRequest)
                    // Application.Set(sb.Append("API").Append(ttCredential.UserID).Append(ttCredential.System).ToString(), ttAA)
                    // sb.Remove(0, sb.Length())

                    case "AmadeusWS":
                        {
                            strResponse = modMain.SendAirRequestAmadeusWS(ttServiceID, ref ttCredential, ref ttProviderSystems, ref strRequest);
                            break;
                        }
                    case "Apollo":
                    case "Galileo":
                        {
                            strResponse = modMain.SendAirRequestGalileo(ttServiceID, ref ttCredential, ref ttProviderSystems, ref strRequest);
                            break;
                        }
                    case "Sabre":
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

                            strResponse = modMain.SendAirRequestSabre(ttServiceID, ref ttCredential, ref ttProviderSystems, ref strRequest);
                            break;
                        }
                    case "Travelport":
                        {
                            strResponse = modMain.SendAirRequestTravelport(ttServiceID, ref ttCredential, ref ttProviderSystems, ref strRequest);
                            break;
                        }

                    default:
                        {
                            throw new Exception(sb.Append("Provider ").Append(ttCredential.Providers[0].Name).Append(" Not Currently Supported.").ToString());
                            sb.Remove(0, sb.Length);
                            break;
                        }
                }

                DateTime StartCounter;
                StartCounter = DateTime.Now;

                strResponse = DecodeAirFlifo(strResponse, ttCredential.UserID);

                CoreLib.SendTrace(ttCredential.UserID, "Performance", sb.Append("Decoding = ").Append((int)Math.Round(DateTime.Now.Subtract(StartCounter).TotalMilliseconds)).ToString(), "", ttProviderSystems.LogUUID);
                sb.Remove(0, sb.Length);

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
                    CoreLib.SendTrace(ttCredential.UserID, "wsAirSchedule", "============= OTA Response ============= ", strResponse, ttProviderSystems.LogUUID);
            }

            return strResponse;
            sb = null;
        }

        #endregion

        #region Web Methods
        [CompressionExtension.CompressionExtension()]
        [WebMethod(Description = "Process Air Schedule Messages Request.")]
        [System.Web.Services.Protocols.SoapHeader("tXML")]
        public wmAirScheduleOut.OTA_AirScheduleRS wmAirSchedule(wmAirScheduleIn.OTA_AirScheduleRQ OTA_AirScheduleRQ)
        {
            string xmlMessage = "";
            wmAirScheduleOut.OTA_AirScheduleRS oAirScheduleRS = null;
            XmlSerializer oSerializer = null;
            System.IO.StringWriter oWriter = null;
            System.IO.StringReader oReader = null;

            oSerializer = new XmlSerializer(typeof(wmAirScheduleIn.OTA_AirScheduleRQ));
            oWriter = new System.IO.StringWriter(new StringBuilder());
            oSerializer.Serialize(oWriter, OTA_AirScheduleRQ);
            xmlMessage = oWriter.ToString();
            xmlMessage = xmlMessage.Replace(" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", "");

            xmlMessage = ServiceRequest(xmlMessage, ttServices.AirSchedule);

            try
            {
                oSerializer = null;
                oSerializer = new XmlSerializer(@type: typeof(wmAirScheduleOut.OTA_AirScheduleRS));
                oReader = new System.IO.StringReader(xmlMessage);
                oAirScheduleRS = (wmAirScheduleOut.OTA_AirScheduleRS)oSerializer.Deserialize(oReader);
            }
            catch (Exception ex)
            {
                CoreLib.SendTrace("", "wsAirFlifo", "Error Deserialing OTA Response", ex.Message, string.Empty);
            }

            return oAirScheduleRS;

        }
        [WebMethod(Description = "Process Air Schedule Xml Messages Request.")]
        public string wmAirScheduleXml(string xmlRequest)
        {
            return ServiceRequest(xmlRequest, ttServices.AirSchedule);
        }
        #endregion
    }
}