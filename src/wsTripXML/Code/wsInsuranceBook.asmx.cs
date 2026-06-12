using System;
using System.Diagnostics;
using System.Text;
using System.Xml.Serialization;
using Microsoft.VisualBasic.CompilerServices;
using TripXMLMain;
using static TripXMLMain.modCore;

namespace wsTripXML.wsTravelTalk
{
    public partial class wsInsuranceBook
    {

        public TripXML tXML;

        private readonly modMain _modMain;

        public wsInsuranceBook(modMain modMain)
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

                // Not Implemented DecodeInsuranceBook(strResponse, ttCredential.UserID)

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
                    CoreLib.SendTrace(ttCredential.UserID, "wsInsuranceBook", "============= OTA Response ============= ", strResponse, ttProviderSystems.LogUUID);
            }

            return strResponse;

        }

        #endregion

        #region  Web Methods 
        public string wmInsuranceBookXml(string xmlRequest)
        {
            return ServiceRequest(xmlRequest, ttServices.InsuranceBook);
        }
        public wmInsuranceBookOut.OTA_InsuranceBookRS wmInsuranceBook(wmInsuranceBookIn.OTA_InsuranceBookRQ OTA_InsuranceBookRQ)
        {
            string xmlMessage = "";
            wmInsuranceBookOut.OTA_InsuranceBookRS oInsuranceBookRS = null;
            XmlSerializer oSerializer = null;
            System.IO.StringWriter oWriter = null;
            System.IO.StringReader oReader = null;

            oSerializer = new XmlSerializer(typeof(wmInsuranceBookIn.OTA_InsuranceBookRQ));
            oWriter = new System.IO.StringWriter(new StringBuilder());
            oSerializer.Serialize(oWriter, OTA_InsuranceBookRQ);
            xmlMessage = oWriter.ToString();
            xmlMessage = xmlMessage.Replace(" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", "");

            xmlMessage = ServiceRequest(xmlMessage, ttServices.InsuranceBook);

            try
            {
                oSerializer = null;
                oSerializer = new XmlSerializer(@type: typeof(wmInsuranceBookOut.OTA_InsuranceBookRS));
                oReader = new System.IO.StringReader(xmlMessage);
                oInsuranceBookRS = (wmInsuranceBookOut.OTA_InsuranceBookRS)oSerializer.Deserialize(oReader);
            }
            catch (Exception ex)
            {
                CoreLib.SendTrace("", "wsInsuranceBook", "Error Deserialing OTA Response", ex.Message, string.Empty);
            }

            return oInsuranceBookRS;

        }

        #endregion

    }

}