using System;
using System.Diagnostics;
using System.Text;
using System.Xml.Serialization;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using TripXMLMain;
using static TripXMLMain.modCore;

namespace wsTripXML.wsTravelTalk
{
    public partial class wsSessionCreate
    {
        public TripXML tXML;

        private readonly modMain _modMain;

        public wsSessionCreate(modMain modMain)
        {
            _modMain = modMain;
        }

        #region  Decode Function 

        // Not Implemented

        #endregion

        #region  Process Service Request All GDS 
        private StringBuilder sb = new StringBuilder();

        private string ServiceRequest(string request, ttServices ttServiceID)
        {
            string response = string.Empty;
            TravelTalkCredential ttCredential = default;
            TripXMLProviderSystems ttProviderSystems = default;
            bool validateXSDOut;
            var startTime = default(DateTime);
            string uuid = string.Empty;

            try
            {
                startTime = DateTime.Now;
                _modMain.PreServiceRequest(ref request, ref ttCredential, ref ttProviderSystems, startTime, (int)ttServiceID, Environment.MachineName, ref uuid);
                validateXSDOut = Conversions.ToBoolean(TripXMLMain.AppState.Get(sb.Append("XSD").Append(ttCredential.UserID).Append("Out").ToString()));
                sb.Remove(0, sb.Length);
                ttProviderSystems.LogUUID = uuid;

                switch (ttCredential.Providers[0].Name ?? "")
                {
                    case "AmadeusWS":
                        {

                            response = modMain.SendOtherRequestAmadeusWS(ttServiceID, ref ttCredential, ref ttProviderSystems, ref request);
                            break;
                        }

                    case "Apollo":
                    case "Galileo":
                        {

                            response = modMain.SendOtherRequestGalileo(ttServiceID, ref ttCredential, ref ttProviderSystems, ref request);
                            break;
                        }

                    case "Sabre":
                        {

                            sb.Remove(0, sb.Length);
                            if (ttProviderSystems.System is null)
                            {
                                FormatErrorMessage(ttServiceID, sb.Append("Access denied to ").Append(ttCredential.Providers[0].Name).Append(" - ").Append(ttCredential.System).Append(" system. Or invalid provider.").ToString(), ttCredential.Providers[0].Name);
                                sb.Remove(0, sb.Length);
                                break;
                            }

                            response = modMain.SendOtherRequestSabre(ttServiceID, ref ttCredential, ref ttProviderSystems, ref request);
                            break;
                        }

                    case "Worldspan":
                        {

                            response = modMain.SendOtherRequestWorldspan(ttServiceID, ref ttCredential, ref ttProviderSystems, ref request);
                            break;
                        }

                    case "Travelport":
                        {

                            response = modMain.SendOtherRequestTravelport(ttServiceID, ref ttCredential, ref ttProviderSystems, ref request);
                            break;
                        }

                    default:
                        {
                            throw new Exception(sb.Append("Provider ").Append(ttCredential.Providers[0].Name).Append(" Not Currently Supported.").ToString());
                        }
                }

                // DecodeSessionCreate(strResponse) Not Implemented.

                modMain.PostServiceRequest(ref response, validateXSDOut, (int)ttServiceID, ttCredential.UserID);
            }

            catch (Exception ex)
            {
                string msgError = $"{ex.Message}";
                if (ex.InnerException is not null)
                {
                    msgError += $"{Constants.vbNewLine}{ex.InnerException.Message}";
                    if (ex.InnerException.InnerException is not null)
                    {
                        msgError += $"{Constants.vbNewLine}{ex.InnerException.InnerException.Message}";
                        if (ex.InnerException.InnerException.InnerException is not null)
                        {
                            msgError += $"{Constants.vbNewLine}{ex.InnerException.InnerException.InnerException.Message}";
                        }
                    }
                }
                response = FormatErrorMessage(ttServiceID, msgError, ttProviderSystems);
            }
            finally
            {
                _modMain.LogResponse(ref response, ref ttCredential, startTime, (int)ttServiceID, Environment.MachineName, ref uuid);
                if (modCore.Trace)
                    CoreLib.SendTrace(ttCredential.UserID, "wsSessionCreate", "============= OTA Response ============= ", response, ttProviderSystems.LogUUID);
            }
            sb = null;
            return response;

        }

        #endregion

        #region  Web Methods 
        public wmSessionCreateOut.SessionCreateRS wmSessionCreate(wmSessionCreateIn.SessionCreateRQ SessionCreateRQ)
        {
            string xmlMessage;
            wmSessionCreateOut.SessionCreateRS oSessionCreateRS = null;
            XmlSerializer oSerializer;
            System.IO.StringWriter oWriter;
            System.IO.StringReader oReader;

            oSerializer = new XmlSerializer(typeof(wmSessionCreateIn.SessionCreateRQ));
            oWriter = new System.IO.StringWriter(new StringBuilder());
            oSerializer.Serialize(oWriter, SessionCreateRQ);
            xmlMessage = oWriter.ToString();
            xmlMessage = xmlMessage.Replace(" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", "");

            xmlMessage = ServiceRequest(xmlMessage, ttServices.CreateSession);

            try
            {
                oSerializer = new XmlSerializer(@type: typeof(wmSessionCreateOut.SessionCreateRS));
                oReader = new System.IO.StringReader(xmlMessage);
                oSessionCreateRS = (wmSessionCreateOut.SessionCreateRS)oSerializer.Deserialize(oReader);
            }
            catch (Exception ex)
            {
                CoreLib.SendTrace("", "wsSessionCreate", "Error Deserialing OTA Response", ex.Message, string.Empty);
            }

            return oSessionCreateRS;

        }
        public string wmSessionCreateXml(string xmlRequest)
        {
            return ServiceRequest(xmlRequest, ttServices.CreateSession);
        }

        #endregion

    }

}