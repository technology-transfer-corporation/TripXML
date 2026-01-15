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


    [WebService(Namespace = "http://tripxml.downtowntravel.com/tripxml/wsMultiMessage", Name = "wsMultiMessage", Description = "A TripXML Web Service to Process MultiMessage Messages Request.")]
    public class wsMultiMessage : WebService
    {

        #region  Web Services Designer Generated Code 

        public wsMultiMessage() : base()
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
        private StringBuilder sb = new StringBuilder();

        private string DecodeMultiMessage(string strResponse, string UserID)
        {
            XmlDocument oDoc = null;
            XmlElement oRoot = null;
            string strResp = "";

            try
            {

                oDoc = new XmlDocument();
                oDoc.LoadXml(strResponse);
                oRoot = oDoc.DocumentElement;

                foreach (XmlNode oNodeLF in oRoot.SelectSingleNode("Response").ChildNodes)
                {
                    foreach (XmlNode oNode in oNodeLF.SelectNodes("PricedItineraries/PricedItinerary/AirItinerary/OriginDestinationOptions/OriginDestinationOption/FlightSegment"))
                    {
                        // *******************
                        // Decode Airports   *
                        // *******************
                        oNode.SelectSingleNode("DepartureAirport").InnerText = TripXMLLoad.DecodeValue(TripXMLLoad.DecodingType.Airport, oNode.SelectSingleNode("DepartureAirport").Attributes["LocationCode"].Value);
                        // GetDecodeValue(ttAirports, oNode.SelectSingleNode("DepartureAirport").Attributes("LocationCode").Value)
                        oNode.SelectSingleNode("ArrivalAirport").InnerText = TripXMLLoad.DecodeValue(TripXMLLoad.DecodingType.Airport, oNode.SelectSingleNode("ArrivalAirport").Attributes["LocationCode"].Value);
                        // GetDecodeValue(ttAirports, oNode.SelectSingleNode("ArrivalAirport").Attributes("LocationCode").Value)

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

                        // *******************
                        // Decode Equipments *
                        // *******************
                        if (oNode.SelectSingleNode("Equipment") is not null)
                        {
                            oNode.SelectSingleNode("Equipment").InnerText = TripXMLLoad.DecodeValue(TripXMLLoad.DecodingType.Equipment, oNode.SelectSingleNode("Equipment").Attributes["AirEquipType"].Value);
                            // GetDecodeValue(ttEquipments, oNode.SelectSingleNode("Equipment").Attributes("AirEquipType").Value)
                        }
                    }
                    strResp = sb.Append(strResp).Append(oNodeLF.OuterXml).ToString();
                    sb.Remove(0, sb.Length);
                }

                // strResponse = oDoc.OuterXml

                strResponse = sb.Append("<MultiMessageRS><Success/><Response>").Append(strResp.Replace("<", "&lt;").Replace(">", "&gt;")).Append("</Response></MultiMessageRS>").ToString();
                sb.Remove(0, sb.Length);
            }

            catch (Exception ex)
            {
                CoreLib.SendTrace(UserID, "wsAirServices", "Error *** Decoding MultiMessage Response", ex.Message, string.Empty);
            }
            return strResponse;
            sb = null;
        }

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

                var argoApp = Application;
                modMain.PreServiceRequest(ref strRequest, ref argoApp, ref ttCredential, ref ttProviderSystems, StartTime, (int)ttServiceID, Server.MachineName, ref UUID);
                ValidateXSDOut = Conversions.ToBoolean(Application.Get(sb.Append("XSD").Append(ttCredential.UserID).Append("Out").ToString()));
                sb.Remove(0, sb.Length);

                strRequest = strRequest.Replace("&amp;", "&");
                strRequest = strRequest.Replace("&lt;", "<").Replace("&gt;", ">");

                switch (ttCredential.Providers[0].Name.ToLower() ?? "")
                {
                    // Case "amadeus"
                    // Dim ttAA As AmadeusAPIAdapter

                    // ttAA = Application.Get("API").Append(ttCredential.UserID).Append(ttCredential.System).Append(ttCredential.Providers(0).PCC)
                    // If ttAA Is Nothing Then
                    // Throw New Exception("Access denied to Amadeus - ").Append(ttCredential.System).Append(" system. Or invalid provider.")
                    // End If

                    // If ttCredential.Providers(0).PCC.Trim.Length > 0 Then
                    // ttAA.SourcePCC = ttCredential.Providers(0).PCC
                    // End If

                    // strResponse = SendOtherRequestAmadeus(ttServiceID, ttCredential, ttAA, strRequest)
                    // Application.Set("API").Append(ttCredential.UserID).Append(ttCredential.System, ttAA)

                    case "apollo":
                    case "galileo":
                        {


                            // Case "sabre"

                            // strResponse = SendOtherRequestSabre(ttServiceID, ttCredential, ttProviderSystems, strRequest)

                            // Case "worldspan"

                            // strResponse = SendOtherRequestWorldspan(ttServiceID, ttCredential, ttProviderSystems, strRequest)

                            strResponse = modMain.SendOtherRequestGalileo(ttServiceID, ref ttCredential, ref ttProviderSystems, ref strRequest);
                            break;
                        }

                    default:
                        {
                            throw new Exception(sb.Append("Provider ").Append(ttCredential.Providers[0].Name).Append(" Not Currently Supported.").ToString());
                            sb.Remove(0, sb.Length);
                            break;
                        }
                }

                strResponse = DecodeMultiMessage(strResponse, ttCredential.UserID);
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
                    CoreLib.SendTrace(ttCredential.UserID, "wsMultiMessage", "============= OTA Response ============= ", strResponse, UUID);
            }
            sb = null;
            return strResponse;

        }

        #endregion

        #region  Web Methods 

        [WebMethod(Description = "Process MultiMessage Messages Request.")]
        public wmMultiMessageOut.MultiMessageRS wmMultiMessage(wmMultiMessageIn.MultiMessageRQ MultiMessageRQ)
        {
            string xmlMessage = "";
            wmMultiMessageOut.MultiMessageRS oMultiMessageRS = null;
            XmlSerializer oSerializer = null;
            System.IO.StringWriter oWriter = null;
            System.IO.StringReader oReader = null;

            MultiMessageRQ.MultiMessage = MultiMessageRQ.MultiMessage.Replace("<", "&lt;").Replace(">", "&gt;");

            oSerializer = new XmlSerializer(typeof(wmMultiMessageIn.MultiMessageRQ));
            oWriter = new System.IO.StringWriter(new StringBuilder());
            oSerializer.Serialize(oWriter, MultiMessageRQ);
            xmlMessage = oWriter.ToString();
            xmlMessage = xmlMessage.Replace(" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", "");
            xmlMessage = xmlMessage.Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", "");
            xmlMessage = xmlMessage.Replace("&amp;lt;", "&lt;").Replace("&amp;gt;", "&gt;");

            xmlMessage = ServiceRequest(xmlMessage, ttServices.MultiMessage);

            try
            {
                oSerializer = null;
                oSerializer = new XmlSerializer(@type: typeof(wmMultiMessageOut.MultiMessageRS));
                oReader = new System.IO.StringReader(xmlMessage);
                oMultiMessageRS = (wmMultiMessageOut.MultiMessageRS)oSerializer.Deserialize(oReader);
            }
            catch (Exception ex)
            {
                CoreLib.SendTrace("", "wsMultiMessage", "Error Deserialing OTA Response", ex.Message, string.Empty);
            }

            return oMultiMessageRS;

        }

        [WebMethod(Description = "Process MultiMessage Xml Messages Request.")]
        public string wmMultiMessageXml(string xmlRequest)
        {
            return ServiceRequest(xmlRequest, ttServices.MultiMessage);
        }

        #endregion

    }

}