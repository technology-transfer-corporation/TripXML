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
    public partial class wsSalesReport
    {
        public TripXML tXML;

        private readonly modMain _modMain;

        public wsSalesReport(modMain modMain)
        {
            _modMain = modMain;
        }

        #region  Decode Function 

        // Not Implemented

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

                switch (ttCredential.Providers[0].Name ?? "")
                {
                    case "Amadeus":
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

                    // strResponse = SendOtherRequestAmadeus(ttServiceID, ttCredential, ttAA, strRequest)
                    // TripXMLMain.AppState.Set(sb.Append("API").Append(ttCredential.UserID).Append(ttCredential.System).ToString(), ttAA)
                    // sb.Remove(0, sb.Length())

                    case "AmadeusWS":
                        {
                            strResponse = modMain.SendOtherRequestAmadeusWS(ttServiceID, ref ttCredential, ref ttProviderSystems, ref strRequest, "v03");
                            break;
                        }
                    case "Sabre":
                        {
                            strResponse = modMain.SendOtherRequestSabre(ttServiceID, ref ttCredential, ref ttProviderSystems, ref strRequest);
                            // DecodeSalesReport(strResponse) Not Implemented.
                            strResponse = DecodeSalesReport(strResponse, ttCredential.UserID);
                            break;
                        }

                    default:
                        {
                            throw new Exception(sb.Append("Provider ").Append(ttCredential.Providers[0].Name).Append(" Not Currently Supported.").ToString());
                            sb.Remove(0, sb.Length);
                            break;
                        }
                }

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
                    CoreLib.SendTrace(ttCredential.UserID, "wsSalesReport", "============= OTA Response ============= ", strResponse, UUID);
            }
            sb = null;
            return strResponse;

        }

        #endregion

        #region  Decode Function 

        private string DecodeSalesReport(string strResponse, string UserID)
        {

            try
            {

                var oDoc = new XmlDocument();
                oDoc.LoadXml(strResponse);

                // Dim ttAirlines As DataView = CType(TripXMLMain.AppState.Get("ttAirlines"), DataView)

                var oRoot = oDoc.DocumentElement;
                foreach (XmlNode oNode in oRoot.SelectNodes("JournalEntries/JournalEntry"))
                {
                    try
                    {

                        // *******************
                        // Decode Airlines   *
                        // *******************
                        if (oNode.SelectSingleNode("Airline") is not null & oNode.SelectSingleNode("Airline").Attributes["Code"] is not null)
                        {
                            if (!string.IsNullOrEmpty(oNode.SelectSingleNode("Airline").Attributes["Code"].Value))
                            {
                                oNode.SelectSingleNode("Airline").InnerText = TripXMLLoad.DecodeValue(TripXMLLoad.DecodingType.Airline, oNode.SelectSingleNode("Airline").Attributes["Code"].Value);
                                // GetCodeValue(ttAirlines, oNode.SelectSingleNode("Airline").Attributes("Code").Value)
                            }
                        }
                    }

                    catch (Exception e)
                    {
                        CoreLib.SendTrace(UserID, "wsSalesReport", "Error *** Decoding Airline Response", e.Message, string.Empty);
                    }

                }

                strResponse = oDoc.OuterXml;
            }

            catch (Exception ex)
            {
                CoreLib.SendTrace(UserID, "wsSalesReport", "Error *** Decoding Airline Response", ex.Message, string.Empty);
            }
            return strResponse;
        }

        #endregion

        #region  Web Methods 
        public wmSalesReportOut.SalesReportRS wmSalesReport(wmSalesReportIn.SalesReportRQ SalesReportRQ)
        {
            string xmlMessage = "";
            wmSalesReportOut.SalesReportRS oSalesReportRS = null;
            XmlSerializer oSerializer = null;
            System.IO.StringWriter oWriter = null;
            System.IO.StringReader oReader = null;

            oSerializer = new XmlSerializer(typeof(wmSalesReportIn.SalesReportRQ));
            oWriter = new System.IO.StringWriter(new StringBuilder());
            oSerializer.Serialize(oWriter, SalesReportRQ);
            xmlMessage = oWriter.ToString();
            xmlMessage = xmlMessage.Replace(" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", "");

            xmlMessage = ServiceRequest(xmlMessage, ttServices.SalesReport);

            try
            {
                oSerializer = null;
                oSerializer = new XmlSerializer(@type: typeof(wmSalesReportOut.SalesReportRS));
                oReader = new System.IO.StringReader(xmlMessage);
                oSalesReportRS = (wmSalesReportOut.SalesReportRS)oSerializer.Deserialize(oReader);
            }

            catch (Exception ex)
            {
                CoreLib.SendTrace("", "wsSalesReport", "Error Deserialing OTA Response", ex.Message, string.Empty);
            }

            return oSalesReportRS;

        }
        public string wmSalesReportXml(string xmlRequest)
        {
            return ServiceRequest(xmlRequest, ttServices.SalesReport);
        }

        #endregion

    }

}