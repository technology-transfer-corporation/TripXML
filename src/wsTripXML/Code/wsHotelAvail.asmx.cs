using System;
using System.Data;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.VisualBasic.CompilerServices;
using TripXMLMain;
using static TripXMLMain.modCore;

namespace wsTripXML.wsTravelTalk
{
    public partial class wsHotelAvail
    {

        private readonly modMain _modMain;

        public wsHotelAvail(modMain modMain)
        {
            _modMain = modMain;
        }

        #region  Decode Function 

        private string DecodeHotelAvail(string strResponse, string UserID)
        {
            XmlDocument oDoc = null;
            XmlElement oRoot = null;
            DataView ttHotels;
            DataView ttHotelAmenities;
            DataView ttHotelAreas;
            DataView ttHotelSubTitles;
            XmlNode oNode = null;

            try
            {

                oDoc = new XmlDocument();
                oDoc.LoadXml(strResponse);
                oRoot = oDoc.DocumentElement;

                ttHotels = (DataView)TripXMLMain.AppState.Get("ttHotels");

                foreach (XmlNode currentONode in oRoot.SelectNodes("RoomStays/RoomStay/BasicPropertyInfo"))
                {
                    oNode = currentONode;
                    // *******************
                    // Decode Hotels   *
                    // *******************
                    if (!(oNode.Attributes["ChainCode"] is null | oNode.Attributes["ChainName"] is null))
                    {
                        string argstrCode = oNode.Attributes["ChainCode"].Value;
                        oNode.Attributes["ChainName"].Value = modMain.GetDecodeValue(ref ttHotels, ref argstrCode);
                        oNode.Attributes["ChainCode"].Value = argstrCode;
                    }
                    // *******************************
                    // Decode Hotels for OutriggerR  *
                    // *******************************
                    if (!(oNode.Attributes["HotelCode"] is null | oNode.Attributes["HotelName"] is null))
                    {
                        if (oNode.Attributes["HotelName"].Value.Length == 0)
                        {
                            string argstrCode1 = oNode.Attributes["HotelCode"].Value;
                            oNode.Attributes["HotelName"].Value = modMain.GetDecodeValue(ref ttHotels, ref argstrCode1);
                            oNode.Attributes["HotelCode"].Value = argstrCode1;
                        }
                    }
                }

                if (oRoot.SelectNodes("RoomStays/RoomStay/RoomTypes/RoomType/Amenities/Amenity") is not null | oRoot.SelectNodes("RoomStays/RoomStay/BasicPropertyInfo/HotelAmenity") is not null)
                {
                    ttHotelAmenities = (DataView)TripXMLMain.AppState.Get("ttHotelAmenities");

                    foreach (XmlNode currentONode1 in oRoot.SelectNodes("RoomStays/RoomStay/RoomTypes/RoomType/Amenities/Amenity"))
                    {
                        oNode = currentONode1;
                        if (oNode.Attributes["RoomAmenity"] is not null)
                        {
                            string argstrCode2 = oNode.Attributes["RoomAmenity"].Value;
                            oNode.InnerText = modMain.GetDecodeValue(ref ttHotelAmenities, ref argstrCode2);
                            oNode.Attributes["RoomAmenity"].Value = argstrCode2;
                        }
                    }

                    foreach (XmlNode currentONode2 in oRoot.SelectNodes("RoomStays/RoomStay/BasicPropertyInfo/HotelAmenity"))
                    {
                        oNode = currentONode2;
                        if (oNode.Attributes["Code"] is not null)
                        {
                            string argstrCode3 = oNode.Attributes["Code"].Value;
                            oNode.InnerText = modMain.GetDecodeValue(ref ttHotelAmenities, ref argstrCode3);
                            oNode.Attributes["Code"].Value = argstrCode3;
                        }
                    }
                }

                if (oRoot.SelectNodes("Areas/Area") is not null)
                {
                    ttHotelAreas = (DataView)TripXMLMain.AppState.Get("ttHotelAreas");

                    foreach (XmlNode currentONode3 in oRoot.SelectNodes("Areas/Area"))
                    {
                        oNode = currentONode3;
                        if (oNode.Attributes["AreaID"] is not null)
                        {
                            string argstrCode4 = oNode.Attributes["AreaID"].Value;
                            oNode.SelectSingleNode("AreaDescription/Text").InnerText = modMain.GetDecodeValue(ref ttHotelAreas, ref argstrCode4);
                            oNode.Attributes["AreaID"].Value = argstrCode4;
                        }
                    }
                }

                if (oRoot.SelectNodes("RoomStays/RoomStay/BasicPropertyInfo/VendorMessages/VendorMessage[@InfoType='Text']") is not null)
                {
                    ttHotelSubTitles = (DataView)TripXMLMain.AppState.Get("ttHotelSubTitles");

                    foreach (XmlNode currentONode4 in oRoot.SelectNodes("RoomStays/RoomStay/BasicPropertyInfo/VendorMessages/VendorMessage[@InfoType='Text']/SubSection"))
                    {
                        oNode = currentONode4;
                        if (oNode.Attributes["SubCode"] is not null)
                        {
                            string argstrCode5 = oNode.Attributes["SubCode"].Value;
                            oNode.Attributes["SubTitle"].Value = modMain.GetDecodeValue(ref ttHotelSubTitles, ref argstrCode5);
                            oNode.Attributes["SubCode"].Value = argstrCode5;
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
        private StringBuilder sb = new StringBuilder();

        private string mstrResponse = "";
        private int mintProviders = 0;

        private void GotResponse(string Response)
        {
            mstrResponse += Response;
            mintProviders += 1;
        }

        public TripXML tXML;

        #region  Process Service Request All GDS 

        private string ServiceRequest(string strRequest, ttServices ttServiceID)
        {
            string strResponse = "";
            TravelTalkCredential ttCredential = default;
            TripXMLProviderSystems ttProviderSystems = default;
            bool ValidateXSDOut;
            var StartTime = default(DateTime);
            string UUID = "";
            int i;

            try
            {
                StartTime = DateTime.Now;
                _modMain.PreServiceRequestPool(ref strRequest, ref ttCredential, ref ttProviderSystems, StartTime, (int)ttServiceID, Environment.MachineName, ref UUID);
                ValidateXSDOut = Conversions.ToBoolean(TripXMLMain.AppState.Get(sb.Append("XSD").Append(ttCredential.UserID).Append("Out").ToString()));
                sb.Remove(0, sb.Length);

                {
                    ref var withBlock = ref ttCredential;
                    var loopTo = withBlock.Providers.Length - 1;
                    for (i = 0; i <= loopTo; i++)
                    {
                        switch (withBlock.Providers[i].Name.ToLower() ?? "")
                        {
                            case "amadeus":
                                {
                                    try
                                    {
                                        // Dim ttAA As AmadeusAPIAdapter

                                        // ttAA = TripXMLMain.AppState.Get(sb.Append("API").Append(.UserID).Append(.System).Append(.Providers(i).PCC).ToString())
                                        sb.Remove(0, sb.Length);
                                        // If ttAA Is Nothing Then
                                        ttProviderSystems = (TripXMLProviderSystems)TripXMLMain.AppState.Get(sb.Append("PS").Append(ttCredential.Providers[i].Name).Append(ttCredential.UserID).Append(ttCredential.System).Append(ttCredential.Providers[i].PCC).ToString());
                                        sb.Remove(0, sb.Length);

                                        if (ttProviderSystems.AmadeusWS == false)
                                        {
                                            GotResponse(FormatErrorMessage(ttServiceID, sb.Append("Access denied to ").Append(withBlock.Providers[i].Name).Append(" - ").Append(ttCredential.System).Append(" system. Or invalid provider.").ToString(), withBlock.Providers[i].Name));
                                            sb.Remove(0, sb.Length);
                                            break;
                                        }
                                        // End If

                                        if (ttProviderSystems.AmadeusWS == true)
                                        {
                                            if (ttCredential.Providers[i].PCC.Trim().Length > 0)
                                            {
                                                ttProviderSystems.PCC = ttCredential.Providers[i].PCC;
                                            }

                                            ttCredential.Providers[0].Name = "AmadeusWS";

                                            if (ttCredential.System == "Test")
                                            {
                                                ttProviderSystems.URL = "https://test.webservices.amadeus.com";
                                            }
                                            else if (ttCredential.System == "Training")
                                            {
                                                ttProviderSystems.URL = "https://production.webservices.amadeus.com";
                                            }
                                            else
                                            {
                                                ttProviderSystems.URL = "https://production.webservices.amadeus.com";
                                            }

                                            var oAmadeusWS = new cServiceAmadeusWS();
                                            var oThreadAmadeusWS = new Thread(new ThreadStart(oAmadeusWS.SendHotelRequest));
                                            oAmadeusWS.GotResponse += GotResponse;

                                            oAmadeusWS.ServiceID = (int)ttServiceID;
                                            oAmadeusWS.Request = strRequest;
                                            oAmadeusWS.ttProviderSystems = ttProviderSystems;
                                            oAmadeusWS.Version = "";
                                            oThreadAmadeusWS.Start();
                                            ttProviderSystems = default;
                                            // Else
                                            // ttProviderSystems = ttAA.ttProviderSystems

                                            // If ttCredential.Providers(i).PCC.Trim.Length > 0 Then
                                            // ttAA.SourcePCC = ttCredential.Providers(i).PCC
                                            // Else
                                            // ttAA.SourcePCC = ttAA.ttProviderSystems.PCC
                                            // End If

                                            // Dim oAmadeus As New cServiceAmadeus
                                            // Dim oThreadAmadeus As New Thread(New ThreadStart(AddressOf oAmadeus.SendHotelRequest))
                                            // AddHandler oAmadeus.GotResponse, AddressOf GotResponse

                                            // With oAmadeus
                                            // .ServiceID = ttServiceID
                                            // .Request = strRequest
                                            // .ttAA = ttAA
                                            // .Version = ""
                                            // End With

                                            // oThreadAmadeus.Start()
                                            // TripXMLMain.AppState.Set(sb.Append("API").Append(.UserID).Append(.System).ToString(), ttAA)
                                            // sb.Remove(0, sb.Length())
                                        }
                                    }
                                    catch (Exception e)
                                    {
                                        GotResponse(FormatErrorMessage(ttServiceID, e.Message, withBlock.Providers[i].Name));
                                    }

                                    break;
                                }
                            case "apollo":
                            case "galileo":
                                {
                                    try
                                    {
                                        ttProviderSystems = (TripXMLProviderSystems)TripXMLMain.AppState.Get(sb.Append("PS").Append(withBlock.Providers[i].Name).Append(withBlock.UserID).Append(withBlock.System).Append(withBlock.Providers[i].PCC).ToString());
                                        sb.Remove(0, sb.Length);
                                        if (ttProviderSystems.System is null)
                                        {
                                            GotResponse(FormatErrorMessage(ttServiceID, sb.Append("Access denied to ").Append(withBlock.Providers[i].Name).Append(" - ").Append(withBlock.System).Append(" system. Or invalid provider.").ToString(), withBlock.Providers[i].Name));
                                            sb.Remove(0, sb.Length);
                                            break;
                                        }

                                        if (ttCredential.Providers[i].PCC.Trim().Length > 0)
                                        {
                                            ttProviderSystems.PCC = ttCredential.Providers[i].PCC;
                                        }

                                        var oGalileo = new cServiceGalileo();
                                        var oThreadGalileo = new Thread(new ThreadStart(oGalileo.SendHotelRequest));
                                        oGalileo.GotResponse += GotResponse;

                                        oGalileo.ServiceID = (int)ttServiceID;
                                        oGalileo.Request = strRequest;
                                        oGalileo.ProviderSystems = ttProviderSystems;
                                        oGalileo.Version = "";

                                        oThreadGalileo.Start();
                                    }

                                    catch (Exception e)
                                    {
                                        GotResponse(FormatErrorMessage(ttServiceID, e.Message, withBlock.Providers[i].Name));
                                    }

                                    break;
                                }
                            case "sabre":
                                {
                                    try
                                    {
                                        ttProviderSystems = (TripXMLProviderSystems)TripXMLMain.AppState.Get(sb.Append("PS").Append(withBlock.Providers[i].Name).Append(withBlock.UserID).Append(withBlock.System).Append(withBlock.Providers[i].PCC).ToString());
                                        sb.Remove(0, sb.Length);
                                        if (ttProviderSystems.System is null)
                                        {
                                            GotResponse(FormatErrorMessage(ttServiceID, sb.Append("Access denied to ").Append(withBlock.Providers[i].Name).Append(" - ").Append(withBlock.System).Append(" system. Or invalid provider.").ToString(), withBlock.Providers[i].Name));
                                            sb.Remove(0, sb.Length);
                                            break;
                                        }

                                        ttProviderSystems.AAAPCC = withBlock.Providers[i].PCC;

                                        // If ttCredential.Providers(i).PCC.Trim.Length > 0 Then
                                        // ttProviderSystems.PCC = ttCredential.Providers(i).PCC
                                        // End If

                                        var oSabre = new cServiceSabre();
                                        var oThreadSabre = new Thread(new ThreadStart(oSabre.SendHotelRequest));
                                        oSabre.GotResponse += GotResponse;

                                        oSabre.ServiceID = ttServiceID;
                                        oSabre.Request = strRequest;
                                        oSabre.ProviderSystems = ttProviderSystems;
                                        oSabre.Version = "";

                                        oThreadSabre.Start();
                                    }
                                    catch (Exception e)
                                    {
                                        GotResponse(FormatErrorMessage(ttServiceID, e.Message, withBlock.Providers[i].Name));
                                    }

                                    break;
                                }

                            default:
                                {
                                    GotResponse(FormatErrorMessage(ttServiceID, sb.Append("Provider ").Append(withBlock.Providers[i].Name).Append(" Not Currently Supported.").ToString(), withBlock.Providers[i].Name));
                                    sb.Remove(0, sb.Length);
                                    break;
                                }
                        }
                    }
                }

                var StartCounter = DateTime.Now;

                while (mintProviders < ttCredential.Providers.Length)
                {
                    if ((int)Math.Round(DateTime.Now.Subtract(StartCounter).TotalSeconds) > modMain.CPrdTimeOut)
                        break;
                    Thread.Sleep(10);
                }

                if (ttCredential.Providers.Length > 1)
                {
                    strResponse = string.Concat("<SuperRS>", mstrResponse, "</SuperRS>");
                    // Aggregate
                    cAggregation.Aggregate(ttServiceID, XslPath, "", ref strResponse);
                }
                else
                {
                    strResponse = mstrResponse;
                }

                StartCounter = DateTime.Now;
                strResponse = DecodeHotelAvail(strResponse, ttCredential.UserID);
                CoreLib.SendTrace(ttCredential.UserID, "Performance", sb.Append("Decoding = ").Append((int)Math.Round(DateTime.Now.Subtract(StartCounter).TotalMilliseconds)).ToString(), "", UUID);
                sb.Remove(0, sb.Length);

                modMain.PostServiceRequest(ref strResponse, ValidateXSDOut, (int)ttServiceID, ttCredential.UserID);
            }

            catch (Exception ex)
            {
                strResponse = FormatErrorMessage(ttServiceID, ex.Message, "");
            }
            finally
            {
                _modMain.LogResponse(ref strResponse, ref ttCredential, StartTime, (int)ttServiceID, Environment.MachineName, ref UUID);
                if (modCore.Trace)
                    CoreLib.SendTrace(ttCredential.UserID, "wsHotelAvail", "============= OTA Response ============= ", strResponse, UUID);
            }
            sb = null;
            return strResponse;

        }

        #endregion

        #region  Web Methods 
        public wmHotelAvailOut.OTA_HotelAvailRS wmHotelAvail(wmHotelAvailIn.OTA_HotelAvailRQ OTA_HotelAvailRQ)
        {
            string xmlMessage = "";
            wmHotelAvailOut.OTA_HotelAvailRS oHotelAvailRS = null;
            XmlSerializer oSerializer = null;
            System.IO.StringWriter oWriter = null;
            System.IO.StringReader oReader = null;
            oSerializer = new XmlSerializer(typeof(wmHotelAvailIn.OTA_HotelAvailRQ));
            oWriter = new System.IO.StringWriter(new StringBuilder());
            oSerializer.Serialize(oWriter, OTA_HotelAvailRQ);
            xmlMessage = oWriter.ToString();
            xmlMessage = xmlMessage.Replace(" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", "");

            xmlMessage = ServiceRequest(xmlMessage, ttServices.HotelAvail);

            try
            {
                oSerializer = null;
                oSerializer = new XmlSerializer(@type: typeof(wmHotelAvailOut.OTA_HotelAvailRS));
                oReader = new System.IO.StringReader(xmlMessage);
                oHotelAvailRS = (wmHotelAvailOut.OTA_HotelAvailRS)oSerializer.Deserialize(oReader);
            }
            catch (Exception ex)
            {
                CoreLib.SendTrace("", "wsHotelAvail", "Error Deserialing OTA Response", ex.Message, string.Empty);
                xmlMessage = "<OTA_HotelAvailRS Version=\"1.001\"><Errors><Error>" + ex.InnerException.ToString() + "</Error></Errors></OTA_HotelAvailRS>";
                oReader = new System.IO.StringReader(xmlMessage);
                oHotelAvailRS = (wmHotelAvailOut.OTA_HotelAvailRS)oSerializer.Deserialize(oReader);
            }

            return oHotelAvailRS;

        }
        public string wmHotelAvailXml(string xmlRequest)
        {
            return ServiceRequest(xmlRequest, ttServices.HotelAvail);
        }

        #endregion

    }

}