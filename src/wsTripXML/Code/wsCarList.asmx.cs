using System;
using System.Diagnostics;
using System.Text;
using System.Xml.Serialization;
using Microsoft.VisualBasic.CompilerServices;
using TripXMLMain;
using static TripXMLMain.modCore;

namespace wsTripXML.wsTravelTalk
{
    public partial class wsCarList
    {
        private StringBuilder sb = new StringBuilder();

        public TripXML tXML;

        private readonly modMain _modMain;

        public wsCarList(modMain modMain)
        {
            _modMain = modMain;
        }

        #region  Decode Function 

        // Not Implemented

        #endregion

        #region  Process Service Request All GDS 

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
                    // Case "Amadeus"
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

                    // strResponse = SendCarRequestAmadeus(ttServiceID, ttCredential, ttAA, strRequest)
                    // TripXMLMain.AppState.Set(sb.Append("API").Append(ttCredential.UserID).Append(ttCredential.System).ToString(), ttAA)
                    // sb.Remove(0, sb.Length())

                    case "AmadeusWS":
                        {

                            strResponse = modMain.SendCarRequestAmadeusWS(ttServiceID, ref ttCredential, ref ttProviderSystems, ref strRequest);
                            break;
                        }

                        // Case "Apollo", "Galileo"

                        // strResponse = SendCarRequestGalileo(ttServiceID, ttCredential, ttProviderSystems, strRequest)

                        // Case "Sabre"

                        // ttProviderSystems = TripXMLMain.AppState.Get(sb.Append("PS").Append(ttCredential.Providers(0).Name).Append(ttCredential.UserID).Append(ttCredential.System).Append(ttCredential.Providers(0).PCC).ToString())
                        // sb.Remove(0, sb.Length())
                        // If ttProviderSystems.System Is Nothing Then
                        // FormatErrorMessage(ttServiceID, sb.Append("Access denied to ").Append(ttCredential.Providers(0).Name).Append(" - ").Append(ttCredential.System).Append(" system. Or invalid provider.").ToString(), ttCredential.Providers(0).Name)
                        // sb.Remove(0, sb.Length())
                        // Exit Select
                        // End If

                        // ttProviderSystems.AAAPCC = ttCredential.Providers(0).PCC

                        // strResponse = SendCarRequestSabre(ttServiceID, ttCredential, ttProviderSystems, strRequest)

                        // Case Else
                        // Throw New Exception(sb.Append("Provider ").Append(ttCredential.Providers(0).Name).Append(" Not Currently Supported.").ToString())
                        // sb.Remove(0, sb.Length())
                }

                // DecodeCarList(strResponse) Not Implemented

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
                    CoreLib.SendTrace(ttCredential.UserID, "wsCarList", "============= OTA Response ============= ", strResponse, UUID);
            }

            return strResponse;
            sb = null;
        }

        #endregion

        #region  Web Methods 
        public wmCarListOut.OTA_VehLocSearchRS wmCarList(wmCarListIn.OTA_VehLocSearchRQ OTA_VehLocSearchRQ)
        {
            string xmlMessage = "";
            wmCarListOut.OTA_VehLocSearchRS oCarListRS = null;
            XmlSerializer oSerializer = null;
            System.IO.StringWriter oWriter = null;
            System.IO.StringReader oReader = null;

            oSerializer = new XmlSerializer(typeof(wmCarListIn.OTA_VehLocSearchRQ));
            oWriter = new System.IO.StringWriter(new StringBuilder());
            oSerializer.Serialize(oWriter, OTA_VehLocSearchRQ);
            xmlMessage = oWriter.ToString();
            xmlMessage = xmlMessage.Replace(" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", "");

            xmlMessage = ServiceRequest(xmlMessage, ttServices.CarList);

            try
            {
                oSerializer = null;
                oSerializer = new XmlSerializer(@type: typeof(wmCarListOut.OTA_VehLocSearchRS));
                oReader = new System.IO.StringReader(xmlMessage);
                oCarListRS = (wmCarListOut.OTA_VehLocSearchRS)oSerializer.Deserialize(oReader);
            }
            catch (Exception ex)
            {
                CoreLib.SendTrace("", "wsCarList", "Error Deserialing OTA Response", ex.Message, string.Empty);
                xmlMessage = "<OTA_VehLocSearchRS Version=\"1.001\"><Errors><Error>" + ex.InnerException.ToString() + "</Error></Errors></OTA_VehLocSearchRS>";
                oReader = new System.IO.StringReader(xmlMessage);
                oCarListRS = (wmCarListOut.OTA_VehLocSearchRS)oSerializer.Deserialize(oReader);
            }

            return oCarListRS;

        }
        public string wmCarListXml(string xmlRequest)
        {
            return ServiceRequest(xmlRequest, ttServices.CarList);
        }

        #endregion

    }

}