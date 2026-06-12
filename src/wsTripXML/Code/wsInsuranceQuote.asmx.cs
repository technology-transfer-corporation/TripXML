using System;
using System.Diagnostics;
using System.Text;
using System.Xml.Serialization;
using Microsoft.VisualBasic.CompilerServices;
using TripXMLMain;
using static TripXMLMain.modCore;

namespace wsTripXML.wsTravelTalk
{
    public partial class wsInsuranceQuote
    {

        public TripXML tXML;

        private readonly modMain _modMain;

        public wsInsuranceQuote(modMain modMain)
        {
            _modMain = modMain;
        }

        #region  Decode Functions 

        // Not Implemented

        #endregion

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
                ValidateXSDOut = Conversions.ToBoolean(TripXMLMain.AppState.Get("XSD" + ttCredential.UserID + "Out"));

                switch (ttCredential.Providers[0].Name ?? "")
                {

                    // strResponse = SendOtherRequestiTravelInsured(ttServiceID, ttCredential, ttProviderSystems, strRequest)

                    case "iTravelInsured":
                    case "iTI":
                        {
                            break;
                        }

                    default:
                        {
                            throw new Exception("Provider " + ttCredential.Providers[0].Name + " Not Currently Supported.");
                        }
                }

                DateTime StartCounter;
                StartCounter = DateTime.Now;

                // Not Implemented DecodeInsuranceQuote(strResponse, ttCredential.UserID)

                CoreLib.SendTrace(ttCredential.UserID, "Performance", "Decoding = " + (int)Math.Round(DateTime.Now.Subtract(StartCounter).TotalMilliseconds), "", ttProviderSystems.LogUUID);

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
                    CoreLib.SendTrace(ttCredential.UserID, "wsInsuranceQuote", "============= OTA Response ============= ", strResponse, ttProviderSystems.LogUUID);
            }

            return strResponse;

        }

        #endregion

        #region  Web Methods 
        public string wmInsuranceQuoteXml(string xmlRequest)
        {
            return ServiceRequest(xmlRequest, ttServices.InsuranceQuote);
        }
        public wmInsuranceQuoteOut.OTA_InsuranceQuoteRS wmInsuranceQuote(wmInsuranceQuoteIn.OTA_InsuranceQuoteRQ OTA_InsuranceQuoteRQ)
        {
            string xmlMessage = "";
            wmInsuranceQuoteOut.OTA_InsuranceQuoteRS oInsuranceQuoteRS = null;
            XmlSerializer oSerializer = null;
            System.IO.StringWriter oWriter = null;
            System.IO.StringReader oReader = null;

            oSerializer = new XmlSerializer(typeof(wmInsuranceQuoteIn.OTA_InsuranceQuoteRQ));
            oWriter = new System.IO.StringWriter(new StringBuilder());
            oSerializer.Serialize(oWriter, OTA_InsuranceQuoteRQ);
            xmlMessage = oWriter.ToString();
            xmlMessage = xmlMessage.Replace(" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", "");

            xmlMessage = ServiceRequest(xmlMessage, ttServices.InsuranceQuote);

            try
            {
                oSerializer = null;
                oSerializer = new XmlSerializer(@type: typeof(wmInsuranceQuoteOut.OTA_InsuranceQuoteRS));
                oReader = new System.IO.StringReader(xmlMessage);
                oInsuranceQuoteRS = (wmInsuranceQuoteOut.OTA_InsuranceQuoteRS)oSerializer.Deserialize(oReader);
            }
            catch (Exception ex)
            {
                CoreLib.SendTrace("", "wsInsuranceQuote", "Error Deserialing OTA Response", ex.Message, string.Empty);
            }

            return oInsuranceQuoteRS;

        }

        #endregion

    }

}