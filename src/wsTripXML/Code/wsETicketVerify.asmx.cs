using System;
using System.Diagnostics;
using System.Text;
using System.Xml.Serialization;
using Microsoft.VisualBasic.CompilerServices;
using TripXMLMain;
using static TripXMLMain.modCore;

namespace wsTripXML.wsTravelTalk
{
    public partial class wsETicketVerify
    {

        private readonly modMain _modMain;

        public wsETicketVerify(modMain modMain)
        {
            _modMain = modMain;
        }


        #region  Decode Functions 

        // Not Apply

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

                switch (ttCredential.Providers[0].Name ?? "")
                {
                    case "Apollo":
                    case "Galileo":
                        {

                            strResponse = modMain.SendOtherRequestGalileo(ttServiceID, ref ttCredential, ref ttProviderSystems, ref strRequest);
                            break;
                        }

                    default:
                        {
                            throw new Exception(sb.Append("Provider ").Append(ttCredential.Providers[0].Name).Append(" Not Currently Supported.").ToString());
                        }
                }

                // Not Apply DecodeETicketVerify(strResponse, ttCredential.UserID)

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
                    CoreLib.SendTrace(ttCredential.UserID, "wsETicketVerify", "============= OTA Response ============= ", strResponse, UUID);
            }
            sb = null;
            return strResponse;

        }

        #endregion

        #region  Web Methods 
        public wmETicketVerifyOut.OTA_ETicketVerifyRS wmETicketVerify(wmETicketVerifyIn.OTA_ETicketVerifyRQ OTA_ETicketVerifyRQ)
        {
            string xmlMessage = "";
            wmETicketVerifyOut.OTA_ETicketVerifyRS oETicketVerifyRS = null;
            XmlSerializer oSerializer = null;
            System.IO.StringWriter oWriter = null;
            System.IO.StringReader oReader = null;

            oSerializer = new XmlSerializer(typeof(wmETicketVerifyIn.OTA_ETicketVerifyRQ));
            oWriter = new System.IO.StringWriter(new StringBuilder());
            oSerializer.Serialize(oWriter, OTA_ETicketVerifyRQ);
            xmlMessage = oWriter.ToString();
            xmlMessage = xmlMessage.Replace(" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", "");

            xmlMessage = ServiceRequest(xmlMessage, ttServices.ETicketVerify);

            try
            {
                oSerializer = null;
                oSerializer = new XmlSerializer(@type: typeof(wmETicketVerifyOut.OTA_ETicketVerifyRS));
                oReader = new System.IO.StringReader(xmlMessage);
                oETicketVerifyRS = (wmETicketVerifyOut.OTA_ETicketVerifyRS)oSerializer.Deserialize(oReader);
            }
            catch (Exception ex)
            {
                CoreLib.SendTrace("", "wsETicketVerify", "Error Deserialing OTA Response", ex.Message, string.Empty);
            }

            return oETicketVerifyRS;

        }
        public string wmETicketVerifyXml(string xmlRequest)
        {
            return ServiceRequest(xmlRequest, ttServices.ETicketVerify);
        }

        #endregion

    }

}