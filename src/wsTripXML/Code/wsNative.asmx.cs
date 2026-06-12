using System;
using System.Diagnostics;
using System.Text;
using System.Xml.Serialization;
using Microsoft.VisualBasic.CompilerServices;
using TripXMLMain;
using static TripXMLMain.modCore;

namespace wsTripXML.wsTravelTalk
{
    public partial class wsNative
    {
        public TripXML tXML;

        private readonly modMain _modMain;

        public wsNative(modMain modMain)
        {
            _modMain = modMain;
        }

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
                _modMain.PreServiceRequest(ref strRequest, ref ttCredential, ref ttProviderSystems, StartTime, (int)ttServiceID, Environment.MachineName, ref UUID);
                ValidateXSDOut = Conversions.ToBoolean(TripXMLMain.AppState.Get(sb.Append("XSD").Append(ttCredential.UserID).Append("Out").ToString()));
                sb.Remove(0, sb.Length);

                // strRequest = strRequest.Replace("&amp;", "&")
                strRequest = strRequest.Replace("&lt;", "<").Replace("&gt;", ">");

                switch (ttCredential.Providers[0].Name.ToLower() ?? "")
                {
                    case "amadeusws":
                        {
                            strResponse = modMain.SendOtherRequestAmadeusWS(ttServiceID, ref ttCredential, ref ttProviderSystems, ref strRequest);
                            break;
                        }
                    case "apollo":
                    case "galileo":
                        {
                            strResponse = modMain.SendOtherRequestGalileo(ttServiceID, ref ttCredential, ref ttProviderSystems, ref strRequest);
                            break;
                        }
                    case "sabre":
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

                            strResponse = modMain.SendOtherRequestSabre(ttServiceID, ref ttCredential, ref ttProviderSystems, ref strRequest);
                            break;
                        }

                    case "worldspan":
                        {
                            strResponse = modMain.SendOtherRequestWorldspan(ttServiceID, ref ttCredential, ref ttProviderSystems, ref strRequest);
                            break;
                        }
                    case "travelport":
                        {
                            strResponse = modMain.SendOtherRequestTravelport(ttServiceID, ref ttCredential, ref ttProviderSystems, ref strRequest);
                            break;
                        }

                    // strResponse = SendOtherRequestSITA(ttServiceID, ttCredential, ttProviderSystems, strRequest)

                    // Case "airnz"

                    // strResponse = SendOtherRequestAirNZ(ttServiceID, ttCredential, ttProviderSystems, strRequest)

                    case "sita":
                        {
                            break;
                        }

                    default:
                        {
                            throw new Exception(sb.Append("Provider ").Append(ttCredential.Providers[0].Name).Append(" Not Currently Supported.").ToString());
                        }
                }

                // DecodeNative(strResponse) Not Implemented.

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
                    CoreLib.SendTrace(ttCredential.UserID, "wsNative", "============= OTA Response ============= ", strResponse, UUID);
            }
            sb = null;
            return strResponse;

        }

        #endregion

        #region  Web Methods 
        public wmNativeOut.NativeRS wmNative(wmNativeIn.NativeRQ NativeRQ)
        {
            string xmlMessage = "";
            wmNativeOut.NativeRS oNativeRS = null;
            XmlSerializer oSerializer = null;
            System.IO.StringWriter oWriter = null;
            System.IO.StringReader oReader = null;

            NativeRQ.Native = NativeRQ.Native.Replace("<", "&lt;").Replace(">", "&gt;");

            oSerializer = new XmlSerializer(typeof(wmNativeIn.NativeRQ));
            oWriter = new System.IO.StringWriter(new StringBuilder());
            oSerializer.Serialize(oWriter, NativeRQ);
            xmlMessage = oWriter.ToString();
            xmlMessage = xmlMessage.Replace(" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", "");
            xmlMessage = xmlMessage.Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", "");
            xmlMessage = xmlMessage.Replace("&amp;lt;", "&lt;").Replace("&amp;gt;", "&gt;");

            xmlMessage = ServiceRequest(xmlMessage, ttServices.Native);

            try
            {
                oSerializer = null;
                oSerializer = new XmlSerializer(@type: typeof(wmNativeOut.NativeRS));
                oReader = new System.IO.StringReader(xmlMessage);
                oNativeRS = (wmNativeOut.NativeRS)oSerializer.Deserialize(oReader);
            }
            catch (Exception ex)
            {
                CoreLib.SendTrace("", "wsNative", "Error Deserialing OTA Response", ex.Message, string.Empty);
            }

            return oNativeRS;

        }
        public wmNativeOut.NativeRS wmTripXMLNative(wmNativeIn.NativeRQ request)
        {
            wmNativeOut.NativeRS oNativeRS = null;
            XmlSerializer oSerializer;
            System.IO.StringWriter oWriter;
            System.IO.StringReader oReader;

            oSerializer = new XmlSerializer(typeof(wmNativeIn.NativeRQ));
            oWriter = new System.IO.StringWriter(new StringBuilder());
            oSerializer.Serialize(oWriter, request);
            string xmlMessage = oWriter.ToString();
            xmlMessage = xmlMessage.Replace(" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", "");

            xmlMessage = ServiceRequest(xmlMessage, ttServices.TripXMLNative);

            try
            {
                oSerializer = null;
                oSerializer = new XmlSerializer(@type: typeof(wmNativeOut.NativeRS));
                oReader = new System.IO.StringReader(xmlMessage);
                oNativeRS = (wmNativeOut.NativeRS)oSerializer.Deserialize(oReader);
            }
            catch (Exception ex)
            {
                CoreLib.SendTrace("", "wsNative", "Error Deserialing OTA Response", ex.Message, string.Empty);
            }

            return oNativeRS;

        }
        public string wmNativeXml(string xmlRequest)
        {
            return ServiceRequest(xmlRequest, ttServices.Native);
        }

        #endregion

    }

}