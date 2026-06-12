using System;
using System.Data;
using System.Diagnostics;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.VisualBasic.CompilerServices;
using TripXMLMain;
using static TripXMLMain.modCore;


namespace wsTripXML.wsTravelTalk
{
    public partial class wsHotelInfo
    {

        public TripXML tXML;

        private readonly modMain _modMain;

        public wsHotelInfo(modMain modMain)
        {
            _modMain = modMain;
        }

        #region  Decode Function 

        private string DecodeHotelInfo(string strResponse, string UserID)
        {
            XmlDocument oDoc = null;
            XmlElement oRoot = null;
            DataView ttHotelAmenities;
            DataView ttHotelAreas;
            DataView ttHotelSubTitles;
            XmlNode oNode = null;

            try
            {

                oDoc = new XmlDocument();
                oDoc.LoadXml(strResponse);
                oRoot = oDoc.DocumentElement;

                if (oRoot.SelectNodes("HotelDescriptiveContents/HotelDescriptiveContent/FacilityInfo/GuestRooms/GuestRoom/Amenities/Amenity") is not null | oRoot.SelectNodes("Criteria/Criterion/HotelAmenity") is not null)
                {
                    ttHotelAmenities = (DataView)TripXMLMain.AppState.Get("ttHotelAmenities");

                    foreach (XmlNode currentONode in oRoot.SelectNodes("HotelDescriptiveContents/HotelDescriptiveContent/FacilityInfo/GuestRooms/GuestRoom/Amenities/Amenity"))
                    {
                        oNode = currentONode;
                        if (oNode.Attributes["RoomAmenityCode"] is not null)
                        {
                            string argstrCode = oNode.Attributes["RoomAmenityCode"].Value;
                            oNode.InnerText = modMain.GetDecodeValue(ref ttHotelAmenities, ref argstrCode);
                            oNode.Attributes["RoomAmenityCode"].Value = argstrCode;
                        }
                    }

                    foreach (XmlNode currentONode1 in oRoot.SelectNodes("Criteria/Criterion/HotelAmenity"))
                    {
                        oNode = currentONode1;
                        if (oNode.Attributes["Code"] is not null)
                        {
                            string argstrCode1 = oNode.Attributes["Code"].Value;
                            oNode.InnerText = modMain.GetDecodeValue(ref ttHotelAmenities, ref argstrCode1);
                            oNode.Attributes["Code"].Value = argstrCode1;
                        }
                    }
                }

                if (oRoot.SelectNodes("Areas/Area") is not null)
                {
                    ttHotelAreas = (DataView)TripXMLMain.AppState.Get("ttHotelAreas");

                    foreach (XmlNode currentONode2 in oRoot.SelectNodes("Areas/Area"))
                    {
                        oNode = currentONode2;
                        if (oNode.Attributes["AreaID"] is not null)
                        {
                            string argstrCode2 = oNode.Attributes["AreaID"].Value;
                            oNode.SelectSingleNode("AreaDescription/Text").InnerText = modMain.GetDecodeValue(ref ttHotelAreas, ref argstrCode2);
                            oNode.Attributes["AreaID"].Value = argstrCode2;
                        }
                    }
                }

                if (oRoot.SelectNodes("RoomStays/RoomStay/BasicPropertyInfo/VendorMessages/VendorMessage[@InfoType='Text']") is not null)
                {
                    ttHotelSubTitles = (DataView)TripXMLMain.AppState.Get("ttHotelSubTitles");

                    foreach (XmlNode currentONode3 in oRoot.SelectNodes("RoomStays/RoomStay/BasicPropertyInfo/VendorMessages/VendorMessage[@InfoType='Text']/SubSection"))
                    {
                        oNode = currentONode3;
                        if (oNode.Attributes["SubCode"] is not null)
                        {
                            string argstrCode3 = oNode.Attributes["SubCode"].Value;
                            oNode.Attributes["SubTitle"].Value = modMain.GetDecodeValue(ref ttHotelSubTitles, ref argstrCode3);
                            oNode.Attributes["SubCode"].Value = argstrCode3;
                        }
                    }
                }

                strResponse = oDoc.OuterXml;
            }

            catch (Exception ex)
            {
                CoreLib.SendTrace(UserID, "wsHotelAvail", "Error *** Decoding HotelAvail Response", ex.Message, string.Empty);
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

                strRequest = strRequest.Replace(" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", "");
                strRequest = strRequest.Replace(" xmlns=\"http://tripxml.downtowntravel.com/tripxml/wsHotelInfo\"", "");
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

                    // strResponse = SendHotelRequestAmadeus(ttServiceID, ttCredential, ttAA, strRequest)
                    // TripXMLMain.AppState.Set(sb.Append("API").Append(ttCredential.UserID).Append(ttCredential.System).ToString(), ttAA)
                    // sb.Remove(0, sb.Length())

                    case "amadeusws":
                        {

                            strResponse = modMain.SendHotelRequestAmadeusWS(ttServiceID, ref ttCredential, ref ttProviderSystems, ref strRequest);
                            break;
                        }

                    case "apollo":
                    case "galileo":
                        {

                            strResponse = modMain.SendHotelRequestGalileo(ttServiceID, ref ttCredential, ref ttProviderSystems, ref strRequest);
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

                            strResponse = modMain.SendHotelRequestSabre(ttServiceID, ref ttCredential, ref ttProviderSystems, ref strRequest);
                            break;
                        }

                    default:
                        {
                            throw new Exception(sb.Append("Provider ").Append(ttCredential.Providers[0].Name).Append(" Not Currently Supported.").ToString());
                            sb.Remove(0, sb.Length);
                            break;
                        }
                }

                strResponse = DecodeHotelInfo(strResponse, ttCredential.UserID);

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
                    CoreLib.SendTrace(ttCredential.UserID, "wsHotelInfo", "============= OTA Response ============= ", strResponse, UUID);
            }

            return strResponse;
            sb = null;
        }

        #endregion

        #region  Web Methods 
        public wmHotelInfoOut.OTA_HotelDescriptiveInfoRS wmHotelInfo(wmHotelInfoIn.OTA_HotelDescriptiveInfoRQ OTA_HotelDescriptiveInfoRQ)
        {
            string xmlMessage = "";
            wmHotelInfoOut.OTA_HotelDescriptiveInfoRS oHotelInfoRS = null;
            XmlSerializer oSerializer = null;
            System.IO.StringWriter oWriter = null;
            System.IO.StringReader oReader = null;

            oSerializer = new XmlSerializer(typeof(wmHotelInfoIn.OTA_HotelDescriptiveInfoRQ));
            oWriter = new System.IO.StringWriter(new StringBuilder());
            oSerializer.Serialize(oWriter, OTA_HotelDescriptiveInfoRQ);
            xmlMessage = oWriter.ToString();
            xmlMessage = xmlMessage.Replace(" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", "");

            xmlMessage = ServiceRequest(xmlMessage, ttServices.HotelInfo);

            try
            {
                oSerializer = null;
                oSerializer = new XmlSerializer(@type: typeof(wmHotelInfoOut.OTA_HotelDescriptiveInfoRS));
                oReader = new System.IO.StringReader(xmlMessage);
                oHotelInfoRS = (wmHotelInfoOut.OTA_HotelDescriptiveInfoRS)oSerializer.Deserialize(oReader);
            }
            catch (Exception ex)
            {
                CoreLib.SendTrace("", "wsHotelInfo", "Error Deserialing OTA Response", ex.InnerException.ToString(), string.Empty);
            }

            return oHotelInfoRS;

        }
        public string wmHotelInfoXml(string xmlRequest)
        {
            return ServiceRequest(xmlRequest, ttServices.HotelInfo);
        }

        #endregion

    }

}