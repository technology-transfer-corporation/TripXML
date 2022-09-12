using Microsoft.Web.Services3;
using Microsoft.Web.Services3.Messaging;
using Microsoft.Web.Services3.Design;
using System.Xml;
using System.IO;
using TripXMLMain;
using System;
using System.Web.Configuration;
using System.Text;

namespace Sita.Sws
{
    class SITAAdapter : SoapClient
    {
        private modCore.TripXMLProviderSystems ttProviderSystems;

        public SITAAdapter(System.Uri destinationURL, modCore.TripXMLProviderSystems ProviderSystems)
            : base(destinationURL)
        {
            ttProviderSystems = ProviderSystems;
            //create our policy
            Policy policy = new Policy();
            policy.Assertions.Add(new MyPolicyAssertion(WebConfigurationManager.AppSettings["SITACertificatePath"].ToString(), WebConfigurationManager.AppSettings["SITACertificatePassword"].ToString()));
            SetPolicy(policy);
        }

        public XmlDocument execute(XmlDocument request)
        {
            SoapEnvelope envelope = new SoapEnvelope();
            envelope.SetBodyObject(request);

            SoapEnvelope response = SendRequestResponse("", envelope);

            return (XmlDocument)response.GetBodyObject(typeof(XmlDocument));
        }

        public string Send(string request)
        {
            //parse the request
            CoreLib.SendTrace(ttProviderSystems.UserID, "SITAAdapter", "Send request", request);
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(request);

            XmlDocument response = execute(doc);

            StringWriter sw = new StringWriter();
            response.WriteTo(new XmlTextWriter(sw));

            string resp = sw.ToString();
            resp = resp.Replace(" Version=\"0.001\" xmlns:ota=\"http://www.opentravel.org/OTA/2003/05\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:sita=\"http://www.sita.aero/PTS/fare/2005/11/PriceRS\">", ">");
            resp = resp.Replace("sita:", "");
            resp = resp.Replace("ota:", "");
            resp = resp.Replace(" xmlns=\"http://www.opentravel.org/OTA/2003/05\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"","");
            CoreLib.SendTrace(ttProviderSystems.UserID, "SITAAdapter", "Receive response", resp);
            return resp;
        }

        public string CreateSession()
        {
            string SessionID = "";
            StringBuilder sb = new StringBuilder();
            XmlDocument doc = new XmlDocument();
            XmlElement oRoot = null;
            oRoot = doc.DocumentElement;

            CoreLib.SendTrace(ttProviderSystems.UserID, "SITAAdapter", "Create Session", "");
            try
            {
                sb.Append("<SITA_SignInRQ xmlns=\"http://www.opentravel.org/OTA/2003/05\">");
                sb.Append("<POS><Source ERSP_UserID=\"").Append(ttProviderSystems.UserName).Append("\" AgentSine=\"").Append(ttProviderSystems.Password).Append("\" PseudoCityCode=\"");
                sb.Append(ttProviderSystems.PCC).Append("\" AgentDutyCode=\"17\" ISOCountry=\"EC\" AirlineVendorID=\"EQ\" AirportCode=\"");
                sb.Append(ttProviderSystems.PCC.Substring(0,3)).Append("\"/></POS></SITA_SignInRQ>");

                SessionID = Send(sb.ToString());

                doc.LoadXml(SessionID);
                oRoot = doc.DocumentElement;

                SessionID = oRoot.SelectSingleNode("pid").InnerText;
                
                return SessionID;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public string CloseSession(string SessionID)
        {
            StringBuilder sb = new StringBuilder();
            string strResponse = "";
            
            CoreLib.SendTrace(ttProviderSystems.UserID, "SITAAdapter", "Close Session", "");

            try
            {
               sb.Append("<SITA_SignOutRQ xmlns=\"http://www.opentravel.org/OTA/2003/05\" TransactionIdentifier=\"" + SessionID + "\">");
               sb.Append("<POS><Source ERSP_UserID=\"").Append(ttProviderSystems.UserName).Append("\" AgentSine=\"").Append(ttProviderSystems.Password).Append("\" PseudoCityCode=\"");
               sb.Append(ttProviderSystems.PCC).Append("\" AgentDutyCode=\"17\" ISOCountry=\"EC\" AirlineVendorID=\"EQ\" AirportCode=\"");
               sb.Append(ttProviderSystems.PCC.Substring(0,3)).Append("\"/></POS></SITA_SignOutRQ>");

               strResponse = Send(sb.ToString());

               return strResponse;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        //private static string certificatePath = "..\\..\\..\\certificate.pfx";
        //private static string certificatePassword = "changeit";
        //private static string serviceUrl = "https://rxabeta.sita.aero/RXA_V01_100/soap/SITAReservationService";
        //public static void Main(string[] args)
        //{
        //    string req = "<OTA_AirFlifoRQ Version=\"0\" xmlns=\"http://www.opentravel.org/OTA/2003/05\" TransactionIdentifier=\"\"><Airline Code=\"XS\"/><FlightNumber>2014</FlightNumber><DepartureDate>2008-10-31</DepartureDate></OTA_AirFlifoRQ>";
        //    SITAAdapter client = new SITAAdapter(new System.Uri(serviceUrl), this.ttProviderSystems);

        //    System.Console.WriteLine(client.Send(req));
        //    System.Console.ReadKey();
        //}
    }
}
