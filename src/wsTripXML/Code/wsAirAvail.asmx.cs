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
using static TripXMLTools.TripXMLLoad;

namespace wsTripXML.wsTravelTalk
{
    public partial class wsAirAvail
    {

        private readonly modMain _modMain;

        public wsAirAvail(modMain modMain)
        {
            _modMain = modMain;
        }

        #region  Decode Functions 

        private string DecodeAirAvail(string strResponse, string UserID)
        {
            XmlDocument oDoc = null;
            XmlElement oRoot = null;
            DataView ttAirports;
            DataView ttAirlines;
            DataView ttEquipments;

            try
            {

                oDoc = new XmlDocument();
                oDoc.LoadXml(strResponse);
                oRoot = oDoc.DocumentElement;

                ttAirports = (DataView)TripXMLMain.AppState.Get("ttAirports");
                ttAirlines = (DataView)TripXMLMain.AppState.Get("ttAirlines");
                ttEquipments = (DataView)TripXMLMain.AppState.Get("ttEquipments");

                foreach (XmlNode oNode in oRoot.SelectNodes("OriginDestinationOptions/OriginDestinationOption/FlightSegment"))
                {
                    // *******************
                    // Decode Airports   *
                    // *******************
                    oNode.SelectSingleNode("DepartureAirport").InnerText = DecodeValue((DecodingType)Conversions.ToInteger(oNode.SelectSingleNode("DepartureAirport").Attributes["LocationCode"].Value), ((int)DecodingType.Airport).ToString());
                    oNode.SelectSingleNode("ArrivalAirport").InnerText = DecodeValue((DecodingType)Conversions.ToInteger(oNode.SelectSingleNode("ArrivalAirport").Attributes["LocationCode"].Value), ((int)DecodingType.Airport).ToString());

                    // *******************
                    // Decode Airlines   *
                    // *******************
                    if (oNode.SelectSingleNode("OperatingAirline") is not null)
                    {
                        oNode.SelectSingleNode("OperatingAirline").InnerText = DecodeValue((DecodingType)Conversions.ToInteger(oNode.SelectSingleNode("OperatingAirline").Attributes["Code"].Value), ((int)DecodingType.Airline).ToString());
                    }
                    if (oNode.SelectSingleNode("MarketingAirline") is not null)
                    {
                        oNode.SelectSingleNode("MarketingAirline").InnerText = DecodeValue((DecodingType)Conversions.ToInteger(oNode.SelectSingleNode("MarketingAirline").Attributes["Code"].Value), ((int)DecodingType.Airline).ToString());
                    }

                    // *******************
                    // Decode Equipments *
                    // *******************
                    if (oNode.SelectSingleNode("Equipment") is not null)
                    {
                        oNode.SelectSingleNode("Equipment").InnerText = DecodeValue((DecodingType)Conversions.ToInteger(oNode.SelectSingleNode("Equipment").Attributes["AirEquipType"].Value), ((int)DecodingType.Equipment).ToString());
                    }
                }

                strResponse = oDoc.OuterXml;
            }

            catch (Exception ex)
            {
                CoreLib.SendTrace(UserID, "wsAirServices", "Error *** Decoding AirAvail Response", ex.Message, string.Empty);
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
                                            var oThreadAmadeusWS = new Thread(new ThreadStart(oAmadeusWS.SendAirRequest));
                                            oAmadeusWS.GotResponse += GotResponse;

                                            oAmadeusWS.ServiceID = (int)ttServiceID;
                                            oAmadeusWS.Request = strRequest;
                                            oAmadeusWS.ttProviderSystems = ttProviderSystems;
                                            oAmadeusWS.Version = "";
                                            oThreadAmadeusWS.Start();
                                            // Else
                                            // If ttCredential.Providers(i).PCC.Trim.Length > 0 Then
                                            // ttAA.SourcePCC = ttCredential.Providers(i).PCC
                                            // End If

                                            // Dim oAmadeus As New cServiceAmadeus
                                            // Dim oThreadAmadeus As New Thread(New ThreadStart(AddressOf oAmadeus.SendAirRequest))
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
                                        var oThreadGalileo = new Thread(new ThreadStart(oGalileo.SendAirRequest));
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
                                        if (ttProviderSystems.System is null)
                                        {
                                            FormatErrorMessage(ttServiceID, sb.Append("Access denied to ").Append(ttCredential.Providers[0].Name).Append(" - ").Append(ttCredential.System).Append(" system. Or invalid provider.").ToString(), ttCredential.Providers[0].Name);
                                            sb.Remove(0, sb.Length);
                                            break;
                                        }

                                        ttProviderSystems.AAAPCC = ttCredential.Providers[0].PCC;
                                        // If ttCredential.Providers(i).PCC.Trim.Length > 0 Then
                                        // ttProviderSystems.PCC = ttCredential.Providers(i).PCC
                                        // End If

                                        var oSabre = new cServiceSabre();
                                        var oThreadSabre = new Thread(new ThreadStart(oSabre.SendAirRequest));
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
                strResponse = DecodeAirAvail(strResponse, ttCredential.UserID);
                CoreLib.SendTrace(ttCredential.UserID, "Performance", sb.Append("Decoding = ").Append((int)Math.Round(DateTime.Now.Subtract(StartCounter).TotalMilliseconds)).ToString(), "", ttProviderSystems.LogUUID);
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
                    CoreLib.SendTrace(ttCredential.UserID, "wsAirAvail", "============= OTA Response ============= ", strResponse, ttProviderSystems.LogUUID);
            }

            return strResponse;
            sb = null;
        }

        #endregion

        #region  Web Methods 
        public wmAirAvailOut.OTA_AirAvailRS wmAirAvail(wmAirAvailIn.OTA_AirAvailRQ OTA_AirAvailRQ)
        {
            string xmlMessage = "";
            wmAirAvailOut.OTA_AirAvailRS oAirAvailRS = null;
            XmlSerializer oSerializer;
            System.IO.StringWriter oWriter;
            System.IO.StringReader oReader;

            oSerializer = new XmlSerializer(typeof(wmAirAvailIn.OTA_AirAvailRQ));
            oWriter = new System.IO.StringWriter(new StringBuilder());
            oSerializer.Serialize(oWriter, OTA_AirAvailRQ);
            xmlMessage = oWriter.ToString();
            xmlMessage = xmlMessage.Replace(" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", "");

            xmlMessage = ServiceRequest(xmlMessage, ttServices.AirAvail);

            try
            {
                oSerializer = null;
                oSerializer = new XmlSerializer(@type: typeof(wmAirAvailOut.OTA_AirAvailRS));
                oReader = new System.IO.StringReader(xmlMessage);
                oAirAvailRS = (wmAirAvailOut.OTA_AirAvailRS)oSerializer.Deserialize(oReader);
            }
            catch (Exception ex)
            {
                CoreLib.SendTrace("", "wsAirAvail", "Error Deserialing OTA Response", ex.Message, string.Empty);
            }

            return oAirAvailRS;

        }
        public string wmAirAvailXml(string xmlRequest)
        {
            return ServiceRequest(xmlRequest, ttServices.AirAvail);
        }
        public string wmAirAvailJson(string xmlRequest)
        {

            string a = ServiceRequest(xmlRequest, ttServices.AirAvail);
            return ToJson(a);

        }

        private string ToJson(string obj)
        {

            var doc = new XmlDocument();
            doc.LoadXml(obj);

            return Newtonsoft.Json.JsonConvert.SerializeXmlNode(doc);
        }



        #endregion

    }

}