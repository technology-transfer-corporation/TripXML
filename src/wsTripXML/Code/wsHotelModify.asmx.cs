using System;
using System.Diagnostics;
using System.Text;
using System.Web.Services;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.VisualBasic.CompilerServices;
using TripXMLMain;
using static TripXMLMain.modCore;
using TripXMLTools;

namespace wsTripXML.wsTravelTalk
{


    [WebService(Namespace = "http://tripxml.downtowntravel.com/tripxml/wsHotelModify", Name = "wsHotelModify", Description = "A TripXML Web Service to Process Hotel Modify Messages Request.")]
    public class wsHotelModify : WebService
    {

        #region  Web Services Designer Generated Code 

        public wsHotelModify() : base()
        {

            // This call is required by the Web Services Designer.
            InitializeComponent();

            // Add your own initialization code after the InitializeComponent() call

        }

        // Required by the Web Services Designer
        private System.ComponentModel.IContainer components;

        // NOTE: The following procedure is required by the Web Services Designer
        // It can be modified using the Web Services Designer.  
        // Do not modify it using the code editor.
        [DebuggerStepThrough()]
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
        }

        protected override void Dispose(bool disposing)
        {
            // CODEGEN: This procedure is required by the Web Services Designer
            // Do not modify it using the code editor.
            if (disposing)
            {
                if (components is not null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #endregion

        #region  Decode Function 

        private string DecodePNRRead(string strResponse, string UserID)
        {
            try
            {
                var oDoc = new XmlDocument();
                oDoc.LoadXml(strResponse);

                // Dim ttAirports As DataView = CType(Application.Get("ttAirports"), DataView)
                // Dim ttAirlines As DataView = CType(Application.Get("ttAirlines"), DataView)

                var oRoot = oDoc.DocumentElement;
                foreach (XmlNode oNode in oRoot.SelectNodes("TravelItinerary/ItineraryInfo/ReservationItems/Item/Air"))
                {
                    // *******************
                    // Decode Airports   *
                    // *******************
                    if (oNode.SelectSingleNode("DepartureAirport") is not null)
                    {
                        oNode.SelectSingleNode("DepartureAirport").InnerText = TripXMLLoad.DecodeValue(TripXMLLoad.DecodingType.Airport, oNode.SelectSingleNode("DepartureAirport").Attributes["LocationCode"].Value);
                        // GetDecodeValue(ttAirports, oNode.SelectSingleNode("DepartureAirport").Attributes("LocationCode").Value)
                    }
                    if (oNode.SelectSingleNode("ArrivalAirport") is not null)
                    {
                        oNode.SelectSingleNode("ArrivalAirport").InnerText = TripXMLLoad.DecodeValue(TripXMLLoad.DecodingType.Airport, oNode.SelectSingleNode("ArrivalAirport").Attributes["LocationCode"].Value);
                        // GetDecodeValue(ttAirports, oNode.SelectSingleNode("ArrivalAirport").Attributes("LocationCode").Value)
                    }

                    // *******************
                    // Decode Airlines   *
                    // *******************
                    if (oNode.SelectSingleNode("OperatingAirline") is not null)
                    {
                        oNode.SelectSingleNode("OperatingAirline").InnerText = TripXMLLoad.DecodeValue(TripXMLLoad.DecodingType.Airline, oNode.SelectSingleNode("OperatingAirline").Attributes["Code"].Value);
                        // GetDecodeValue(ttAirlines, oNode.SelectSingleNode("OperatingAirline").Attributes("Code").Value)
                    }
                    if (oNode.SelectSingleNode("MarketingAirline") is not null)
                    {
                        oNode.SelectSingleNode("MarketingAirline").InnerText = TripXMLLoad.DecodeValue(TripXMLLoad.DecodingType.Airline, oNode.SelectSingleNode("MarketingAirline").Attributes["Code"].Value);
                        // GetDecodeValue(ttAirlines, oNode.SelectSingleNode("MarketingAirline").Attributes("Code").Value)
                    }
                }

                strResponse = oDoc.OuterXml;
            }

            catch (Exception ex)
            {
                CoreLib.SendTrace(UserID, "wsPNRRead", "Error *** Decoding AirAvail Response", ex.Message, string.Empty);
            }
            return strResponse;
        }

        #endregion

        #region  Process Service Request All GDS 
        private StringBuilder sb = new StringBuilder();

        private string ServiceRequest(string strRequest, ttServices ttServiceID)
        {
            string strResponse = "";
            TravelTalkCredential ttCredential = default;
            TripXMLProviderSystems ttProviderSystems = default;
            bool ValidateXSDOut;
            DateTime StartTime = default;
            DateTime StartCounter = default;
            string UUID = "";

            try
            {
                StartTime = DateTime.Now;

                var argoApp = Application;
                modMain.PreServiceRequest(ref strRequest, ref argoApp, ref ttCredential, ref ttProviderSystems, StartTime, (int)ttServiceID, Server.MachineName, ref UUID);
                ValidateXSDOut = Conversions.ToBoolean(Application.Get(sb.Append("XSD").Append(ttCredential.UserID).Append("Out").ToString()));
                sb.Remove(0, sb.Length);

                // Case "OutriggerR"

                // strResponse = SendHotelRequestOutriggerR(ttServiceID, ttCredential, ttProviderSystems, strRequest)

                switch (ttCredential.Providers[0].Name ?? "")
                {
                    default:
                        {
                            throw new Exception(sb.Append("Provider ").Append(ttCredential.Providers[0].Name).Append(" Not Currently Supported.").ToString());
                            sb.Remove(0, sb.Length);
                            break;
                        }
                }

                StartCounter = DateTime.Now;

                strResponse = DecodePNRRead(strResponse, ttCredential.UserID);

                CoreLib.SendTrace(ttCredential.UserID, "Performance", sb.Append("Decoding = ").Append((int)Math.Round(DateTime.Now.Subtract(StartCounter).TotalMilliseconds)).ToString(), "", ttProviderSystems.LogUUID);
                sb.Remove(0, sb.Length);

                modMain.PostServiceRequest(ref strResponse, ValidateXSDOut, (int)ttServiceID, ttCredential.UserID);
            }

            catch (Exception ex)
            {
                strResponse = FormatErrorMessage(ttServiceID, ex.Message, ttCredential.Providers[0].Name);
            }
            finally
            {
                modMain.LogResponse(ref strResponse, ref ttCredential, StartTime, (int)ttServiceID, Server.MachineName, ref UUID);
                if (modCore.Trace)
                    CoreLib.SendTrace(ttCredential.UserID, "wsHotelModify", "============= OTA Response ============= ", strResponse, ttProviderSystems.LogUUID);
            }
            sb = null;
            return strResponse;

        }

        #endregion

        #region  Web Methods 

        [WebMethod(Description = "Process Hotel Modify Messages Request.")]
        public wmTravelItineraryOut.OTA_TravelItineraryRS wmHotelModify(wmHotelModifyIn.OTA_HotelResModifyRQ OTA_HotelResModifyRQ)
        {
            string xmlMessage = "";
            wmTravelItineraryOut.OTA_TravelItineraryRS oTravelBuildRS = null;
            XmlSerializer oSerializer = null;
            System.IO.StringWriter oWriter = null;
            System.IO.StringReader oReader = null;

            oSerializer = new XmlSerializer(typeof(wmHotelModifyIn.OTA_HotelResModifyRQ));
            oWriter = new System.IO.StringWriter(new StringBuilder());
            oSerializer.Serialize(oWriter, OTA_HotelResModifyRQ);
            xmlMessage = oWriter.ToString();
            xmlMessage = xmlMessage.Replace(" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", "");

            xmlMessage = ServiceRequest(xmlMessage, ttServices.HotelModify);

            try
            {
                oSerializer = null;
                oSerializer = new XmlSerializer(@type: typeof(wmTravelItineraryOut.OTA_TravelItineraryRS));
                oReader = new System.IO.StringReader(xmlMessage);
                oTravelBuildRS = (wmTravelItineraryOut.OTA_TravelItineraryRS)oSerializer.Deserialize(oReader);
            }
            catch (Exception ex)
            {
                CoreLib.SendTrace("", "wsHotelModify", "Error Deserialing OTA Response", ex.Message, string.Empty);
            }

            return oTravelBuildRS;

        }

        [WebMethod(Description = "Process Hotel Modify Xml Messages Request.")]
        public string wmHotelModifyXml(string xmlRequest)
        {
            return ServiceRequest(xmlRequest, ttServices.HotelModify);
        }

        #endregion

    }

}