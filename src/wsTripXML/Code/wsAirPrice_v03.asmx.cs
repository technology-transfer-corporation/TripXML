using System;
using System.Diagnostics;
using System.Text;
using System.Xml.Serialization;
using Microsoft.VisualBasic.CompilerServices;
using TripXMLMain;
using static TripXMLMain.modCore;

namespace wsTripXML.wsTravelTalk
{
    public partial class wsAirPrice_v03
    {
        private StringBuilder sb = new StringBuilder();
        public TripXML tXML;

        private readonly modMain _modMain;

        public wsAirPrice_v03(modMain modMain)
        {
            _modMain = modMain;
        }

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

                    // strResponse = SendAirRequestAmadeus(ttServiceID, ttCredential, ttAA, strRequest)
                    // TripXMLMain.AppState.Set(sb.Append("API").Append(ttCredential.UserID).Append(ttCredential.System).ToString(), ttAA)
                    // sb.Remove(0, sb.Length())

                    case "AmadeusWS":
                        {

                            strResponse = modMain.SendAirRequestAmadeusWS(ttServiceID, ref ttCredential, ref ttProviderSystems, ref strRequest, "v03");
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

                            strResponse = modMain.SendAirRequestSabre(ttServiceID, ref ttCredential, ref ttProviderSystems, ref strRequest, "v03");
                            break;
                        }
                    case "Travelport":
                        {
                            strResponse = modMain.SendAirRequestTravelport(ttServiceID, ref ttCredential, ref ttProviderSystems, ref strRequest, "v03");
                            break;
                        }
                    case "Worldspan":
                        {
                            strResponse = modMain.SendAirRequestWorldspan(ttServiceID, ref ttCredential, ref ttProviderSystems, ref strRequest, "v03");
                            break;
                        }
                    case "Galileo":
                        {
                            strResponse = modMain.SendAirRequestGalileo(ttServiceID, ref ttCredential, ref ttProviderSystems, ref strRequest, "v03");
                            break;
                        }

                    default:
                        {
                            throw new Exception(sb.Append("Provider ").Append(ttCredential.Providers[0].Name).Append(" Not Currently Supported.").ToString());
                            sb.Remove(0, sb.Length);
                            break;
                        }
                }

                // DecodeAirPrice is not implemented

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
                    CoreLib.SendTrace(ttCredential.UserID, "wsAirPrice", "============= OTA Response ============= ", strResponse, UUID);
            }

            return strResponse;
            sb = null;
        }

        #endregion

        #region  Web Methods 
        public wmAirPriceOut.OTA_AirPriceRS wmAirPrice(wmAirPriceIn.OTA_AirPriceRQ OTA_AirPriceRQ)
        {
            string xmlMessage = "";
            wmAirPriceOut.OTA_AirPriceRS oAirPriceRS = null;
            XmlSerializer oSerializer = null;
            System.IO.StringWriter oWriter = null;
            System.IO.StringReader oReader = null;

            oSerializer = new XmlSerializer(typeof(wmAirPriceIn.OTA_AirPriceRQ));
            oWriter = new System.IO.StringWriter(new StringBuilder());
            oSerializer.Serialize(oWriter, OTA_AirPriceRQ);
            xmlMessage = oWriter.ToString();
            xmlMessage = xmlMessage.Replace(" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", "");

            xmlMessage = ServiceRequest(xmlMessage, ttServices.AirPrice);

            try
            {
                oSerializer = null;
                oSerializer = new XmlSerializer(@type: typeof(wmAirPriceOut.OTA_AirPriceRS));
                oReader = new System.IO.StringReader(xmlMessage);
                oAirPriceRS = (wmAirPriceOut.OTA_AirPriceRS)oSerializer.Deserialize(oReader);
            }
            catch (Exception ex)
            {
                CoreLib.SendTrace("", "wsAirPrice", "Error Deserialing OTA Response", ex.Message, string.Empty);
            }

            return oAirPriceRS;

        }
        public string wmAirPriceXml(string xmlRequest)
        {
            return ServiceRequest(xmlRequest, ttServices.AirPrice);
        }

        #endregion

    }

}