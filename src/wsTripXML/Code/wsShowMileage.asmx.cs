using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using Microsoft.VisualBasic.CompilerServices;
using TripXMLMain;
using static TripXMLMain.modCore;
using TripXMLTools;
using wsTripXML.wsTravelTalk.wmShowMileageOut;

namespace wsTripXML.wsTravelTalk
{
    public partial class wsShowMileage
    {

        private readonly modMain _modMain;

        public wsShowMileage(modMain modMain)
        {
            _modMain = modMain;
        }

        #region  Decode Function 
        public enum enMile
        {
            FromCity = 0,
            ToCity = 1,
            DepTime = 2,
            ArrTime = 3,
            Meals = 4,
            EQP = 5,
            ELPD = 6,
            ACCUM = 7,
            MILES = 8,
            SM = 9
        }

        /// <summary>
        /// Get Code of Airline based on OPERATED BY line
        /// </summary>
        /// <param name="encodedLine">Line that containes OPERATED BY</param>
        /// <param name="UserID"></param>
        /// <param name="UUID"></param>
        /// <returns></returns>
        private string DecodePNRRead(string encodedLine)
        {
            string responseCode;
            try
            {
                if (encodedLine is null)
                {
                    responseCode = encodedLine;
                }
                else
                {
                    responseCode = TripXMLLoad.EncodeValue(TripXMLLoad.DecodingType.Airline, encodedLine);
                }
            }
            catch (Exception ex)
            {
                // CoreLib.SendTrace(UserID, "wsPNRRead", "Error *** Decoding AirAvail Response", ex.Message, UUID)
                responseCode = encodedLine;
            }
            return responseCode;
        }
        /// <summary>
        /// Clean up string fopr Decoding. Removing unneeded information from string in order to maximize probobility of the correct result.
        /// </summary>
        /// <param name="encodedLine"></param>
        /// <returns>Airline Code. EX: TK</returns>
        private string CleanUpString(string remark)
        {
            // *DTW-CMH OPERATED BY DL/REPUBLIC AIRWAYS DELTA CONNECTION FOR DELTA AIR LINES.
            // *YUL-MCO OPERATED BY /AIR CANADA ROUGE.
            // *TSA-KNH OPERATED BY /EVA AIRWAYS FOR UNI AIRWAYS.
            // *ATL-SLC OPERATED BY DELTA AIR LINES FOR LATAM AIRLINES GROUP.
            // *FLL-ATL OPERATED BY DELTA AIR LINES INC.
            // *MCO-FRA OPERATED BY EW DISCOVER GMBH.
            // *MSP-YYZ OPERATED BY /AIR CANADA EXPRESS - JAZZ.
            try
            {
                int index = remark.IndexOf("OPERATED BY");
                int endIndex = remark.IndexOf("/");
                string _remark = remark.Substring(index).Replace(".", "");

                if (remark.Contains("/"))
                {
                    var _reg = new Regex(@"\s\w{2,}/");
                    if (_reg.IsMatch(_remark))
                    {
                        // *DTW-CMH OPERATED BY DL/REPUBLIC AIRWAYS DELTA CONNECTION FOR DELTA AIR LINES.
                        // *FRA-MEX OPERATED BY LUFTHANSA/DEUTSCHE LUFTHANSA AG.
                        // In this case correct answer is DL
                        _remark = remark.Substring(index, endIndex - index);
                    }
                    else
                    {
                        // *YUL-MCO OPERATED BY /AIR CANADA ROUGE.
                        // In this correct answer is AIR CANADA ROUGE
                        _remark = remark.Substring(endIndex + 1);

                        if (_remark.Contains(" - "))
                        {
                            var elem = _remark.Split(new[] { " - " }, StringSplitOptions.None).ToList();
                            _remark = elem[0].Trim();
                        }

                    }
                }

                else
                {
                    // *MCO-FRA OPERATED BY EW DISCOVER GMBH.
                    // In this case correct answer is EW
                    //string _code = remark.Substring(index + 12).Trim();
                    //var _reg = new Regex(@"^([A-Z]){2}\s");

                    //if (_reg.IsMatch(_remark.Replace("OPERATED BY ", "")))
                    //{
                    //    _remark = _code.Substring(0, 2);
                    //    return _remark;
                    //}

                    if (_remark.Contains(" AS "))
                    {
                        var elem = _remark.Replace("OPERATED BY ", "").Split(new[] { " AS " }, StringSplitOptions.None).ToList();
                        _remark = elem[0].Trim();
                    }

                }

                return _remark;
            }
            catch (Exception ex)
            {
                return remark;
            }

        }

        #endregion

        #region  Process Service Request All GDS 
        private StringBuilder sb = new StringBuilder();

        private string ServiceRequest(string request, ttServices ttServiceID)
        {
            string response = "";
            TravelTalkCredential ttCredential = default;
            TripXMLProviderSystems ttProviderSystems = default;
            bool ValidateXSDOut;
            var StartTime = default(DateTime);
            string UUID = "";

            try
            {
                StartTime = DateTime.Now;
                _modMain.PreServiceRequest(ref request, ref ttCredential, ref ttProviderSystems, StartTime, (int)ttServiceID, Environment.MachineName, ref UUID);
                ValidateXSDOut = Conversions.ToBoolean(TripXMLMain.AppState.Get(sb.Append("XSD").Append(ttCredential.UserID).Append("Out").ToString()));
                sb.Remove(0, sb.Length);
                if (request.Contains("OTA_ShowMilesRQ"))
                {
                    // All comunicatioon for this purpose will be gone through Sabe
                    var ttDefProvider = new TripXMLProviderSystems();
                    string aaapcc = ttDefProvider.AAAPCC;
                    string _pcc = ttCredential.Providers[0].PCC;

                    ttDefProvider.AAAPCC = ttCredential.Providers[0].PCC;
                    ttCredential.Providers[0].PCC = "A5C6";
                    _modMain.PreServiceRequest(ref request, ref ttCredential, ref ttDefProvider, StartTime, (int)ttServiceID, Environment.MachineName, ref UUID, "", true);
                    response = modMain.SendOtherRequestSabre(ttServiceID, ref ttCredential, ref ttDefProvider, ref request);
                    ttCredential.Providers[0].PCC = _pcc;
                }
                else
                {
                    switch (ttCredential.Providers[0].Name ?? "")
                    {
                        case "AmadeusWS":
                            {
                                response = modMain.SendOtherRequestAmadeusWS(ttServiceID, ref ttCredential, ref ttProviderSystems, ref request);
                                break;
                            }
                        case "Apollo":
                        case "Galileo":
                            {
                                response = modMain.SendOtherRequestGalileo(ttServiceID, ref ttCredential, ref ttProviderSystems, ref request);
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
                                response = modMain.SendOtherRequestSabre(ttServiceID, ref ttCredential, ref ttProviderSystems, ref request);
                                break;
                            }

                        default:
                            {
                                throw new Exception(sb.Append("Provider ").Append(ttCredential.Providers[0].Name).Append(" Not Currently Supported.").ToString());
                                sb.Remove(0, sb.Length);
                                break;
                            }
                    }
                }


                // DecodeShowMileage(strResponse) Not Implemented.

                modMain.PostServiceRequest(ref response, ValidateXSDOut, (int)ttServiceID, ttCredential.UserID);
            }

            catch (Exception ex)
            {
                response = FormatErrorMessage(ttServiceID, ex.Message, ttCredential.Providers[0].Name);
            }
            finally
            {
                _modMain.LogResponse(ref response, ref ttCredential, StartTime, (int)ttServiceID, Environment.MachineName, ref UUID);
                if (modCore.Trace)
                    CoreLib.SendTrace(ttCredential.UserID, "wsShowMileage", "============= OTA Response ============= ", response, UUID);
            }
            sb = null;
            return response;

        }

        #endregion

        #region  Web Methods 
        public OTA_ShowMileageRS wmShowMileage(wmShowMileageIn.OTA_ShowMileageRQ OTA_ShowMileageRQ)
        {
            OTA_ShowMileageRS oShowMileageRS = null;

            var oSerializer = new XmlSerializer(typeof(wmShowMileageIn.OTA_ShowMileageRQ));
            var oWriter = new System.IO.StringWriter(new StringBuilder());
            oSerializer.Serialize(oWriter, OTA_ShowMileageRQ);
            string xmlMessage = oWriter.ToString();
            xmlMessage = xmlMessage.Replace(" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", "");

            xmlMessage = ServiceRequest(xmlMessage, ttServices.ShowMileage);

            try
            {
                oSerializer = null;
                oSerializer = new XmlSerializer(@type: typeof(OTA_ShowMileageRS));
                var oReader = new System.IO.StringReader(xmlMessage);
                oShowMileageRS = (OTA_ShowMileageRS)oSerializer.Deserialize(oReader);
            }
            catch (Exception ex)
            {
                CoreLib.SendTrace("", "wsShowMileage", "Error Deserialing OTA Response", ex.Message, string.Empty);
            }

            return oShowMileageRS;

        }
        public OTA_ShowMileageRS wmShowMiles(wmShowMileageIn.OTA_ShowMilesRQ OTA_ShowMilesRQ)
        {
            OTA_ShowMileageRS oShowMileageRS = null;

            var oSerializer = new XmlSerializer(typeof(wmShowMileageIn.OTA_ShowMilesRQ));
            var oWriter = new System.IO.StringWriter(new StringBuilder());
            oSerializer.Serialize(oWriter, OTA_ShowMilesRQ);
            string xmlMessage = oWriter.ToString();
            xmlMessage = xmlMessage.Replace(" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", "");

            xmlMessage = ServiceRequest(xmlMessage, ttServices.ShowMileage);

            try
            {
                oSerializer = null;
                oSerializer = new XmlSerializer(@type: typeof(OTA_ShowMileageRS));
                var oReader = new System.IO.StringReader(xmlMessage);
                oShowMileageRS = (OTA_ShowMileageRS)oSerializer.Deserialize(oReader);
                FillMilesInformation(ref oShowMileageRS, OTA_ShowMilesRQ);
            }
            catch (Exception ex)
            {
                CoreLib.SendTrace("", "wsShowMileage", "Error Deserialing OTA Response", ex.Message, string.Empty);
            }

            return oShowMileageRS;

        }

        private void FillMilesInformation(ref OTA_ShowMileageRS response, wmShowMileageIn.OTA_ShowMilesRQ request)
        {
            try
            {
                // DPTR ARVL               MEALS EQP ELPD ACCUM MILES SM
                // JFK IST 100P 650A\u00871 M    333 9.50  9.50  5000 N
                // Error: ChrW(135) & "INVALID FLT"

                if (response.Errors?.ToList().Exists(e => (e.Value?.Contains("INVALID FLT")) ?? false) == true)
                {
                    response.OperatingCarrier = "INVALID FLT";
                    return;
                }

                var elems = response.Remarks.Remark[1].Split(' ').ToList();
                response.FromCity = elems[(int)enMile.FromCity];
                response.ToCity = new ToCity() { Value = elems[(int)enMile.ToCity], Mileage = elems[(int)enMile.MILES], AccumulativeMileage = elems[(int)enMile.ACCUM] };
                response.TotalMileage = elems[(int)enMile.MILES];

                if (response.Remarks.Remark.ToList().Exists(l => l.Contains(" OPERATED BY ")))
                {
                    string remark = response.Remarks.Remark.ToList().Find(l => l.Contains(" OPERATED BY ")); // .Split("OPERATED BY", StringSplitOptions.RemoveEmptyEntries).ToList
                    if (!string.IsNullOrEmpty(remark))
                    {
                        response.OperatingCarrier = GetOperatingCarrier(remark);
                    }
                    else
                    {
                        response.OperatingCarrier = request.CarrierCode;
                    }
                }
                else
                {
                    response.OperatingCarrier = request.CarrierCode;
                }
            }

            catch (Exception ex)
            {
                response.Errors = new[] { new Error() { Value = ex.Message } };
            }
        }

        private string GetOperatingCarrier(string remark)
        {
            remark = CleanUpString(remark);
            string _code = DecodePNRRead(remark);
            if (_code.Length.Equals(2))
            {
                return _code;
            }
            return string.Empty;
        }
        public string wmShowMileageXml(string xmlRequest)
        {
            return ServiceRequest(xmlRequest, ttServices.ShowMileage);
        }
        public string wmShowMilesXml(string xmlRequest)
        {
            return ServiceRequest(xmlRequest, ttServices.ShowMileage);
        }

        #endregion


    }

}