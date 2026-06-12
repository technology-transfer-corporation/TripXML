using System;
using System.Data;
using System.Diagnostics;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.VisualBasic.CompilerServices;
using TripXMLMain;
using static TripXMLMain.modCore;

namespace wsTripXML.wsTravelTalk
{
    public partial class wsProfileRead
    {
        public TripXML tXML;

        private readonly modMain _modMain;

        public wsProfileRead(modMain modMain)
        {
            _modMain = modMain;
        }

        #region  Decode Function 

        private string DecodeProfileRead(string strResponse, string UserID)
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
                            string argstrCode = oNode.SelectSingleNode("DepartureAirport").Attributes["LocationCode"].Value;
                            oNode.SelectSingleNode("DepartureAirport").InnerText = modMain.GetDecodeValue(ref ttAirports, ref argstrCode);
                            oNode.SelectSingleNode("DepartureAirport").Attributes["LocationCode"].Value = argstrCode;
                        }
                        if (oNode.SelectSingleNode("ArrivalAirport") is not null)
                        {
                            string argstrCode1 = oNode.SelectSingleNode("ArrivalAirport").Attributes["LocationCode"].Value;
                            oNode.SelectSingleNode("ArrivalAirport").InnerText = modMain.GetDecodeValue(ref ttAirports, ref argstrCode1);
                            oNode.SelectSingleNode("ArrivalAirport").Attributes["LocationCode"].Value = argstrCode1;
                        }

                        // *******************
                        // Decode Airlines   *
                        // *******************
                        if (oNode.SelectSingleNode("OperatingAirline") is not null)
                        {
                            if (oNode.SelectSingleNode("OperatingAirline").Attributes["Code"] is not null)
                            {
                                string argstrCode2 = oNode.SelectSingleNode("OperatingAirline").Attributes["Code"].Value;
                                oNode.SelectSingleNode("OperatingAirline").InnerText = modMain.GetDecodeValue(ref ttAirlines, ref argstrCode2);
                                oNode.SelectSingleNode("OperatingAirline").Attributes["Code"].Value = argstrCode2;
                            }
                        }
                        if (oNode.SelectSingleNode("MarketingAirline") is not null)
                        {
                            string argstrCode3 = oNode.SelectSingleNode("MarketingAirline").Attributes["Code"].Value;
                            oNode.SelectSingleNode("MarketingAirline").InnerText = modMain.GetDecodeValue(ref ttAirlines, ref argstrCode3);
                            oNode.SelectSingleNode("MarketingAirline").Attributes["Code"].Value = argstrCode3;
                        }

                        // *******************
                        // Decode Equipments   *
                        // *******************
                        if (oNode.SelectSingleNode("Equipment") is not null)
                        {
                            if (oNode.SelectSingleNode("Equipment").Attributes["AirEquipType"] is not null)
                            {
                                string argstrCode4 = oNode.SelectSingleNode("Equipment").Attributes["AirEquipType"].Value;
                                oNode.SelectSingleNode("Equipment").InnerText = modMain.GetDecodeValue(ref ttEquipments, ref argstrCode4);
                                oNode.SelectSingleNode("Equipment").Attributes["AirEquipType"].Value = argstrCode4;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        CoreLib.SendTrace(UserID, "wsProfileRead", "Error *** Decoding AirAvail Response", e.Message, string.Empty);
                    }

                }

                strResponse = oDoc.OuterXml;
            }

            catch (Exception ex)
            {
                CoreLib.SendTrace(UserID, "wsProfileRead", "Error *** Decoding AirAvail Response", ex.Message, string.Empty);
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

                            strResponse = modMain.SendOtherRequestGalileo(ttServiceID, ref ttCredential, ref ttProviderSystems, ref strRequest);
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
                            sb.Remove(0, sb.Length);
                            break;
                        }
                }

                strResponse = DecodeProfileRead(strResponse, ttCredential.UserID);

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
                    CoreLib.SendTrace(ttCredential.UserID, "wsProfileRead", "============= OTA Response ============= ", strResponse, UUID);
            }

            return strResponse;
            sb = null;
        }

        #endregion

        #region  Web Methods 
        public wmProfileReadOut.OTA_ProfileReadRS wmProfileRead(wmProfileReadIn.OTA_ProfileReadRQ OTA_ProfileReadRQ)
        {
            string xmlMessage = "";
            wmProfileReadOut.OTA_ProfileReadRS oProfileReadRS = null;
            XmlSerializer oSerializer = null;
            System.IO.StringWriter oWriter = null;
            System.IO.StringReader oReader = null;

            oSerializer = new XmlSerializer(typeof(wmProfileReadIn.OTA_ProfileReadRQ));
            oWriter = new System.IO.StringWriter(new StringBuilder());
            oSerializer.Serialize(oWriter, OTA_ProfileReadRQ);
            xmlMessage = oWriter.ToString();
            xmlMessage = xmlMessage.Replace(" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", "");

            xmlMessage = ServiceRequest(xmlMessage, ttServices.ProfileRead);

            try
            {
                oSerializer = null;
                oSerializer = new XmlSerializer(@type: typeof(wmProfileReadOut.OTA_ProfileReadRS));
                oReader = new System.IO.StringReader(xmlMessage);
                oProfileReadRS = (wmProfileReadOut.OTA_ProfileReadRS)oSerializer.Deserialize(oReader);
            }
            catch (Exception ex)
            {
                CoreLib.SendTrace("", "wsProfileRead", "Error Deserialing OTA Response", ex.Message, string.Empty);
            }

            return oProfileReadRS;

        }
        public string wmProfileReadXml(string xmlRequest)
        {
            return ServiceRequest(xmlRequest, ttServices.ProfileRead);
        }

        #endregion

    }

}