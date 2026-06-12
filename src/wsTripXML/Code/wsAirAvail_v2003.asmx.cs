using System;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Web.Services;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.VisualBasic.CompilerServices;
using TripXMLMain;
using static TripXMLMain.modCore;
using static TripXMLTools.TripXMLLoad;

namespace wsTripXML.wsTravelTalk
{


    [WebService(Namespace = "http://tripxml.downtowntravel.com/tripxml/wsAirAvail", Name = "wsAirAvail_v2003", Description = "A TripXML Web Service to Process Air Availability Messages Request. OTA version 2.003.")]
    public class wsAirAvail_v2003 : WebService
    {

        #region  Web Services Designer Generated Code 

        public wsAirAvail_v2003() : base()
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

        private string mstrResponse = "";
        private int mintProviders = 0;
        private StringBuilder sb = new StringBuilder();

        private void GotResponse(string Response)
        {
            mstrResponse += Response;
            mintProviders += 1;
        }

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

                var argoApp = Application;
                modMain.PreServiceRequestPool(ref strRequest, ref argoApp, ref ttCredential, ref ttProviderSystems, StartTime, (int)ttServiceID, Server.MachineName, ref UUID, "2005A");
                ValidateXSDOut = Conversions.ToBoolean(Application.Get(sb.Append("XSD").Append(ttCredential.UserID).Append("Out").ToString()));
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
                                    break;
                                }
                            // Try
                            // Dim ttAA As AmadeusAPIAdapter

                            // ttAA = Application.Get(sb.Append("API").Append(.UserID).Append(.System).Append(.Providers(i).PCC).ToString())
                            // sb.Remove(0, sb.Length())
                            // If ttAA Is Nothing Then
                            // GotResponse(FormatErrorMessage(ttServiceID, sb.Append("Access denied to ").Append(.Providers(i).Name).Append(" - ").Append(ttCredential.System).Append(" system. Or invalid provider.").ToString(), .Providers(i).Name, "", False, "1.004"))
                            // sb.Remove(0, sb.Length())
                            // Exit Select
                            // End If

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
                            // Version = "2005A"
                            // End With
                            // oThreadAmadeus.Start()
                            // Application.Set(sb.Append("API").Append(.UserID).Append(.System).ToString(), ttAA)
                            // sb.Remove(0, sb.Length())
                            // Catch e As Exception
                            // GotResponse(FormatErrorMessage(ttServiceID, e.Message, .Providers(i).Name))
                            // End Try
                            case "apollo":
                            case "galileo":
                                {
                                    try
                                    {
                                        // ttProviderSystems = Application.Get("PS").Append(.Providers(i).Name).Append(.UserID).Append(.System).Append(.Providers(i).PCC)
                                        var cacheKey = new StringBuilder();
                                        cacheKey.Append("PS").Append(withBlock.Providers[i].Name).Append(withBlock.UserID).Append(withBlock.System).Append(withBlock.Providers[i].PCC);
                                        ttProviderSystems = (TripXMLProviderSystems)Application.Get(cacheKey.ToString());
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
                                        oGalileo.Version = "2005A";
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
                                        ttProviderSystems = (TripXMLProviderSystems)Application.Get(sb.Append("PS").Append(withBlock.Providers[i].Name).Append(withBlock.UserID).Append(withBlock.System).Append(withBlock.Providers[i].PCC).ToString());
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
                                        oSabre.Version = "2005A";
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
                                    GotResponse(FormatErrorMessage(ttServiceID, sb.Append("Provider ").Append(withBlock.Providers[i].Name).Append(" Not Currently Supported.").ToString(), withBlock.Providers[i].Name, "", false, "1.004"));
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
                CoreLib.SendTrace(ttCredential.UserID, "Performance", sb.Append("Decoding = ").Append((int)Math.Round(DateTime.Now.Subtract(StartCounter).TotalMilliseconds)).ToString(), "", UUID);
                sb.Remove(0, sb.Length);

                modMain.PostServiceRequest(ref strResponse, ValidateXSDOut, (int)ttServiceID, ttCredential.UserID, "2005A");
            }

            catch (Exception ex)
            {
                strResponse = FormatErrorMessage(ttServiceID, ex.Message, "1.004");
            }
            finally
            {
                modMain.LogResponse(ref strResponse, ref ttCredential, StartTime, (int)ttServiceID, Server.MachineName, ref UUID);
                if (modCore.Trace)
                    CoreLib.SendTrace(ttCredential.UserID, "wsAirAvail_v2003", "============= OTA Response ============= ", strResponse, UUID);
            }

            return strResponse;
            sb = null;
        }

        #endregion

        #region  Web Methods 

        [WebMethod(Description = "Process Air Availability Messages Request. OTA version 2.003.")]
        public wmAirAvail2005AOut.OTA_AirAvailRS wmAirAvail(wmAirAvail2005AIn.OTA_AirAvailRQ OTA_AirAvailRQ)
        {
            string xmlMessage = "";
            wmAirAvail2005AOut.OTA_AirAvailRS oAirAvailRS = null;
            XmlSerializer oSerializer = null;
            System.IO.StringWriter oWriter = null;
            System.IO.StringReader oReader = null;

            if (OTA_AirAvailRQ.Version == "2.003")
            {

                oSerializer = new XmlSerializer(typeof(wmAirAvail2005AIn.OTA_AirAvailRQ));
                oWriter = new System.IO.StringWriter(new StringBuilder());
                oSerializer.Serialize(oWriter, OTA_AirAvailRQ);
                xmlMessage = oWriter.ToString();
                xmlMessage = xmlMessage.Replace(" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", "");

                xmlMessage = ServiceRequest(xmlMessage, ttServices.AirAvail);
            }

            else
            {
                xmlMessage = "<OTA_AirAvailRS Version=\"1.004\"><Errors>";
                xmlMessage += "<Error Type=\"E\">Invalid OTA version.</Error></Errors></OTA_AirAvailRS>";
            }

            try
            {
                oSerializer = null;
                oSerializer = new XmlSerializer(@type: typeof(wmAirAvail2005AOut.OTA_AirAvailRS));
                oReader = new System.IO.StringReader(xmlMessage);
                oAirAvailRS = (wmAirAvail2005AOut.OTA_AirAvailRS)oSerializer.Deserialize(oReader);
            }
            catch (Exception ex)
            {
                CoreLib.SendTrace("", "wsAirAvail_v2003", "Error Deserialing OTA Response", ex.Message, string.Empty);
            }

            return oAirAvailRS;

        }

        [WebMethod(Description = "Process Air Availability Xml Messages Request. OTA version 2.003.")]
        public string wmAirAvailXml(string xmlRequest)
        {
            if (xmlRequest.IndexOf("Version=\"2.003\"") > 0)
            {
                return ServiceRequest(xmlRequest, ttServices.AirAvail);
            }
            else
            {
                xmlRequest = "<OTA_AirAvailRS Version='1.004'><Errors>";
                xmlRequest += "<Error Type=\"E\">Invalid OTA version.</Error></Errors></OTA_AirAvailRS>";
                return xmlRequest;
            }
        }

        #endregion

    }

}