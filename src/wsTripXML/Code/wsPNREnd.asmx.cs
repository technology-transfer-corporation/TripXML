using System;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.VisualBasic.CompilerServices;
using TripXMLMain;
using static TripXMLMain.modCore;
using static TripXMLTools.TripXMLLoad;

namespace wsTripXML.wsTravelTalk
{
    public partial class wsPNREnd
    {
        public TripXML tXML;

        private readonly modMain _modMain;

        public wsPNREnd(modMain modMain)
        {
            _modMain = modMain;
        }

        #region  Decode Function 

        private string DecodePNREnd(string strResponse, string UserID)
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
                        }
                        if (oNode.SelectSingleNode("ArrivalAirport") is not null)
                        {
                            oNode.SelectSingleNode("ArrivalAirport").InnerText = DecodeValue(DecodingType.Airport, oNode.SelectSingleNode("ArrivalAirport").Attributes["LocationCode"].Value);
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
                            // oNode.SelectSingleNode("OperatingAirline").InnerText = DecodeValue(DecodingType.Airline, oNode.SelectSingleNode("OperatingAirline").Attributes("Code").Value)
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
                                else
                                {
                                    attCode.Value = EncodeValue(DecodingType.Airline, oNode.SelectSingleNode("OperatingAirline").InnerText);

                                    if (!string.IsNullOrEmpty(attCode.Value))
                                    {
                                        oNode.SelectSingleNode("OperatingAirline").Attributes.Append(attCode);
                                        oNode.SelectSingleNode("OperatingAirline").InnerText = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(oNode.SelectSingleNode("OperatingAirline").InnerText.ToLower());
                                    }
                                }
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
                    catch (Exception e)
                    {
                        CoreLib.SendTrace(UserID, "wsPNREnd", "Error *** Decoding AirAvail Response", e.Message, string.Empty);
                    }

                }

                strResponse = oDoc.OuterXml;
            }

            catch (Exception ex)
            {
                CoreLib.SendTrace(UserID, "wsPNREnd", "Error *** Decoding AirAvail Response", ex.Message, string.Empty);
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
            var StartTime = default(DateTime);
            string UUID = "";

            try
            {
                StartTime = DateTime.Now;
                strRequest = strRequest.Replace(" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns=\"http://tripxml.downtowntravel.com/tripxml/wsQueue\"", "");
                _modMain.PreServiceRequest(ref strRequest, ref ttCredential, ref ttProviderSystems, StartTime, (int)ttServiceID, Environment.MachineName, ref UUID);
                validateXSDOut = Conversions.ToBoolean(TripXMLMain.AppState.Get(sb.Append("XSD").Append(ttCredential.UserID).Append("Out").ToString()));
                sb.Remove(0, sb.Length);

                switch (ttCredential.Providers[0].Name ?? "")
                {
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

                strResponse = DecodePNREnd(strResponse, ttCredential.UserID);

                modMain.PostServiceRequest(ref strResponse, validateXSDOut, (int)ttServiceID, ttCredential.UserID);
            }

            catch (Exception ex)
            {
                strResponse = FormatErrorMessage(ttServiceID, ex.Message, ttCredential.Providers[0].Name);
            }
            finally
            {
                _modMain.LogResponse(ref strResponse, ref ttCredential, StartTime, (int)ttServiceID, Environment.MachineName, ref UUID);
                if (modCore.Trace)
                    CoreLib.SendTrace(ttCredential.UserID, "wsPNREnd", "============= OTA Response ============= ", strResponse, UUID);
            }

            return strResponse;
        }

        #endregion

        #region  Web Methods 
        public wmTravelItineraryOut_v03.OTA_TravelItineraryRS wmPNREnd(wmPNREndIn.OTA_PNREndRQ OTA_PNREndRQ)
        {
            string xmlMessage;
            wmTravelItineraryOut_v03.OTA_TravelItineraryRS oPNREndRS = null;
            XmlSerializer oSerializer;
            System.IO.StringWriter oWriter;
            System.IO.StringReader oReader;

            oSerializer = new XmlSerializer(typeof(wmPNREndIn.OTA_PNREndRQ));
            oWriter = new System.IO.StringWriter(new StringBuilder());
            oSerializer.Serialize(oWriter, OTA_PNREndRQ);
            xmlMessage = oWriter.ToString();
            xmlMessage = xmlMessage.Replace(" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", "");

            xmlMessage = ServiceRequest(xmlMessage, ttServices.PNREnd);

            try
            {
                oSerializer = new XmlSerializer(@type: typeof(wmTravelItineraryOut_v03.OTA_TravelItineraryRS));
                oReader = new System.IO.StringReader(xmlMessage);
                oPNREndRS = (wmTravelItineraryOut_v03.OTA_TravelItineraryRS)oSerializer.Deserialize(oReader);
            }
            catch (Exception ex)
            {
                CoreLib.SendTrace("", "wsPNREnd", "Error Deserialing OTA Response", ex.Message, string.Empty);
            }

            return oPNREndRS;

        }
        public string wmPNREndXml(string xmlRequest)
        {
            return ServiceRequest(xmlRequest, ttServices.PNREnd);
        }

        #endregion

    }

}