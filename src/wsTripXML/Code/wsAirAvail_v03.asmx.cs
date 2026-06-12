using System;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using TripXMLMain;
using static TripXMLMain.modCore;
using static TripXMLTools.TripXMLLoad;

namespace wsTripXML.wsTravelTalk
{
    public partial class wsAirAvail_v03
    {

        private readonly modMain _modMain;

        public wsAirAvail_v03(modMain modMain)
        {
            _modMain = modMain;
        }

        #region  Decode Functions 

        private string DecodeAirAvail(string strResponse, string UserID)
        {
            XmlDocument oDoc = null;
            XmlElement oRoot = null;

            try
            {

                oDoc = new XmlDocument();
                oDoc.LoadXml(strResponse);
                oRoot = oDoc.DocumentElement;

                foreach (XmlNode oNode in oRoot.SelectNodes("OriginDestinationOptions/OriginDestinationOption/FlightSegment"))
                {
                    // *******************
                    // Decode Airports   *
                    // *******************
                    oNode.SelectSingleNode("DepartureAirport").InnerText = DecodeValue(DecodingType.Airport, oNode.SelectSingleNode("DepartureAirport").Attributes["LocationCode"].Value);
                    oNode.SelectSingleNode("ArrivalAirport").InnerText = DecodeValue(DecodingType.Airport, oNode.SelectSingleNode("ArrivalAirport").Attributes["LocationCode"].Value);

                    // *******************
                    // Decode Airlines   *
                    // *******************
                    if (oNode.SelectSingleNode("OperatingAirline") is not null)
                    {
                        oNode.SelectSingleNode("OperatingAirline").InnerText = DecodeValue(DecodingType.Airline, oNode.SelectSingleNode("OperatingAirline").Attributes["Code"].Value);
                    }
                    if (oNode.SelectSingleNode("MarketingAirline") is not null)
                    {
                        oNode.SelectSingleNode("MarketingAirline").InnerText = DecodeValue(DecodingType.Airline, oNode.SelectSingleNode("MarketingAirline").Attributes["Code"].Value);
                    }

                    // *******************
                    // Decode Equipments *
                    // *******************
                    if (oNode.SelectSingleNode("Equipment") is not null)
                    {
                        oNode.SelectSingleNode("Equipment").InnerText = DecodeValue(DecodingType.Equipment, oNode.SelectSingleNode("Equipment").Attributes["AirEquipType"].Value);
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
            XmlDocument oDoc = null;
            XmlElement oRoot = null;
            XmlNode oNode = null;
            int sDate;
            string oDate;
            DateTime oModifyDate;
            var LastMessage = DateTime.Now;
            StringBuilder sb1 = null;
            int CountMsgs = 0;
            int Pages = 0;

            try
            {
                sb1 = new StringBuilder();

                StartTime = DateTime.Now;
                oModifyDate = LastMessage;
                oDoc = new XmlDocument();
                oDoc.LoadXml(strRequest);
                oRoot = oDoc.DocumentElement;



                oNode = oRoot.ChildNodes[1].ChildNodes[1];
                string strD = oNode.InnerXml.ToString();
                sDate = int.Parse(oNode.InnerXml.ToString());
                Pages = int.Parse(oNode.Attributes["Pages"].Value);

                oNode = oRoot.ChildNodes[1].ChildNodes[0];
                oDate = oNode.InnerXml.ToString();


                if (sDate > 15)
                {
                    throw new Exception("Invalid request: Could only request the GDS less than 15 period of days.");
                }
                else if (sDate * Pages > 45)
                {
                    throw new Exception("Invalid request: Could only request the GDS less than 45 number of searches.");
                }


                // Dim nDate As String = oDate.Substring(0, oDate.IndexOf("T"))
                oModifyDate = DateTime.Parse(oDate.Substring(0, oDate.IndexOf("T")));
                // Dim oTime As String = oDate.Substring(oDate.IndexOf("T") + 1, 8).ToString
                string oTime = oDate.Substring(oDate.IndexOf("T"), oDate.Length - oDate.IndexOf("T")).ToString();
                _modMain.PreServiceRequestPool(ref strRequest, ref ttCredential, ref ttProviderSystems, StartTime, (int)ttServiceID, Environment.MachineName, ref UUID);
                ValidateXSDOut = Conversions.ToBoolean(TripXMLMain.AppState.Get(sb.Append("XSD").Append(ttCredential.UserID).Append("Out").ToString()));
                sb.Remove(0, sb.Length);



                {
                    ref var withBlock = ref ttCredential;
                    var loopTo = withBlock.Providers.Length - 1;
                    for (i = 0; i <= loopTo; i++)
                    {

                        for (int j = 0, loopTo1 = sDate - 1; j <= loopTo1; j++)
                        {

                            if (CountMsgs != 0)
                            {
                                oTime = "T00:00:00";
                                oModifyDate = DateAndTime.DateAdd(DateInterval.Day, 1d, oModifyDate);
                                string tdate = Strings.Format(oModifyDate, "yyyy-MM-dd");

                                sb1.Append(tdate).Append(oTime);
                                string fDate = sb1.ToString();

                                sb1.Remove(0, sb1.Length);

                                oNode.InnerXml = fDate;

                                strRequest = sb1.Append(strRequest.Substring(0, strRequest.IndexOf("<OTA_AirAvailRQ>"))).Append(oRoot.OuterXml).ToString();

                                sb1.Remove(0, sb1.Length);

                            }

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

                                                // ttCredential.Providers(0).Name = "AmadeusWS"

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
                                                oAmadeusWS.Version = "v03";

                                                int waitTime = 0;
                                                var r = new Random();
                                                waitTime = r.Next(100, 300);
                                                Thread.Sleep(waitTime);

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
                                                // .Version = "v03"
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
                                            oGalileo.Version = "v03";
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
                                            var oThreadSabre = new Thread(new ThreadStart(oSabre.SendAirRequest));
                                            oSabre.GotResponse += GotResponse;

                                            oSabre.ServiceID = ttServiceID;
                                            oSabre.Request = strRequest;
                                            oSabre.ProviderSystems = ttProviderSystems;
                                            oSabre.Version = "v03";
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
                            CountMsgs += 1;
                        }
                    }
                }

                var StartCounter = DateTime.Now;

                // Do While mintProviders < ttCredential.Providers.Length
                while (mintProviders < CountMsgs)
                {
                    if ((int)Math.Round(DateTime.Now.Subtract(StartCounter).TotalSeconds) > modMain.CPrdTimeOut)
                        break;
                    Thread.Sleep(20);
                }

                // If ttCredential.Providers.Length > 1 Then
                if (CountMsgs > 1)
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
                    CoreLib.SendTrace(ttCredential.UserID, "wsAirAvail_v03", "============= OTA Response ============= ", strResponse, UUID);
            }

            return strResponse;
            sb = null;
        }

        #endregion

        #region  Web Methods 
        public wmAirAvailOut.OTA_AirAvailRS wmAirAvail(wmAirAvailIn_v03.OTA_AirAvailRQ OTA_AirAvailRQ)
        {
            string xmlMessage = "";
            wmAirAvailOut.OTA_AirAvailRS oAirAvailRS = null;
            XmlSerializer oSerializer;
            System.IO.StringWriter oWriter;
            System.IO.StringReader oReader;

            oSerializer = new XmlSerializer(typeof(wmAirAvailIn_v03.OTA_AirAvailRQ));
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
                CoreLib.SendTrace("", "wsAirAvail_v03", "Error Deserialing OTA Response", ex.Message, string.Empty);
            }

            return oAirAvailRS;

        }
        public string wmAirAvailXml(string xmlRequest)
        {
            return ServiceRequest(xmlRequest, ttServices.AirAvail);
        }

        #endregion

    }

}