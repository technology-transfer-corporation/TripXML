using System;
using System.Diagnostics;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.VisualBasic.CompilerServices;
using TripXMLMain;
using static TripXMLMain.modCore;

namespace wsTripXML.wsTravelTalk
{
    public partial class wsUpdate
    {

        private readonly modMain _modMain;

        public wsUpdate(modMain modMain)
        {
            _modMain = modMain;
        }

        #region  Decode Function 

        // Not Implemented

        #endregion
        private StringBuilder sb = new StringBuilder();

        private string mstrResponse = "";
        private int mintProviders = 0;
        private string RecordLocator = "";
        // Private ttAPIAdapter As AmadeusAPIAdapter

        private void GotResponse(string Response)
        {
            mstrResponse += Response;
            mintProviders += 1;
        }

        public TripXML tXML;

        #region  Process Service Request All Suppliers 
        private string ServiceRequest(string strRequest, ttServices ttServiceID)
        {
            string strResponse = "";
            // Dim ttAA As AmadeusAdapter = Nothing
            TravelTalkCredential ttCredential = default;
            TripXMLProviderSystems ttProviderSystems = default;
            bool ValidateXSDOut;
            var StartTime = default(DateTime);
            string UUID = "";
            string ConversationID = "";

            try
            {
                StartTime = DateTime.Now;
                _modMain.PreServiceRequest(ref strRequest, ref ttCredential, ref ttProviderSystems, StartTime, (int)ttServiceID, Environment.MachineName, ref UUID);
                ValidateXSDOut = Conversions.ToBoolean(TripXMLMain.AppState.Get(sb.Append("XSD").Append(ttCredential.UserID).Append("Out").ToString()));
                sb.Remove(0, sb.Length);

                {
                    ref var withBlock = ref ttCredential;
                    switch (withBlock.Providers[0].Name.ToLower() ?? "")
                    {
                        case "amadeus":
                            {
                                break;
                            }
                        // ttAPIAdapter = TripXMLMain.AppState.Get(sb.Append("API").Append(.UserID).Append(.System).Append(.Providers(0).PCC).ToString())
                        // sb.Remove(0, sb.Length())
                        // If ttAPIAdapter Is Nothing Then
                        // Throw New Exception(sb.Append("Access denied to Amadeus - ").Append(.System).Append(" system. Or invalid provider.").ToString())
                        // sb.Remove(0, sb.Length())
                        // End If

                        // If ttCredential.Providers(0).PCC.Trim.Length > 0 Then
                        // ttAPIAdapter.SourcePCC = ttCredential.Providers(0).PCC
                        // End If

                        // '********************************
                        // '* Send  PNR Modify Request     * 
                        // '********************************
                        // strResponse = SendTravelRequestAmadeus(ttServiceID, ttCredential, ttAPIAdapter, strRequest)
                        // TripXMLMain.AppState.Set(sb.Append("API").Append(ttCredential.UserID).Append(ttCredential.System).ToString(), ttAPIAdapter)
                        // sb.Remove(0, sb.Length())

                        case "amadeusws":
                            {

                                strResponse = modMain.SendTravelRequestAmadeusWS(ttServiceID, ref ttCredential, ref ttProviderSystems, ref strRequest);
                                break;
                            }

                        case "apollo":
                        case "galileo":
                            {
                                ttProviderSystems = (TripXMLProviderSystems)TripXMLMain.AppState.Get(sb.Append("PS").Append(ttCredential.Providers[0].Name).Append(ttCredential.UserID).Append(ttCredential.System).Append(ttCredential.Providers[0].PCC).ToString());
                                sb.Remove(0, sb.Length);
                                if (ttProviderSystems.System is null)
                                {
                                    FormatErrorMessage(ttServiceID, sb.Append("Access denied to ").Append(ttCredential.Providers[0].Name).Append(" - ").Append(ttCredential.System).Append(" system. Or invalid provider.").ToString(), ttCredential.Providers[0].Name);
                                    sb.Remove(0, sb.Length);
                                    break;
                                }

                                strResponse = modMain.SendTravelRequestGalileo(ttServiceID, ref ttCredential, ref ttProviderSystems, ref strRequest);
                                break;
                            }

                        case "sabre":
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

                                strResponse = modMain.SendTravelRequestSabre(ttServiceID, ref ttCredential, ref ttProviderSystems, ref strRequest);
                                break;
                            }
                        case "worldspan":
                            {

                                strResponse = modMain.SendTravelRequestWorldspan(ttServiceID, ref ttCredential, ref ttProviderSystems, ref strRequest);
                                break;
                            }

                        default:
                            {
                                GotResponse(FormatErrorMessage(ttServiceID, sb.Append("Provider ").Append(withBlock.Providers[0].Name).Append(" Not Currently Supported.").ToString(), withBlock.Providers[0].Name));
                                sb.Remove(0, sb.Length);
                                break;
                            }
                    }

                }

                // DecodeUpdate(strResponse) Not Implemented.

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
                    CoreLib.SendTrace(ttCredential.UserID, "wsUpdate", "============= OTA Response ============= ", strResponse, UUID);
            }
            sb = null;
            return strResponse;

        }

        #endregion

        #region  Web Methods 
        public wmTravelItineraryOut_v03.OTA_TravelItineraryRS wmUpdate(wmUpdateIn.OTA_UpdateRQ OTA_UpdateRQ)
        {
            string xmlMessage = "";
            wmTravelItineraryOut_v03.OTA_TravelItineraryRS OTA_UpdateRS = null;
            XmlSerializer oSerializer = null;
            System.IO.StringWriter oWriter = null;
            System.IO.StringReader oReader = null;

            oSerializer = new XmlSerializer(typeof(wmUpdateIn.OTA_UpdateRQ));
            oWriter = new System.IO.StringWriter(new StringBuilder());

            // *************************************
            // * Get PNR Modify XML Request Msg    * 
            // *************************************
            oSerializer.Serialize(oWriter, OTA_UpdateRQ);
            xmlMessage = oWriter.ToString();
            xmlMessage = xmlMessage.Replace(" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", "");
            xmlMessage = xmlMessage.Replace(" xmlns=\"http://tripxml.downtowntravel.com/tripxml/wsUpdate\"", "");
            xmlMessage = xmlMessage.Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", "");

            xmlMessage = ServiceRequest(xmlMessage, ttServices.Update);

            try
            {
                oSerializer = null;
                oSerializer = new XmlSerializer(@type: typeof(wmTravelItineraryOut_v03.OTA_TravelItineraryRS));
                oReader = new System.IO.StringReader(xmlMessage);
                OTA_UpdateRS = (wmTravelItineraryOut_v03.OTA_TravelItineraryRS)oSerializer.Deserialize(oReader);
            }
            catch (Exception ex)
            {
                CoreLib.SendTrace("", "wsUpdate", "Error Deserialing OTA Response", ex.Message, string.Empty);
                XmlDocument oDoc;
                XmlElement oRoot;
                oDoc = new XmlDocument();
                oDoc.LoadXml(xmlMessage);
                oRoot = oDoc.DocumentElement;
                string sessionID = "";
                if (oRoot.SelectSingleNode("ConversationID") is not null)
                {
                    sessionID = oRoot.SelectSingleNode("ConversationID").OuterXml.Replace("&amp;", "&");
                }

                string itinRefXmlList = oRoot.SelectSingleNode("TravelItinerary/ItineraryRef").OuterXml;
                string custInfoXmlList = oRoot.SelectSingleNode("TravelItinerary/CustomerInfos").OuterXml;
                string tpaInfoXmlList = oRoot.SelectSingleNode("TravelItinerary/TPA_Extensions").OuterXml;
                string errMessage = string.Format("<Errors><Error>{0}</Error><Error>{1}</Error></Errors>", ex.InnerException.Message.ToString(), ex.Message.ToString());

                xmlMessage = string.Format("<OTA_TravelItineraryRS Version=\"v03\" xmlns:stl=\"http://services.sabre.com/STL/v01\">{0}<TravelItinerary>{1}{2}{3}{4}</TravelItinerary>{5}</OTA_TravelItineraryRS>", errMessage, itinRefXmlList, custInfoXmlList, "<ItineraryInfo></ItineraryInfo>", tpaInfoXmlList, sessionID);

                oReader = new System.IO.StringReader(xmlMessage);
                OTA_UpdateRS = (wmTravelItineraryOut_v03.OTA_TravelItineraryRS)oSerializer.Deserialize(oReader);

            }

            return OTA_UpdateRS;

        }
        public string wmUpdateXml(string xmlRequest)
        {
            return ServiceRequest(xmlRequest, ttServices.Update);
        }

        #endregion

    }

}