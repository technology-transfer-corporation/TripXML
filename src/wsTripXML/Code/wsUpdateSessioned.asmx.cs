using System;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Web.Services;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using TripXMLMain;
using static TripXMLMain.modCore;
using static TripXMLTools.TripXMLLoad;

namespace wsTripXML.wsTravelTalk
{

    [System.Web.Services.Protocols.SoapDocumentService(RoutingStyle = System.Web.Services.Protocols.SoapServiceRoutingStyle.RequestElement)]
    [WebService(Namespace = "http://tripxml.downtowntravel.com/tripxml/wsUpdateSessioned", Name = "wsUpdateSessioned", Description = "A TripXML Web Service to Process UpdateSessioned Messages Request.")]
    public class wsUpdateSessioned : WebService
    {

        #region  Web Services Designer Generated Code 

        public wsUpdateSessioned() : base()
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
                                oNode.SelectSingleNode("DepartureAirport").InnerText = DecodeValue(DecodingType.Airport, oNode.SelectSingleNode("DepartureAirport").Attributes["LocationCode"].Value);
                                // GetDecodeValue(ttAirports, oNode.SelectSingleNode("DepartureAirport").Attributes("LocationCode").Value)
                            }
                            if (oNode.SelectSingleNode("ArrivalAirport") is not null)
                            {
                                oNode.SelectSingleNode("ArrivalAirport").InnerText = DecodeValue(DecodingType.Airport, oNode.SelectSingleNode("ArrivalAirport").Attributes["LocationCode"].Value);
                                // GetDecodeValue(ttAirports, oNode.SelectSingleNode("ArrivalAirport").Attributes("LocationCode").Value)
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
                                    // GetDecodeValue(ttAirlines, oNode.SelectSingleNode("OperatingAirline").Attributes("Code").Value)
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
                                    attCode.Value = EncodeValue(DecodingType.Airline, oNode.SelectSingleNode("OperatingAirline").InnerText);
                                    // GetEncodeValue(ttAirlines, oNode.SelectSingleNode("OperatingAirline").InnerText)

                                    if (!string.IsNullOrEmpty(attCode.Value))
                                    {
                                        oNode.SelectSingleNode("OperatingAirline").Attributes.Append(attCode);
                                        oNode.SelectSingleNode("OperatingAirline").InnerText = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(oNode.SelectSingleNode("OperatingAirline").InnerText.ToLower());
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
                            // If Not oNode.SelectSingleNode("OperatingAirline") Is Nothing Then
                            // If Not oNode.SelectSingleNode("OperatingAirline").Attributes("Code") Is Nothing Then
                            // If oNode.SelectSingleNode("OperatingAirline").Attributes("Code").Value <> "" Then
                            // oNode.SelectSingleNode("OperatingAirline").InnerText = DecodeValue(DecodingType.Airline, oNode.SelectSingleNode("OperatingAirline").Attributes("Code").Value)
                            // ElseIf Not oNode.SelectSingleNode("OperatingAirline") Is Nothing Then
                            // Dim attCode As XmlAttribute
                            // attCode = oDoc.CreateAttribute("Code")
                            // attCode.Value = TripXMLLoad.EncodeValue(TripXMLLoad.DecodingType.Airline, oNode.SelectSingleNode("OperatingAirline").InnerText)
                            // 'GetEncodeValue(ttAirlinesNames, oNode.SelectSingleNode("OperatingAirline").InnerText)
                            // oNode.SelectSingleNode("OperatingAirline").Attributes.Append(attCode)

                            // oNode.SelectSingleNode("OperatingAirline").InnerText = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(oNode.SelectSingleNode("OperatingAirline").InnerText.ToLower())
                            // End If
                            // 'oNode.SelectSingleNode("OperatingAirline").InnerText = DecodeValue(DecodingType.Airline, oNode.SelectSingleNode("OperatingAirline").Attributes("Code").Value)
                            // Else
                            // If Not oNode.SelectSingleNode("OperatingAirline") Is Nothing Then
                            // Dim attCode As XmlAttribute
                            // attCode = oDoc.CreateAttribute("Code")
                            // If Not oNode.SelectSingleNode("OperatingAirline").Attributes("Code") Is Nothing Then
                            // attCode.Value = TripXMLLoad.DecodeValue(TripXMLLoad.DecodingType.Airline, oNode.SelectSingleNode("OperatingAirline").Attributes("Code").Value)
                            // 'GetEncodeValue(ttAirlines, oNode.SelectSingleNode("OperatingAirline").InnerText)

                            // If Not String.IsNullOrEmpty(attCode.Value) Then
                            // oNode.SelectSingleNode("OperatingAirline").Attributes.Append(attCode)
                            // oNode.SelectSingleNode("OperatingAirline").InnerText = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(oNode.SelectSingleNode("OperatingAirline").InnerText.ToLower())
                            // End If
                            // Else
                            // attCode.Value = TripXMLLoad.EncodeValue(TripXMLLoad.DecodingType.Airline, oNode.SelectSingleNode("OperatingAirline").InnerText)

                            // If Not String.IsNullOrEmpty(attCode.Value) Then
                            // oNode.SelectSingleNode("OperatingAirline").Attributes.Append(attCode)
                            // oNode.SelectSingleNode("OperatingAirline").InnerText = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(oNode.SelectSingleNode("OperatingAirline").InnerText.ToLower())
                            // End If
                            // End If
                            // End If
                            // End If
                            // ElseIf Not oNode.SelectSingleNode("OperatingAirline") Is Nothing Then
                            // Dim attCode As XmlAttribute
                            // attCode = oDoc.CreateAttribute("Code")
                            // attCode.Value = TripXMLLoad.EncodeValue(TripXMLLoad.DecodingType.Airline, oNode.SelectSingleNode("OperatingAirline").InnerText)
                            // 'GetEncodeValue(ttAirlinesNames, oNode.SelectSingleNode("OperatingAirline").InnerText)
                            // oNode.SelectSingleNode("OperatingAirline").Attributes.Append(attCode)

                            // oNode.SelectSingleNode("OperatingAirline").InnerText = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(oNode.SelectSingleNode("OperatingAirline").InnerText.ToLower())
                            // End If

                            if (oNode.SelectSingleNode("MarketingAirline") is not null)
                            {
                                oNode.SelectSingleNode("MarketingAirline").InnerText = DecodeValue(DecodingType.Airline, oNode.SelectSingleNode("MarketingAirline").Attributes["Code"].Value);
                                // GetDecodeValue(ttAirlines, oNode.SelectSingleNode("MarketingAirline").Attributes("Code").Value)
                            }

                            // *******************
                            // Decode Equipments   *
                            // *******************
                            if (oNode.SelectSingleNode("Equipment") is not null)
                            {
                                if (oNode.SelectSingleNode("Equipment").Attributes["AirEquipType"] is not null)
                                {
                                    oNode.SelectSingleNode("Equipment").InnerText = DecodeValue(DecodingType.Equipment, oNode.SelectSingleNode("Equipment").Attributes["AirEquipType"].Value);
                                    // GetDecodeValue(ttEquipments, oNode.SelectSingleNode("Equipment").Attributes("AirEquipType").Value)
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

        private StringBuilder sb = new StringBuilder();

        private string mstrResponse = "";
        private int mintProviders = 0;
        private string _recordLocator = "";
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
            bool validateXSDOut;
            var startTime = default(DateTime);
            string uuID = "";
            // Dim conversationID As String = ""

            try
            {
                startTime = DateTime.Now;

                var argoApp = Application;
                modMain.PreServiceRequest(ref strRequest, ref argoApp, ref ttCredential, ref ttProviderSystems, startTime, (int)ttServiceID, Server.MachineName, ref uuID);
                validateXSDOut = Conversions.ToBoolean(Application.Get(sb.Append("XSD").Append(ttCredential.UserID).Append("Out").ToString()));
                sb.Remove(0, sb.Length);

                {
                    ref var withBlock = ref ttCredential;
                    switch (withBlock.Providers[0].Name.ToLower() ?? "")
                    {
                        case "amadeusws":
                            {
                                strResponse = modMain.SendTravelRequestAmadeusWS(ttServiceID, ref ttCredential, ref ttProviderSystems, ref strRequest);
                                break;
                            }


                        case "apollo":
                        case "galileo":
                            {
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

                                // Dim ttDefProvider As New TripXMLProviderSystems()
                                // Dim sTPRequest As String = CreatePNRRead(strRequest)
                                // PreServiceRequest(sTPRequest, Application, ttCredential, ttDefProvider, startTime, ttServiceID, Server.MachineName, uuID, "", True)
                                // strResponse = SendPNRRequestTravelPort(ttServiceID, ttCredential, ttDefProvider, sTPRequest, "v03")

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
                strResponse = DecodePNRRead(strResponse, ttCredential.UserID, uuID);
                // DecodeUpdateSessioned(strResponse) Not Implemented.

                modMain.PostServiceRequest(ref strResponse, validateXSDOut, (int)ttServiceID, ttCredential.UserID);
            }

            catch (Exception ex)
            {
                strResponse = FormatErrorMessage(ttServiceID, ex.Message, ttCredential.Providers[0].Name);
            }
            finally
            {
                modMain.LogResponse(ref strResponse, ref ttCredential, startTime, (int)ttServiceID, Server.MachineName, ref uuID);
                if (modCore.Trace)
                    CoreLib.SendTrace(ttCredential.UserID, "wsUpdateSessioned", "============= OTA Response ============= ", strResponse, ttProviderSystems.LogUUID);
            }
            sb = null;
            return strResponse;

        }

        #endregion

        #region  Web Methods 

        [CompressionExtension.CompressionExtension()]
        [WebMethod(Description = "Process UpdateSessioned Info Messages Request.")]
        [System.Web.Services.Protocols.SoapHeader("tXML")]
        public wmTravelItineraryOut_v03.OTA_TravelItineraryRS wmUpdateSessioned(wmUpdateSessionedIn.OTA_UpdateSessionedRQ OTA_UpdateSessionedRQ)
        {
            string xmlMessage = string.Empty;
            wmTravelItineraryOut_v03.OTA_TravelItineraryRS otaUpdateSessionedRS;
            var oSerializer = new XmlSerializer(typeof(wmUpdateSessionedIn.OTA_UpdateSessionedRQ));
            System.IO.StringWriter oWriter;
            System.IO.StringReader oReader;

            try
            {
                oWriter = new System.IO.StringWriter(new StringBuilder());

                // *************************************
                // * Get PNR Modify XML Request Msg    * 
                // *************************************
                oSerializer.Serialize(oWriter, OTA_UpdateSessionedRQ);
                xmlMessage = oWriter.ToString();
                xmlMessage = xmlMessage.Replace(" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", "");
                xmlMessage = xmlMessage.Replace(" xmlns=\"http://tripxml.downtowntravel.com/tripxml/wsUpdateSessioned\"", "");
                xmlMessage = xmlMessage.Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", "");
                xmlMessage = xmlMessage.Replace(Constants.vbCrLf, "");
                xmlMessage = xmlMessage.Replace("\"", "'");

                xmlMessage = ServiceRequest(xmlMessage, ttServices.UpdateSessioned);

                oSerializer = null;
                oSerializer = new XmlSerializer(@type: typeof(wmTravelItineraryOut_v03.OTA_TravelItineraryRS));
                oReader = new System.IO.StringReader(xmlMessage);
                otaUpdateSessionedRS = (wmTravelItineraryOut_v03.OTA_TravelItineraryRS)oSerializer.Deserialize(oReader);
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

                string itinRefXmlList = string.Empty;
                string custInfoXmlList = string.Empty;
                string tpaInfoXmlList = string.Empty;
                if (oRoot.SelectSingleNode("TravelItinerary") is null)
                {
                    itinRefXmlList = oRoot.SelectSingleNode("TravelItinerary/ItineraryRef").OuterXml;
                    custInfoXmlList = oRoot.SelectSingleNode("TravelItinerary/CustomerInfos").OuterXml;
                    tpaInfoXmlList = oRoot.SelectSingleNode("TravelItinerary/TPA_Extensions").OuterXml;
                }

                string errMessage = $"<Errors><Error>{ex.InnerException?.Message}</Error><Error>{ex.Message}</Error></Errors>";

                xmlMessage = $"<OTA_TravelItineraryRS Version=\"v03\" xmlns:stl=\"http://services.sabre.com/STL/v01\">{errMessage}<TravelItinerary>{itinRefXmlList}{custInfoXmlList}<ItineraryInfo></ItineraryInfo>{tpaInfoXmlList}</TravelItinerary>{sessionID}</OTA_TravelItineraryRS>";

                oReader = new System.IO.StringReader(xmlMessage);
                otaUpdateSessionedRS = (wmTravelItineraryOut_v03.OTA_TravelItineraryRS)oSerializer.Deserialize(oReader);

            }

            return otaUpdateSessionedRS;

        }

        [WebMethod(Description = "Process UpdateSessioned Info Xml Messages Request.")]
        public string wmUpdateSessionedXml(string xmlRequest)
        {
            return ServiceRequest(xmlRequest, ttServices.UpdateSessioned);
        }

        #endregion

    }

}