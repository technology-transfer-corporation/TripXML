using System;
using System.Data;
using System.Diagnostics;
using System.Text;
using System.Web.Services;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.VisualBasic.CompilerServices;
using TripXMLMain;
using static TripXMLMain.modCore;

namespace wsTripXML.wsTravelTalk
{


    [WebService(Namespace = "http://tripxml.downtowntravel.com/tripxml/wsCruiseRead", Name = "wsCruiseRead", Description = "A TripXML Web Service to Process Cruise PNR Read Messages Request.")]


    public class wsCruiseRead : WebService
    {

        #region  Web Services Designer Generated Code 

        public wsCruiseRead() : base()
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

        private string DecodeCruiseRead(string strResponse, string UserID)
        {
            XmlDocument oDoc = null;
            XmlElement oRoot = null;
            DataView ttCruiseLines;
            DataView ttCruiseShips;
            DataView ttCruiseAdvisory;
            XmlNode oNode = null;

            try
            {

                oDoc = new XmlDocument();
                oDoc.LoadXml(strResponse);
                oRoot = oDoc.DocumentElement;

                ttCruiseAdvisory = (DataView)Application.Get("ttCruiseAdvisory");

                if (oRoot.SelectSingleNode("Errors") is null)
                {

                    ttCruiseLines = (DataView)Application.Get("ttCruiseLines");
                    ttCruiseShips = (DataView)Application.Get("ttCruiseShips");

                    oNode = oRoot.SelectSingleNode("TravelItinerary/ItineraryInfo/ReservationItems/Item/Cruise/SailingInfo/SelectedSailing");

                    // *******************************
                    // Decode CruiseLines & Ships    *
                    // *******************************
                    string argstrCode = oNode.Attributes["VendorCode"].Value;
                    oNode.Attributes["VendorName"].Value = modMain.GetDecodeValue(ref ttCruiseLines, ref argstrCode);
                    oNode.Attributes["VendorCode"].Value = argstrCode;
                    oNode.Attributes["ShipName"].Value = modMain.GetCruiseFilterValue(ref ttCruiseShips, oNode.Attributes["VendorCode"].Value, oNode.Attributes["ShipCode"].Value);

                    // *******************************
                    // Decode Advisory Errors Codes  *
                    // *******************************
                    foreach (XmlNode currentONode in oRoot.SelectNodes("Warnings/Warning"))
                    {
                        oNode = currentONode;
                        if (oNode.InnerText.Length == 0)
                        {
                            string argstrCode1 = oNode.Attributes["Code"].Value;
                            oNode.InnerText = modMain.GetDecodeValue(ref ttCruiseAdvisory, ref argstrCode1);
                            oNode.Attributes["Code"].Value = argstrCode1;
                        }
                    }
                }
                else
                {
                    // *******************************
                    // Decode Advisory Errors Codes  *
                    // *******************************
                    foreach (XmlNode currentONode1 in oRoot.SelectNodes("Errors/Error"))
                    {
                        oNode = currentONode1;
                        if (oNode.InnerText.Length == 0)
                        {
                            string argstrCode2 = oNode.Attributes["Code"].Value;
                            oNode.InnerText = modMain.GetDecodeValue(ref ttCruiseAdvisory, ref argstrCode2);
                            oNode.Attributes["Code"].Value = argstrCode2;
                        }
                    }
                }

                strResponse = oDoc.OuterXml;
            }

            catch (Exception ex)
            {
                CoreLib.SendTrace(UserID, "wsCruiseRead", "Error *** Decoding CruiseRead Response", ex.Message, string.Empty);
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

                var argoApp = Application;
                modMain.PreServiceRequest(ref strRequest, ref argoApp, ref ttCredential, ref ttProviderSystems, StartTime, (int)ttServiceID, Server.MachineName, ref UUID);
                ValidateXSDOut = Conversions.ToBoolean(Application.Get(sb.Append("XSD").Append(ttCredential.UserID).Append("Out").ToString()));
                sb.Remove(0, sb.Length);

                switch (ttCredential.Providers[0].Name.ToLower() ?? "")
                {
                    case "amadeus":
                        {
                            break;
                        }
                    // Dim ttAA As AmadeusAPIAdapter

                    // ttAA = Application.Get(sb.Append("API").Append(ttCredential.UserID).Append(ttCredential.System).Append(ttCredential.Providers(0).PCC).ToString())
                    // sb.Remove(0, sb.Length())
                    // If ttAA Is Nothing Then
                    // Throw New Exception(sb.Append("Access denied to Amadeus - ").Append(ttCredential.System).Append(" system. Or invalid provider.").ToString())
                    // sb.Remove(0, sb.Length())
                    // End If

                    // If ttCredential.Providers(0).PCC.Trim.Length > 0 Then
                    // ttAA.SourcePCC = ttCredential.Providers(0).PCC
                    // End If

                    // 'Send Reuest
                    // strResponse = SendCruiseRequestAmadeus(ttServiceID, ttCredential, ttAA, strRequest)
                    // Application.Set(sb.Append("API").Append(ttCredential.UserID).Append(ttCredential.System).ToString(), ttAA)
                    // sb.Remove(0, sb.Length())

                    case "amadeusws":
                        {

                            strResponse = modMain.SendCruiseRequestAmadeusWS(ttServiceID, ref ttCredential, ref ttProviderSystems, ref strRequest);
                            break;
                        }

                    case "apollo":
                    case "galileo":
                        {

                            strResponse = modMain.SendCruiseRequestGalileo(ttServiceID, ref ttCredential, ref ttProviderSystems, ref strRequest);
                            break;
                        }

                    default:
                        {
                            throw new Exception(sb.Append("Provider ").Append(ttCredential.Providers[0].Name).Append(" Not Currently Supported.").ToString());
                            sb.Remove(0, sb.Length);
                            break;
                        }
                }

                strResponse = DecodeCruiseRead(strResponse, ttCredential.UserID);

                modMain.PostServiceRequest(ref strResponse, ValidateXSDOut, (int)ttServiceID, ttCredential.UserID);
            }

            catch (Exception ex)
            {
                strResponse = FormatErrorMessage(ttServiceID, ex.Message, ttCredential.Providers[0].Name);
            }
            finally
            {
                modMain.LogResponse(ref strResponse, ref ttCredential, StartTime, (int)ttServiceID, Server.MachineName, ref UUID);
                if (modCore.Trace)
                    CoreLib.SendTrace(ttCredential.UserID, "wsCruiseRead", "============= OTA Response ============= ", strResponse, UUID);
            }
            sb = null;
            return strResponse;

        }

        #endregion

        #region  Web Methods 

        [WebMethod(Description = "Process Cruise PNR Read Messages Request.")]
        public wmCruiseReadOut.OTA_CruiseReadRS wmCruiseRead(wmCruiseReadIn.OTA_CruiseReadRQ OTA_CruiseReadRQ)
        {
            string xmlMessage = "";
            wmCruiseReadOut.OTA_CruiseReadRS oCruiseReadRS = null;
            XmlSerializer oSerializer = null;
            System.IO.StringWriter oWriter = null;
            System.IO.StringReader oReader = null;

            oSerializer = new XmlSerializer(typeof(wmCruiseReadIn.OTA_CruiseReadRQ));
            oWriter = new System.IO.StringWriter(new StringBuilder());
            oSerializer.Serialize(oWriter, OTA_CruiseReadRQ);
            xmlMessage = oWriter.ToString();
            xmlMessage = xmlMessage.Replace(" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", "");

            xmlMessage = ServiceRequest(xmlMessage, ttServices.CruiseRead);

            try
            {
                oSerializer = null;
                oSerializer = new XmlSerializer(@type: typeof(wmCruiseReadOut.OTA_CruiseReadRS));
                oReader = new System.IO.StringReader(xmlMessage);
                oCruiseReadRS = (wmCruiseReadOut.OTA_CruiseReadRS)oSerializer.Deserialize(oReader);
            }
            catch (Exception ex)
            {
                CoreLib.SendTrace("", "wsCruiseRead", "Error Deserialing OTA Response", ex.Message, string.Empty);
            }

            return oCruiseReadRS;

        }

        [WebMethod(Description = "Process Cruise PNR Read Xml Messages Request.")]
        public string wmCruiseReadXml(string xmlRequest)
        {
            return ServiceRequest(xmlRequest, ttServices.CruiseRead);
        }

        #endregion

    }

}