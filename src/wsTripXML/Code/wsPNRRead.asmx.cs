using System;
using System.Data;
using System.Diagnostics;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.VisualBasic.CompilerServices;
using TripXMLMain;
using static TripXMLMain.modCore;
using static TripXMLTools.TripXMLLoad;

namespace wsTripXML.wsTravelTalk
{
    public partial class wsPNRRead
    {
        public TripXML tXML;

        private readonly modMain _modMain;

        public wsPNRRead(modMain modMain)
        {
            _modMain = modMain;
        }

        #region  Decode Function 

        private string DecodePNRRead(string strResponse, string UserID, ref string strUUID)
        {
            XmlDocument oDoc;
            XmlElement oRoot;
            DataView ttAirports;
            DataView ttAirlines;
            DataView ttEquipments;

            try
            {

                oDoc = new XmlDocument();
                oDoc.LoadXml(strResponse);
                oRoot = oDoc.DocumentElement;

                ttAirports = (DataView)TripXMLMain.AppState.Get("ttAirports");
                ttAirlines = (DataView)TripXMLMain.AppState.Get("ttAirlines");
                ttEquipments = (DataView)TripXMLMain.AppState.Get("ttEquipments");

                var nsmgr = new XmlNamespaceManager(oDoc.NameTable);
                var testNode = oDoc.SelectSingleNode("TravelItinerary/ItineraryInfo/ReservationItems/Item", nsmgr);

                if (testNode["Air"] is null)
                {
                    CoreLib.SendTrace(UserID, "wsPNRRead", "Error * No Air Segments in PNR", "", strUUID);
                }
                else
                {

                    foreach (XmlNode oNode in oRoot.SelectNodes("TravelItinerary/ItineraryInfo/ReservationItems/Item/Air"))
                    {
                        try
                        {
                            if (oNode is not null)
                            {
                                // *******************
                                // Decode Airports   *
                                // *******************
                                if (oNode.SelectSingleNode("DepartureAirport") is not null)
                                {
                                    oNode.SelectSingleNode("DepartureAirport").InnerText = DecodeValue(DecodingType.Airport, oNode.SelectSingleNode("DepartureAirport").Attributes["LocationCode"].Value);
                                }
                                if (oNode.SelectSingleNode("ArrivalAirport") is not null)
                                {
                                    oNode.SelectSingleNode("ArrivalAirport").InnerText = DecodeValue(DecodingType.Airport, oNode.SelectSingleNode("ArrivalAirport").Attributes["LocationCode"].Value);
                                }

                                // *******************
                                // Decode Airlines   *
                                // *******************
                                if (oNode.SelectSingleNode("OperatingAirline") is not null & oNode.SelectSingleNode("OperatingAirline").Attributes["Code"] is not null)
                                {
                                    if (!string.IsNullOrEmpty(oNode.SelectSingleNode("OperatingAirline").Attributes["Code"].Value))
                                    {
                                        oNode.SelectSingleNode("OperatingAirline").InnerText = DecodeValue(DecodingType.Airline, oNode.SelectSingleNode("OperatingAirline").Attributes["Code"].Value);
                                    }
                                }

                                if (oNode.SelectSingleNode("MarketingAirline") is not null)
                                {
                                    oNode.SelectSingleNode("MarketingAirline").InnerText = DecodeValue(DecodingType.Airline, oNode.SelectSingleNode("MarketingAirline").Attributes["Code"].Value);
                                }

                                // *******************
                                // Decode Equipments   *
                                // *******************
                                if (oNode.SelectSingleNode("Equipment") is not null)
                                {
                                    if (oNode.SelectSingleNode("Equipment").Attributes["AirEquipType"] is not null)
                                    {
                                        oNode.SelectSingleNode("Equipment").InnerText = DecodeValue(DecodingType.Equipment, oNode.SelectSingleNode("Equipment").Attributes["AirEquipType"].Value);
                                    }
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            CoreLib.SendTrace(UserID, "wsPNRRead", "Error ** Decoding AirAvail Response", e.Message, strUUID);
                        }

                    }
                }

                strResponse = oDoc.OuterXml;
            }

            catch (Exception ex)
            {
                CoreLib.SendTrace(UserID, "wsPNRRead", "Error *** Decoding AirAvail Response", ex.Message, strUUID);
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
                strRequest = strRequest.Replace(" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns=\"http://tripxml.downtowntravel.com/tripxml/wsQueue\"", "");
                _modMain.PreServiceRequest(ref strRequest, ref ttCredential, ref ttProviderSystems, StartTime, (int)ttServiceID, Environment.MachineName, ref UUID);
                ValidateXSDOut = Conversions.ToBoolean(TripXMLMain.AppState.Get(sb.Append("XSD").Append(ttCredential.UserID).Append("Out").ToString()));
                sb.Remove(0, sb.Length);

                switch (ttCredential.Providers[0].Name ?? "")
                {
                    case "Amadeus":
                        {
                            break;
                        }
                    // Dim ttAA As AmadeusAPIAdapter

                    // ttAA = TripXMLMain.AppState.Get(sb.Append("API").Append(ttCredential.UserID).Append(ttCredential.System).Append(ttCredential.Providers(0).PCC).ToString())
                    // sb.Remove(0, sb.Length())
                    // If ttAA Is Nothing Then
                    // Throw New Exception(sb.Append("Access denied to Amadeus - ").Append(ttCredential.System).Append(" system. Or invalid provider.").ToString())
                    // sb.Remove(0, sb.Length())
                    // End If

                    // If ttCredential.Providers(0).PCC.Trim.Length > 0 Then
                    // ttAA.SourcePCC = ttCredential.Providers(0).PCC
                    // End If

                    // strResponse = SendPNRRequestAmadeus(ttServiceID, ttCredential, ttAA, strRequest)
                    // TripXMLMain.AppState.Set(sb.Append("API").Append(ttCredential.UserID).Append(ttCredential.System).ToString(), ttAA)
                    // sb.Remove(0, sb.Length())

                    case "AmadeusWS":
                        {

                            strResponse = modMain.SendPNRRequestAmadeusWS(ttServiceID, ref ttCredential, ref ttProviderSystems, ref strRequest);
                            break;
                        }

                    case "Apollo":
                    case "Galileo":
                        {

                            strResponse = modMain.SendPNRRequestGalileo(ttServiceID, ref ttCredential, ref ttProviderSystems, ref strRequest);
                            break;
                        }

                    case "Sabre":
                        {

                            // ttProviderSystems = TripXMLMain.AppState.Get(sb.Append("PS").Append(ttCredential.Providers(0).Name).Append(ttCredential.UserID).Append(ttCredential.System).Append(ttCredential.Providers(0).PCC).ToString())
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

                    case "Worldspan":
                        {

                            strResponse = modMain.SendPNRRequestWorldspan(ttServiceID, ref ttCredential, ref ttProviderSystems, ref strRequest);
                            break;
                        }

                    default:
                        {
                            throw new Exception(sb.Append("Provider ").Append(ttCredential.Providers[0].Name).Append(" Not Currently Supported.").ToString());
                        }
                }

                strResponse = DecodePNRRead(strResponse, ttCredential.UserID, ref UUID);

                modMain.PostServiceRequest(ref strResponse, ValidateXSDOut, (int)ttServiceID, ttCredential.UserID);
            }

            catch (Exception ex)
            {
                strResponse = FormatErrorMessage(ttServiceID, ex.Message, ttCredential.Providers[0].Name);
            }
            finally
            {
                _modMain.LogResponse(ref strResponse, ref ttCredential, StartTime, (int)ttServiceID, Environment.MachineName, ref UUID);
                if (modCore.Trace)
                    CoreLib.SendTrace(ttCredential.UserID, "wsPNRRead", "============= OTA Response ============= ", strResponse, UUID);
            }

            return strResponse;
            sb = null;
        }

        #endregion

        #region  Web Methods 
        public wmTravelItineraryOut.OTA_TravelItineraryRS wmPNRRead(wmPNRReadIn.OTA_ReadRQ OTA_ReadRQ)
        {
            string xmlMessage = "";
            wmTravelItineraryOut.OTA_TravelItineraryRS oPNRReadRS = null;
            XmlSerializer oSerializer = null;
            System.IO.StringWriter oWriter = null;
            System.IO.StringReader oReader = null;

            oSerializer = new XmlSerializer(typeof(wmPNRReadIn.OTA_ReadRQ));
            oWriter = new System.IO.StringWriter(new StringBuilder());
            oSerializer.Serialize(oWriter, OTA_ReadRQ);
            xmlMessage = oWriter.ToString();
            xmlMessage = xmlMessage.Replace(" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", "");

            xmlMessage = ServiceRequest(xmlMessage, ttServices.PNRRead);

            try
            {
                oSerializer = null;
                oSerializer = new XmlSerializer(@type: typeof(wmTravelItineraryOut.OTA_TravelItineraryRS));
                oReader = new System.IO.StringReader(xmlMessage);
                oPNRReadRS = (wmTravelItineraryOut.OTA_TravelItineraryRS)oSerializer.Deserialize(oReader);
            }
            catch (Exception ex)
            {
                CoreLib.SendTrace("", "wsPNRRead", "Error Deserialing OTA Response", ex.InnerException.ToString(), string.Empty);
                xmlMessage = "<OTA_TravelItineraryRS Version=\"2.000\"><Errors><Error>" + ex.InnerException.ToString() + "</Error></Errors></OTA_TravelItineraryRS>";
                oReader = new System.IO.StringReader(xmlMessage.Replace("&", "&amp;"));
                oPNRReadRS = (wmTravelItineraryOut.OTA_TravelItineraryRS)oSerializer.Deserialize(oReader);
            }

            return oPNRReadRS;

        }
        public string wmPNRReadXml(string xmlRequest)
        {
            return ServiceRequest(xmlRequest, ttServices.PNRRead);
        }

        #endregion

    }

}