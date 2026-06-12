using System;
using System.Diagnostics;
using System.Text;
using System.Xml.Serialization;
using Microsoft.VisualBasic.CompilerServices;
using TripXMLMain;
using static TripXMLMain.modCore;

namespace wsTripXML.wsTravelTalk
{
    public partial class wsSessionClose
    {
        public TripXML tXML;

        private readonly modMain _modMain;

        public wsSessionClose(modMain modMain)
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
                ttProviderSystems.LogUUID = UUID;

                switch (ttCredential.Providers[0].Name ?? "")
                {

                    case "AmadeusWS":
                        {

                            strResponse = modMain.SendOtherRequestAmadeusWS(ttServiceID, ref ttCredential, ref ttProviderSystems, ref strRequest);
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
                            // ttProviderSystems.LogUUID = UUID
                            sb.Remove(0, sb.Length);
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

                    case "Worldspan":
                        {

                            if (strRequest.Contains("<ConversationID></ConversationID>") || strRequest.Contains("<ConversationID>NONE</ConversationID>"))
                            {
                                CoreLib.SendTrace("", "wsSessionClose", "NO SessionID Passed", strRequest, string.Empty);
                                string argmessage = "NO SessionID Passed";
                                AddLog(LogType.Info, ref argmessage, new TripXMLProviderSystems(), strRequest);
                                throw new Exception(sb.Append("Provider ").Append(ttCredential.Providers[0].Name).Append("No Session ID Provided.").ToString());
                            }

                            strResponse = modMain.SendOtherRequestWorldspan(ttServiceID, ref ttCredential, ref ttProviderSystems, ref strRequest);
                            break;
                        }
                    case "Travelport":
                        {

                            strResponse = modMain.SendOtherRequestTravelport(ttServiceID, ref ttCredential, ref ttProviderSystems, ref strRequest);
                            break;
                        }

                    default:
                        {
                            throw new Exception(sb.Append("Provider ").Append(ttCredential.Providers[0].Name).Append(" Not Currently Supported.").ToString());
                        }
                }

                // DecodeSessionClose(strResponse) Not Implemented.

                modMain.PostServiceRequest(ref strResponse, ValidateXSDOut, (int)ttServiceID, ttCredential.UserID);
            }

            catch (Exception ex)
            {
                strResponse = FormatErrorMessage(ttServiceID, ex.Message, ttProviderSystems);
            }
            finally
            {
                _modMain.LogResponse(ref strResponse, ref ttCredential, StartTime, (int)ttServiceID, Environment.MachineName, ref UUID);
                if (modCore.Trace)
                    CoreLib.SendTrace(ttCredential.UserID, "wsSessionClose", "============= OTA Response ============= ", strResponse, ttProviderSystems.LogUUID);
            }
            sb = null;
            return strResponse;

        }

        #endregion

        #region  Web Methods 
        public wmSessionCloseOut.SessionCloseRS wmSessionClose(wmSessionCloseIn.SessionCloseRQ SessionCloseRQ)
        {
            string xmlMessage;
            wmSessionCloseOut.SessionCloseRS oSessionCloseRS = null;
            XmlSerializer oSerializer;
            System.IO.StringWriter oWriter;
            System.IO.StringReader oReader;

            oSerializer = new XmlSerializer(typeof(wmSessionCloseIn.SessionCloseRQ));
            oWriter = new System.IO.StringWriter(new StringBuilder());
            oSerializer.Serialize(oWriter, SessionCloseRQ);
            xmlMessage = oWriter.ToString();
            xmlMessage = xmlMessage.Replace(" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", "");

            xmlMessage = ServiceRequest(xmlMessage, ttServices.CloseSession);

            try
            {
                oSerializer = new XmlSerializer(@type: typeof(wmSessionCloseOut.SessionCloseRS));
                oReader = new System.IO.StringReader(xmlMessage);
                oSessionCloseRS = (wmSessionCloseOut.SessionCloseRS)oSerializer.Deserialize(oReader);
            }
            catch (Exception ex)
            {
                CoreLib.SendTrace("", "wsSessionClose", "Error Deserialing OTA Response", ex.Message, string.Empty);
            }

            return oSessionCloseRS;

        }
        public string wmSessionCloseXml(string xmlRequest)
        {
            return ServiceRequest(xmlRequest, ttServices.CloseSession);
        }

        #endregion

    }

}