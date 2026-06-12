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
    public partial class wsCryptic
    {
        public TripXML tXML;

        private readonly modMain _modMain;

        public wsCryptic(modMain modMain)
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
                    case "Travelport":
                        {
                            strResponse = modMain.SendOtherRequestTravelport(ttServiceID, ref ttCredential, ref ttProviderSystems, ref strRequest);
                            break;
                        }
                    case "Worldspan":
                        {
                            strResponse = modMain.SendOtherRequestWorldspan(ttServiceID, ref ttCredential, ref ttProviderSystems, ref strRequest);
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

                            strResponse = modMain.SendOtherRequestSabre(ttServiceID, ref ttCredential, ref ttProviderSystems, ref strRequest);
                            break;
                        }

                    default:
                        {
                            throw new Exception(sb.Append("Provider ").Append(ttCredential.Providers[0].Name).Append(" Not Currently Supported.").ToString());
                        }
                }

                // DecodeCryptic(strResponse) Not Implemented.

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
                    CoreLib.SendTrace(ttCredential.UserID, "wsCryptic", "============= OTA Response ============= ", strResponse, UUID);
            }
            sb = null;
            return strResponse;

        }

        #endregion

        #region  Web Methods 
        public wmCrypticOut.CrypticRS wmCryptic(wmCrypticIn.CrypticRQ CrypticRQ)
        {
            string xmlMessage = "";
            wmCrypticOut.CrypticRS oCrypticRS = null;
            XmlSerializer oSerializer = null;
            System.IO.StringWriter oWriter = null;
            System.IO.StringReader oReader = null;

            oSerializer = new XmlSerializer(typeof(wmCrypticIn.CrypticRQ));
            oWriter = new System.IO.StringWriter(new StringBuilder());
            oSerializer.Serialize(oWriter, CrypticRQ);
            xmlMessage = oWriter.ToString();
            xmlMessage = xmlMessage.Replace(" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", "");

            xmlMessage = ServiceRequest(xmlMessage, ttServices.Cryptic);

            try
            {
                oSerializer = null;
                oSerializer = new XmlSerializer(@type: typeof(wmCrypticOut.CrypticRS));
                oReader = new System.IO.StringReader(xmlMessage);
                oCrypticRS = (wmCrypticOut.CrypticRS)oSerializer.Deserialize(oReader);
                // Adding Back the CR removed by the Serializer
                if (oCrypticRS.Response is not null)
                {
                    oCrypticRS.Response = oCrypticRS.Response.Replace(Constants.vbLf, Constants.vbNewLine).Replace("<", "&lt;").Replace(">", "&gt;");

                    if (oCrypticRS.Screen is not null)
                    {
                        foreach (var oLine in oCrypticRS.Screen)
                        {
                            if (oLine.Value is not null)
                            {
                                oLine.Value = oLine.Value.Replace(Constants.vbLf, Constants.vbNewLine).Replace("<", "&lt;").Replace(">", "&gt;");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                CoreLib.SendTrace("", "wsCryptic", "Error Deserialing OTA Response", ex.Message, string.Empty);
            }

            return oCrypticRS;

        }
        public string wmCrypticXml(string xmlRequest)
        {
            return ServiceRequest(xmlRequest, ttServices.Cryptic);
        }

        #endregion

    }

}