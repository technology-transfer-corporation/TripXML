using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.VisualBasic.CompilerServices;
using TripXMLMain;
using static TripXMLMain.modCore;
using static TripXMLTools.TripXMLLoad;

namespace wsTripXML.wsTravelTalk
{
    public partial class wsPNRRead_v03
    {
        public TripXML tXML;

        private readonly modMain _modMain;

        public wsPNRRead_v03(modMain modMain)
        {
            _modMain = modMain;
        }

        #region  Decode Function 

        private string DecodePNRRead(string strResponse, string UserID, string UUID)
        {
            XmlDocument oDoc;
            XmlElement oRoot;

            try
            {

                oDoc = new XmlDocument();
                oDoc.LoadXml(strResponse);
                oRoot = oDoc.DocumentElement;

                var testNode = oRoot.SelectSingleNode("TravelItinerary/ItineraryInfo/ReservationItems/Item/Air");

                if (testNode is null)
                {
                    CoreLib.SendTrace(UserID, "wsPNRRead", "Error *** No Air Segments in PNR", "", UUID);
                }
                else
                {

                    foreach (XmlNode oNode in oRoot.SelectNodes("TravelItinerary/ItineraryInfo/ReservationItems/Item/Air"))
                    {
                        try
                        {
                            var arnkElem = oNode.SelectSingleNode("TPA_Extensions/Arnk");
                            if (arnkElem is not null)
                            {
                                continue;
                            }
                            // *******************
                            // Decode Airports   *
                            // *******************
                            if (oNode.SelectSingleNode("DepartureAirport") is not null)
                            {
                                // oNode.SelectSingleNode("DepartureAirport").InnerText = DecodeValue(ttAirports, oNode.SelectSingleNode("DepartureAirport").Attributes("LocationCode").Value)
                                oNode.SelectSingleNode("DepartureAirport").InnerText = DecodeValue(DecodingType.Airport, oNode.SelectSingleNode("DepartureAirport").Attributes["LocationCode"].Value);
                                // DecodeValue(DecodingType.Airport, oNode.SelectSingleNode("DepartureAirport").Attributes("LocationCode").Value)
                            }
                            if (oNode.SelectSingleNode("ArrivalAirport") is not null)
                            {
                                oNode.SelectSingleNode("ArrivalAirport").InnerText = DecodeValue(DecodingType.Airport, oNode.SelectSingleNode("ArrivalAirport").Attributes["LocationCode"].Value);
                                // DecodeValue(DecodingType.Airport, oNode.SelectSingleNode("ArrivalAirport").Attributes("LocationCode").Value)
                            }

                            // *******************
                            // Decode Airlines   *
                            // *******************
                            if (oNode.SelectSingleNode("OperatingAirline") is not null)
                            {
                                if (oNode.SelectSingleNode("OperatingAirline").Attributes["Code"] is not null)
                                {
                                    if (!string.IsNullOrEmpty(oNode.SelectSingleNode("OperatingAirline").Attributes["Code"].Value))
                                    {
                                        oNode.SelectSingleNode("OperatingAirline").InnerText = DecodeValue(DecodingType.Airline, oNode.SelectSingleNode("OperatingAirline").Attributes["Code"].Value);
                                    }
                                    else if (oNode.SelectSingleNode("OperatingAirline") is not null)
                                    {
                                        XmlAttribute attCode;
                                        attCode = oDoc.CreateAttribute("Code");
                                        attCode.Value = EncodeValue(DecodingType.Airline, oNode.SelectSingleNode("OperatingAirline").InnerText);
                                        // GetEncodeValue(ttAirlinesNames, oNode.SelectSingleNode("OperatingAirline").InnerText)
                                        oNode.SelectSingleNode("OperatingAirline").Attributes.Append(attCode);

                                        oNode.SelectSingleNode("OperatingAirline").InnerText = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(oNode.SelectSingleNode("OperatingAirline").InnerText.ToLower());
                                    }
                                }
                                else if (oNode.SelectSingleNode("OperatingAirline") is not null)
                                {
                                    XmlAttribute attCode;
                                    attCode = oDoc.CreateAttribute("Code");
                                    if (oNode.SelectSingleNode("OperatingAirline").Attributes["Code"] is not null)
                                    {
                                        attCode.Value = DecodeValue(DecodingType.Airline, oNode.SelectSingleNode("OperatingAirline").Attributes["Code"].Value);
                                        // GetEncodeValue(ttAirlines, oNode.SelectSingleNode("OperatingAirline").InnerText)

                                        if (!string.IsNullOrEmpty(attCode.Value))
                                        {
                                            oNode.SelectSingleNode("OperatingAirline").Attributes.Append(attCode);
                                            oNode.SelectSingleNode("OperatingAirline").InnerText = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(oNode.SelectSingleNode("OperatingAirline").InnerText.ToLower());
                                        }
                                    }
                                    else
                                    {
                                        attCode.Value = EncodeValue(DecodingType.Airline, oNode.SelectSingleNode("OperatingAirline").InnerText);

                                        if (!string.IsNullOrEmpty(attCode.Value))
                                        {
                                            oNode.SelectSingleNode("OperatingAirline").Attributes.Append(attCode);
                                            oNode.SelectSingleNode("OperatingAirline").InnerText = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(oNode.SelectSingleNode("OperatingAirline").InnerText.ToLower());
                                        }
                                    }
                                }
                            }
                            else if (oNode.SelectSingleNode("OperatingAirline") is not null)
                            {
                                XmlAttribute attCode;
                                attCode = oDoc.CreateAttribute("Code");
                                attCode.Value = EncodeValue(DecodingType.Airline, oNode.SelectSingleNode("OperatingAirline").InnerText);
                                // GetEncodeValue(ttAirlinesNames, oNode.SelectSingleNode("OperatingAirline").InnerText)
                                oNode.SelectSingleNode("OperatingAirline").Attributes.Append(attCode);

                                oNode.SelectSingleNode("OperatingAirline").InnerText = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(oNode.SelectSingleNode("OperatingAirline").InnerText.ToLower());
                            }

                            if (oNode.SelectSingleNode("MarketingAirline") is not null)
                            {
                                oNode.SelectSingleNode("MarketingAirline").InnerText = DecodeValue(DecodingType.Airline, oNode.SelectSingleNode("MarketingAirline").Attributes["Code"].Value);
                                // DecodeValue(DecodingType.Airline, oNode.SelectSingleNode("MarketingAirline").Attributes("Code").Value)
                            }

                            // *******************
                            // Decode Equipments   *
                            // *******************
                            if (oNode.SelectSingleNode("Equipment") is not null)
                            {
                                if (oNode.SelectSingleNode("Equipment").Attributes["AirEquipType"] is not null)
                                {
                                    oNode.SelectSingleNode("Equipment").InnerText = DecodeValue(DecodingType.Equipment, oNode.SelectSingleNode("Equipment").Attributes["AirEquipType"].Value);
                                    // DecodeValue(DecodingType.Equipment, oNode.SelectSingleNode("Equipment").Attributes("AirEquipType").Value)
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            CoreLib.SendTrace(UserID, "wsPNRRead", "Error *** Decoding AirAvail Response", e.Message, UUID);
                        }

                    }
                }

                strResponse = oDoc.OuterXml;
            }

            catch (Exception ex)
            {
                CoreLib.SendTrace(UserID, "wsPNRRead", "Error *** Decoding AirAvail Response", ex.Message, UUID);
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
            bool validateXSDOut;
            var startTime = default(DateTime);
            string uuid = "";

            try
            {
                startTime = DateTime.Now;
                _modMain.PreServiceRequest(ref strRequest, ref ttCredential, ref ttProviderSystems, startTime, (int)ttServiceID, Environment.MachineName, ref uuid);
                validateXSDOut = Conversions.ToBoolean(TripXMLMain.AppState.Get(sb.Append("XSD").Append(ttCredential.UserID).Append("Out").ToString()));
                sb.Remove(0, sb.Length);

                switch (ttCredential.Providers[0].Name ?? "")
                {
                    case "AmadeusWS":
                        {
                            strResponse = modMain.SendPNRRequestAmadeusWS(ttServiceID, ref ttCredential, ref ttProviderSystems, ref strRequest, "v03");
                            break;
                        }
                    case "Sabre":
                        {
                            if (ttProviderSystems.System is null)
                            {
                                FormatErrorMessage(ttServiceID, sb.Append("Access denied to ").Append(ttCredential.Providers[0].Name).Append(" - ").Append(ttCredential.System).Append(" system. Or invalid provider.").ToString(), ttCredential.Providers[0].Name);
                                sb.Remove(0, sb.Length);
                                break;
                            }

                            ttProviderSystems.AAAPCC = ttCredential.Providers[0].PCC;
                            strResponse = modMain.SendPNRRequestSabre(ttServiceID, ref ttCredential, ref ttProviderSystems, ref strRequest, "v03");
                            break;
                        }
                    case "Travelport":
                        {
                            strResponse = modMain.SendPNRRequestTravelPort(ttServiceID, ref ttCredential, ref ttProviderSystems, ref strRequest, "v03");
                            break;
                        }
                    case "Apollo":
                    case "Galileo":
                        {
                            strResponse = modMain.SendPNRRequestGalileo(ttServiceID, ref ttCredential, ref ttProviderSystems, ref strRequest, "v03");
                            break;
                        }
                    case "Worldspan":
                        {

                            if (Conversions.ToBoolean(TripXMLMain.modCore.config["IsTravelportWorldspan"]))
                            {
                                var ttDefProvider = new TripXMLProviderSystems();
                                _modMain.PreServiceRequest(ref strRequest, ref ttCredential, ref ttDefProvider, startTime, (int)ttServiceID, Environment.MachineName, ref uuid, "", true);
                                strResponse = modMain.SendPNRRequestTravelPort(ttServiceID, ref ttCredential, ref ttDefProvider, ref strRequest, "v03");
                            }
                            else
                            {
                                strResponse = modMain.SendPNRRequestWorldspan(ttServiceID, ref ttCredential, ref ttProviderSystems, ref strRequest, "v03");
                            }

                            break;
                        }

                    default:
                        {
                            throw new Exception(sb.Append("Provider ").Append(ttCredential.Providers[0].Name).Append(" Not Currently Supported.").ToString());
                        }
                }

                strResponse = DecodePNRRead(strResponse, ttCredential.UserID, uuid);

                modMain.PostServiceRequest(ref strResponse, validateXSDOut, (int)ttServiceID, ttCredential.UserID);
            }

            catch (Exception ex)
            {
                strResponse = FormatErrorMessage(ttServiceID, ex.Message, ttCredential.Providers[0].Name);
            }
            finally
            {
                _modMain.LogResponse(ref strResponse, ref ttCredential, startTime, (int)ttServiceID, Environment.MachineName, ref uuid);
                if (modCore.Trace)
                    CoreLib.SendTrace(ttCredential.UserID, "wsPNRRead", "============= OTA Response ============= ", strResponse, uuid);
            }
            sb = null;
            return strResponse;

        }

        #endregion

        #region  Web Methods 
        public wmTravelItineraryOut_v03.OTA_TravelItineraryRS wmPNRRead(wmPNRReadIn.OTA_ReadRQ OTA_ReadRQ)
        {

            wmTravelItineraryOut_v03.OTA_TravelItineraryRS oPNRReadRS;
            string xmlMessage = string.Empty;
            try
            {
                var oSerializer = new XmlSerializer(typeof(wmPNRReadIn.OTA_ReadRQ));
                var oWriter = new StringWriter(new StringBuilder());
                oSerializer.Serialize(oWriter, OTA_ReadRQ);

                xmlMessage = oWriter.ToString();
                xmlMessage = xmlMessage.Replace(" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", "");

                xmlMessage = ServiceRequest(xmlMessage, ttServices.PNRRead);

                oSerializer = new XmlSerializer(@type: typeof(wmTravelItineraryOut_v03.OTA_TravelItineraryRS));
                var oReader = new StringReader(xmlMessage);
                oPNRReadRS = (wmTravelItineraryOut_v03.OTA_TravelItineraryRS)oSerializer.Deserialize(oReader);
            }
            catch (Exception ex)
            {
                oPNRReadRS = GetErrorPNRObject(ex, xmlMessage);
            }

            return oPNRReadRS;

        }

        private wmTravelItineraryOut_v03.OTA_TravelItineraryRS GetErrorPNRObject(Exception ex, string xmlMessage)
        {
            wmTravelItineraryOut_v03.OTA_TravelItineraryRS oPNRReadRS;
            var errList = new List<Exception>();

            try
            {
                errList.Add(ex);
                StringReader oReader;
                XmlSerializer oSerializer;
                XmlDocument oDoc;
                XmlElement oRoot;
                oDoc = new XmlDocument();
                oDoc.LoadXml(xmlMessage);
                oRoot = oDoc.DocumentElement;
                string sessionID = "";

                if (oRoot.SelectSingleNode("ConversationID") is not null)
                {
                    sessionID = oRoot.SelectSingleNode("ConversationID").InnerText;
                }

                string itinRefXmlList;
                wmTravelItineraryOut_v03.ItineraryRef oItinRef;

                var oCustInfos = new wmTravelItineraryOut_v03.CustomerInfosRS();
                wmTravelItineraryOut_v03.TPA_ExtensionsRS oTPA;

                try
                {
                    itinRefXmlList = oRoot.SelectSingleNode("TravelItinerary/ItineraryRef")?.OuterXml;
                    oSerializer = new XmlSerializer(@type: typeof(wmTravelItineraryOut_v03.ItineraryRef));
                    oReader = new StringReader(itinRefXmlList);
                    oItinRef = (wmTravelItineraryOut_v03.ItineraryRef)oSerializer.Deserialize(oReader);
                }
                catch (Exception eref)
                {
                    oItinRef = new wmTravelItineraryOut_v03.ItineraryRef();
                    errList.Add(eref);
                }

                string custInfoXmlList;
                try
                {
                    custInfoXmlList = oRoot.SelectSingleNode("TravelItinerary/CustomerInfos")?.OuterXml;
                    oSerializer = new XmlSerializer(@type: typeof(wmTravelItineraryOut_v03.CustomerInfosRS), new XmlRootAttribute("CustomerInfos"));
                    oReader = new StringReader(custInfoXmlList);
                    oCustInfos = (wmTravelItineraryOut_v03.CustomerInfosRS)oSerializer.Deserialize(oReader);
                }

                catch (Exception ecust)
                {
                    oCustInfos = new wmTravelItineraryOut_v03.CustomerInfosRS();
                    errList.Add(ecust);
                }

                string tpaInfoXmlList;
                try
                {
                    tpaInfoXmlList = oRoot.SelectSingleNode("TravelItinerary/TPA_Extensions")?.OuterXml;
                    oSerializer = new XmlSerializer(@type: typeof(wmTravelItineraryOut_v03.TPA_ExtensionsRS), new XmlRootAttribute("TPA_Extensions"));
                    oReader = new StringReader(tpaInfoXmlList);
                    oTPA = (wmTravelItineraryOut_v03.TPA_ExtensionsRS)oSerializer.Deserialize(oReader);
                }
                catch (Exception etpa)
                {
                    oTPA = new wmTravelItineraryOut_v03.TPA_ExtensionsRS();
                    errList.Add(etpa);
                }

                var travelItin = new wmTravelItineraryOut_v03.TravelItinerary()
                {
                    ItineraryRef = oItinRef,
                    CustomerInfos = oCustInfos,
                    TPA_Extensions = oTPA
                };

                oPNRReadRS = new wmTravelItineraryOut_v03.OTA_TravelItineraryRS()
                {
                    Errors = GetErrorObject(errList),
                    ConversationID = sessionID,
                    TravelItinerary = travelItin,
                    Success = null
                };
            }

            catch (Exception exX)
            {
                errList.Add(exX);

                oPNRReadRS = new wmTravelItineraryOut_v03.OTA_TravelItineraryRS() { Errors = GetErrorObject(errList) };
            }

            return oPNRReadRS;
        }

        private wmTravelItineraryOut_v03.Error[] GetErrorObject(List<Exception> exs)
        {
            var errMessage = new List<wmTravelItineraryOut_v03.Error>();
            try
            {
                foreach (Exception ex in exs)
                {
                    errMessage.Add(new wmTravelItineraryOut_v03.Error() { Value = ex.Message });
                    if (ex.InnerException is not null)
                    {
                        errMessage.Add(new wmTravelItineraryOut_v03.Error() { Value = ex.InnerException.Message });
                    }
                }
            }
            catch (Exception exp)
            {
                errMessage.Add(new wmTravelItineraryOut_v03.Error() { Value = exp.Message });
                errMessage.Add(new wmTravelItineraryOut_v03.Error() { Value = exs.FirstOrDefault().Message });
            }

            return errMessage.ToArray();
        }
        public string wmPNRReadXml(string xmlRequest)
        {
            return ServiceRequest(xmlRequest, ttServices.PNRRead);
        }

        #endregion

    }

}