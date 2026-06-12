using System;
using System.Diagnostics;
using System.Text;
using System.Xml.Serialization;
using Microsoft.VisualBasic.CompilerServices;
using TripXMLMain;
using static TripXMLMain.modCore;
using wsTripXML.wsTravelTalk.wmAuthorizationIn;
using wsTripXML.wsTravelTalk.wmAuthorizationOut;

namespace wsTripXML.wsTravelTalk
{
    public partial class wsAuthorization
    {
        public TripXML tXML;


        private readonly modMain _modMain;

        public wsAuthorization(modMain modMain)
        {
            _modMain = modMain;
        }


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
                            strResponse = modMain.SendTravelRequestAmadeusWS(ttServiceID, ref ttCredential, ref ttProviderSystems, ref strRequest);
                            break;
                        }

                    case "worldspan":
                        {
                            strResponse = modMain.SendTravelRequestWorldspan(ttServiceID, ref ttCredential, ref ttProviderSystems, ref strRequest);
                            break;
                        }

                    default:
                        {
                            throw new Exception(sb.Append("Provider ").Append(ttCredential.Providers[0].Name).Append(" Not Currently Supported.").ToString());
                        }
                }

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
                    CoreLib.SendTrace(ttCredential.UserID, "wsAuthorization", "============= OTA Response ============= ", strResponse, UUID);
            }

            return strResponse;
        }

        #endregion

        #region  Web Methods 
        public OTA_AuthorizationRS wmAuthorization(OTA_AuthorizationRQ OTA_AuthorizationRQ)
        {
            string xmlMessage = "";
            OTA_AuthorizationRS oAuthorizationRS = null;
            XmlSerializer oSerializer;
            System.IO.StringWriter oWriter;
            System.IO.StringReader oReader;

            oSerializer = new XmlSerializer(typeof(OTA_AuthorizationRQ));
            oWriter = new System.IO.StringWriter(new StringBuilder());
            oSerializer.Serialize(oWriter, OTA_AuthorizationRQ);
            xmlMessage = oWriter.ToString();
            xmlMessage = xmlMessage.Replace(" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", "");
            xmlMessage = xmlMessage.Replace(" xmlns=\"http://www.opentravel.org/OTA/2003/05\"", "");

            xmlMessage = ServiceRequest(xmlMessage, ttServices.Authorization);

            try
            {
                oSerializer = new XmlSerializer(@type: typeof(OTA_AuthorizationRS));
                oReader = new System.IO.StringReader(xmlMessage);
                oAuthorizationRS = (OTA_AuthorizationRS)oSerializer.Deserialize(oReader);
                oAuthorizationRS.Authorization.AuthorizationDetail.BookingReferenceID = new BookingReferenceID();
                oAuthorizationRS.Authorization.AuthorizationDetail.BookingReferenceID.ID = OTA_AuthorizationRQ.AuthorizationDetail.BookingReferenceID.ID;
                oAuthorizationRS.Authorization.AuthorizationDetail.BookingReferenceID.Type = OTA_AuthorizationRQ.AuthorizationDetail.BookingReferenceID.Type;

                if (!string.IsNullOrEmpty(oAuthorizationRS.Authorization.AuthorizationResult.AuthorizationCode))
                {
                    oAuthorizationRS.Authorization.AuthorizationResult.Result = Detail.Approved;
                }
            }

            catch (Exception ex)
            {
                CoreLib.SendTrace("", "wsAuthorization", "Error Deserialing OTA Response", ex.Message, string.Empty);
            }

            return oAuthorizationRS;

        }
        public string wmAuthorizationXml(string xmlRequest)
        {
            return ServiceRequest(xmlRequest, ttServices.Authorization);
        }

        #endregion

    }
}