using System;
using System.Diagnostics;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using TripXMLMain;
using static TripXMLMain.modCore;

namespace wsTripXML.wsTravelTalk
{
    public partial class wsPing
    {
        public TripXML tXML;

        private readonly modMain _modMain;

        public wsPing(modMain modMain)
        {
            _modMain = modMain;
        }

        #region  Decode Functions 

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

                switch (ttCredential.Providers[0].Name.ToLower() ?? "")
                {

                    case "amadeusws":
                        {

                            XmlDocument oDoc = null;
                            XmlElement oRoot = null;
                            XmlNode oNode = null;
                            double dTime;
                            int iWait = 10;

                            oDoc = new XmlDocument();
                            oDoc.LoadXml(strRequest);
                            oRoot = oDoc.DocumentElement;

                            if (oRoot.SelectSingleNode("WaitTime") is not null)
                            {

                                iWait = Conversions.ToInteger(oRoot.SelectSingleNode("WaitTime").InnerText);

                            }

                            dTime = DateAndTime.Timer;
                            while (DateAndTime.Timer - dTime <= iWait)
                            {
                            }

                            strResponse = "<TXML_PingRS><Success/></TXML_PingRS>";
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


            catch (Exception ex)
            {
                strResponse = FormatErrorMessage(ttServiceID, ex.Message, ttCredential.Providers[0].Name);
            }
            finally
            {
                _modMain.LogResponse(ref strResponse, ref ttCredential, StartTime, (int)ttServiceID, Environment.MachineName, ref UUID);
                if (modCore.Trace)
                    CoreLib.SendTrace(ttCredential.UserID, "wsIssueTicket", "============= OTA Response ============= ", strResponse, UUID);
            }

            return strResponse;
            sb = null;
        }

        #endregion

        #region  Web Methods 
        public wmPingOut.TXML_PingRS wmPing(wmPingIn.TXML_PingRQ TXML_PingRQ)
        {
            string xmlMessage = "";
            wmPingOut.TXML_PingRS oPingRS = null;
            XmlSerializer oSerializer = null;
            System.IO.StringWriter oWriter = null;
            System.IO.StringReader oReader = null;

            oSerializer = new XmlSerializer(typeof(wmPingIn.TXML_PingRQ));
            oWriter = new System.IO.StringWriter(new StringBuilder());
            oSerializer.Serialize(oWriter, TXML_PingRQ);
            xmlMessage = oWriter.ToString();
            xmlMessage = xmlMessage.Replace(" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", "");

            xmlMessage = ServiceRequest(xmlMessage, ttServices.Ping);

            try
            {
                oSerializer = null;
                oSerializer = new XmlSerializer(@type: typeof(wmPingOut.TXML_PingRS));
                oReader = new System.IO.StringReader(xmlMessage);
                oPingRS = (wmPingOut.TXML_PingRS)oSerializer.Deserialize(oReader);
            }
            catch (Exception ex)
            {
                CoreLib.SendTrace("", "wsPing", "Error Deserialing OTA Response", ex.Message, string.Empty);
            }

            return oPingRS;

        }
        public string wmPingXml(string xmlRequest)
        {
            return ServiceRequest(xmlRequest, ttServices.TicketDisplay);
        }

        #endregion
        public string ping(int Delay)
        {
            double dTime;
            int iWait = 10;

            if (Delay != 0)
            {

                iWait = Delay;

            }

            dTime = DateAndTime.Timer;
            while (DateAndTime.Timer - dTime <= iWait)
            {
            }

            return "Waited " + iWait.ToString() + " seconds.";
        }

    }

    // Public Class _Default
    // Inherits Page
    // Protected Sub Page_Load(sender As Object, e As EventArgs)
    // Dim v As String = Request.QueryString("Delay")
    // If v IsNot Nothing Then
    // Response.Write("param is ")
    // Response.Write(v)
    // End If
    // End Sub
    // End Class


}