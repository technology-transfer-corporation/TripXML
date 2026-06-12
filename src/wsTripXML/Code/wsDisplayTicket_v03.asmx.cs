using System;
using System.Diagnostics;
using System.Text;
using System.Xml.Serialization;
using Microsoft.VisualBasic.CompilerServices;
using TripXMLMain;
using static TripXMLMain.modCore;

namespace wsTripXML.wsTravelTalk
{
    public partial class wsDisplayTicket_v03
    {
        public TripXML tXML;

        private readonly modMain _modMain;

        public wsDisplayTicket_v03(modMain modMain)
        {
            _modMain = modMain;
        }

        #region  Decode Functions 

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

                switch (ttCredential.Providers[0].Name.ToLower() ?? "")
                {

                    case "amadeusws":
                        {

                            strResponse = modMain.SendTravelRequestAmadeusWS(ttServiceID, ref ttCredential, ref ttProviderSystems, ref strRequest, "v03");
                            break;
                        }

                    default:
                        {
                            throw new Exception(sb.Append("Provider ").Append(ttCredential.Providers[0].Name).Append(" Not Currently Supported.").ToString());
                            sb.Remove(0, sb.Length);
                            break;
                        }
                }

                // Dim StartCounter As Date
                // StartCounter = Now
                // Not Implemented DecodeIssueTicket(strResponse, ttCredential.UserID)
                // CoreLib.SendTrace(ttCredential.UserID, "Performance", "Decoding = ").Append(CType(Now.Subtract(StartCounter).TotalMilliseconds, Integer), "")

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
                    CoreLib.SendTrace(ttCredential.UserID, "wsIssueTicket", "============= OTA Response ============= ", strResponse, ttProviderSystems.LogUUID);
            }

            return strResponse;
            sb = null;
        }

        #endregion

        #region  Web Methods 
        public wmDisplayTicketOut_v03.TT_DisplayTicketRS wmDisplayTicket(wmDisplayTicketIn.TT_DisplayTicketRQ TT_DisplayTicketRQ)
        {
            string xmlMessage = "";
            wmDisplayTicketOut_v03.TT_DisplayTicketRS oDisplayTicketRS = null;
            XmlSerializer oSerializer = null;
            System.IO.StringWriter oWriter = null;
            System.IO.StringReader oReader = null;

            oSerializer = new XmlSerializer(typeof(wmDisplayTicketIn.TT_DisplayTicketRQ));
            oWriter = new System.IO.StringWriter(new StringBuilder());
            oSerializer.Serialize(oWriter, TT_DisplayTicketRQ);
            xmlMessage = oWriter.ToString();
            xmlMessage = xmlMessage.Replace(" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", "");

            xmlMessage = ServiceRequest(xmlMessage, ttServices.TicketDisplay);

            try
            {
                oSerializer = null;
                oSerializer = new XmlSerializer(@type: typeof(wmDisplayTicketOut_v03.TT_DisplayTicketRS));
                oReader = new System.IO.StringReader(xmlMessage);
                oDisplayTicketRS = (wmDisplayTicketOut_v03.TT_DisplayTicketRS)oSerializer.Deserialize(oReader);
            }
            catch (Exception ex)
            {
                CoreLib.SendTrace("", "wsDisplayTicket", "Error Deserialing OTA Response", ex.Message, string.Empty);
                xmlMessage = "<TT_DisplayTicketRS Version=\"v03\"><Errors><Error>" + ex.InnerException.ToString() + "</Error></Errors></TT_DisplayTicketRS>";
                oReader = new System.IO.StringReader(xmlMessage);
                oDisplayTicketRS = (wmDisplayTicketOut_v03.TT_DisplayTicketRS)oSerializer.Deserialize(oReader);
            }

            return oDisplayTicketRS;

        }
        public string wmDisplayTicketXml(string xmlRequest)
        {
            return ServiceRequest(xmlRequest, ttServices.TicketDisplay);
        }

        #endregion

    }

}