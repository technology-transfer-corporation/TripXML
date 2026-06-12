using System;
using System.Diagnostics;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.VisualBasic.CompilerServices;
using TripXMLMain;
using static TripXMLMain.modCore;
using TripXMLTools;

namespace wsTripXML.wsTravelTalk
{
    public partial class wsTravelBuild_v03
    {
        public TripXML tXML;

        private readonly modMain _modMain;

        public wsTravelBuild_v03(modMain modMain)
        {
            _modMain = modMain;
        }

        #region  Decode Function 

        private string DecodeTravelBuild(string strResponse, string UserID, ref string strUUID)
        {
            XmlDocument oDoc = null;
            XmlElement oRoot = null;

            try
            {

                oDoc = new XmlDocument();
                oDoc.LoadXml(strResponse);
                oRoot = oDoc.DocumentElement;

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
                    if (oNode.SelectSingleNode("OperatingAirline") is not null & oNode.SelectSingleNode("OperatingAirline").Attributes["Code"] is not null)
                    {
                        if (!string.IsNullOrEmpty(oNode.SelectSingleNode("OperatingAirline").Attributes["Code"].Value))
                        {
                            oNode.SelectSingleNode("OperatingAirline").InnerText = TripXMLLoad.DecodeValue(TripXMLLoad.DecodingType.Airline, oNode.SelectSingleNode("OperatingAirline").Attributes["Code"].Value);
                            // GetDecodeValue(ttAirlines, oNode.SelectSingleNode("OperatingAirline").Attributes("Code").Value)
                        }
                    }

                    if (oNode.SelectSingleNode("MarketingAirline") is not null)
                    {
                        if (oNode.SelectSingleNode("MarketingAirline").Attributes["Code"] is not null)
                        {
                            oNode.SelectSingleNode("MarketingAirline").InnerText = TripXMLLoad.DecodeValue(TripXMLLoad.DecodingType.Airline, oNode.SelectSingleNode("MarketingAirline").Attributes["Code"].Value);
                            // GetDecodeValue(ttAirlines, oNode.SelectSingleNode("MarketingAirline").Attributes("Code").Value)
                        }
                    }
                    // *******************
                    // Decode Equipments *
                    // *******************
                    if (oNode.SelectSingleNode("Equipment") is not null)
                    {
                        if (oNode.SelectSingleNode("Equipment").Attributes["AirEquipType"] is not null)
                        {
                            oNode.SelectSingleNode("Equipment").InnerText = TripXMLLoad.DecodeValue(TripXMLLoad.DecodingType.Equipment, oNode.SelectSingleNode("Equipment").Attributes["AirEquipType"].Value);
                            // GetDecodeValue(ttEquipments, oNode.SelectSingleNode("Equipment").Attributes("AirEquipType").Value)
                        }
                    }
                }

                strResponse = oDoc.OuterXml;
            }

            catch (Exception ex)
            {
                CoreLib.SendTrace(UserID, "wsTravelServices", "Error *** Decoding TravelBuild Response", ex.Message, strUUID);
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
                    case "amadeus":
                        {
                            break;
                        }
                    // Dim ttAA As AmadeusAPIAdapter

                    // ttAA = TripXMLMain.AppState.Get(sb.Append("API").Append(ttCredential.UserID).Append(ttCredential.System).Append(ttCredential.Providers(0).PCC).ToString())
                    // sb.Remove(0, sb.Length())
                    // If ttAA Is Nothing Then
                    // Throw New Exception(sb.Append("Access denied to Amadeus - ").Append(ttCredential.System).Append(" system. Or invalid provider.").ToString())
                    // sb.Remove(0, sb.Length())
                    // End If

                    // If ttCredential.Providers(0).PCC.Trim.Length > 0 Then
                    // ttAA.SourcePCC = ttCredential.Providers(0).PCC
                    // End If

                    // strResponse = SendTravelRequestAmadeus(ttServiceID, ttCredential, ttAA, strRequest, "v03")
                    // TripXMLMain.AppState.Set(sb.Append("API").Append(ttCredential.UserID).Append(ttCredential.System).ToString(), ttAA)
                    // sb.Remove(0, sb.Length())

                    case "amadeusws":
                        {

                            strResponse = modMain.SendTravelRequestAmadeusWS(ttServiceID, ref ttCredential, ref ttProviderSystems, ref strRequest, "v03");
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

                            strResponse = modMain.SendTravelRequestSabre(ttServiceID, ref ttCredential, ref ttProviderSystems, ref strRequest, "v03");
                            break;
                        }

                    case "travelfusion":
                        {
                            break;
                        }
                    // strResponse = SendTravelRequestTravelFusion(ttServiceID, ttCredential, ttProviderSystems, strRequest, "v03")
                    case "galileo":
                        {
                            strResponse = modMain.SendTravelRequestGalileo(ttServiceID, ref ttCredential, ref ttProviderSystems, ref strRequest, "v03");
                            break;
                        }
                    case "worldspan":
                        {
                            strResponse = modMain.SendTravelRequestWorldspan(ttServiceID, ref ttCredential, ref ttProviderSystems, ref strRequest, "v03");
                            break;
                        }

                    default:
                        {
                            throw new Exception(sb.Append("Provider ").Append(ttCredential.Providers[0].Name).Append(" Not Currently Supported.").ToString());
                            sb.Remove(0, sb.Length);
                            break;
                        }
                }

                strResponse = DecodeTravelBuild(strResponse, ttCredential.UserID, ref UUID);

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
                    CoreLib.SendTrace(ttCredential.UserID, "wsTravelBuild", "============= OTA Response ============= ", strResponse, UUID);
            }
            sb = null;
            return strResponse;

        }

        #endregion

        #region  Web Methods 
        public wmTravelItineraryOut_v03.OTA_TravelItineraryRS wmTravelBuild(wmTravelItineraryIn.OTA_TravelItineraryRQ OTA_TravelItineraryRQ)
        {
            var oSerializer = new XmlSerializer(typeof(wmTravelItineraryIn.OTA_TravelItineraryRQ));
            var oWriter = new System.IO.StringWriter(new StringBuilder());
            oSerializer.Serialize(oWriter, OTA_TravelItineraryRQ);
            string xmlMessage = oWriter.ToString();
            xmlMessage = xmlMessage.Replace(" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", "");

            string ipAddress = TripXMLRuntime.GetClientIpAddress();

            if (xmlMessage.Contains("</Remarks>") & xmlMessage.Contains("ATL1S2157"))
            {
                xmlMessage = xmlMessage.Replace("</Remarks>", $"<Remark>*PR*** TRAVEL SITE IP {ipAddress}</Remark></Remarks>");
            }

            xmlMessage = ServiceRequest(xmlMessage, ttServices.TravelBuild);

            wmTravelItineraryOut_v03.OTA_TravelItineraryRS oTravelBuildRS;
            System.IO.StringReader oReader;

            try
            {
                oSerializer = null;
                oSerializer = new XmlSerializer(@type: typeof(wmTravelItineraryOut_v03.OTA_TravelItineraryRS));
                oReader = new System.IO.StringReader(xmlMessage);
                oTravelBuildRS = (wmTravelItineraryOut_v03.OTA_TravelItineraryRS)oSerializer.Deserialize(oReader);
            }
            catch (Exception ex)
            {

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

                oReader = new System.IO.StringReader(xmlMessage.Replace("&", "&amp;"));
                oTravelBuildRS = (wmTravelItineraryOut_v03.OTA_TravelItineraryRS)oSerializer.Deserialize(oReader);

            }

            return oTravelBuildRS;

        }
        public string wmTravelBuildXml(string xmlRequest)
        {
            return ServiceRequest(xmlRequest, ttServices.TravelBuild);
        }

        #endregion

    }

}