using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Xml;

namespace AmadeusWS
{
    /// <summary>
    /// Hand-rolled replacement for the legacy "Web References\wsBlackList" ASMX proxy
    /// (System.Web.Services.Protocols.SoapHttpClientProtocol is not available on net10.0).
    /// Produces the identical SOAP 1.1 document/literal wrapped envelope described by
    /// Web References\wsBlackList\wsFlightBlackList.wsdl:
    ///   operation  wmFlightAdd
    ///   SOAPAction "http://admintalk/wsFlightBlackList/wmFlightAdd"
    ///   namespace  http://admintalk/wsFlightBlackList (elementFormDefault="qualified")
    ///   wrapper    wmFlightAdd { Airline, FlightNo, Departure (xs:dateTime), COS }
    ///   response   wmFlightAddResponse/wmFlightAddResult (xs:string)
    /// </summary>
    internal static class BlackListClient
    {
        private const string ServiceNamespace = "http://admintalk/wsFlightBlackList";

        private static readonly HttpClient _httpClient = new HttpClient(new SocketsHttpHandler
        {
            PooledConnectionLifetime = TimeSpan.FromMinutes(5)
        });
        // HttpClient's default 100-second timeout matches the legacy
        // SoapHttpClientProtocol.Timeout default (100000 ms).

        /// <summary>
        /// Adds a flight to the AdminTalk black list. Returns the wmFlightAddResult string.
        /// </summary>
        public static string wmFlightAdd(string airline, string flightNo, DateTime departure, string cos)
        {
            // URL source preserved from the legacy proxy constructor:
            // Properties.Settings.Default.AmadeusWS_wsBlackList_wsFlightBlackList
            // (applicationSettings section of the host config; compiled default is
            // http://localhost/AdminTalk/WebServices/wsFlightBlackList.asmx).
            string url = Properties.Settings.Default.AmadeusWS_wsBlackList_wsFlightBlackList;

            var sb = new StringBuilder();
            sb.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            sb.Append("<soap:Envelope xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">");
            sb.Append("<soap:Body>");
            sb.Append($"<wmFlightAdd xmlns=\"{ServiceNamespace}\">");
            if (airline != null)
                sb.Append($"<Airline>{Escape(airline)}</Airline>");
            if (flightNo != null)
                sb.Append($"<FlightNo>{Escape(flightNo)}</FlightNo>");
            // RoundtripKind matches the XmlSerializer/ASMX wire format for xs:dateTime.
            sb.Append($"<Departure>{XmlConvert.ToString(departure, XmlDateTimeSerializationMode.RoundtripKind)}</Departure>");
            if (cos != null)
                sb.Append($"<COS>{Escape(cos)}</COS>");
            sb.Append("</wmFlightAdd>");
            sb.Append("</soap:Body>");
            sb.Append("</soap:Envelope>");

            using var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Content = new StringContent(sb.ToString(), Encoding.UTF8, "text/xml");
            // ASMX clients quote the SOAPAction value.
            request.Headers.TryAddWithoutValidation("SOAPAction", "\"http://admintalk/wsFlightBlackList/wmFlightAdd\"");

            using var response = _httpClient.Send(request, HttpCompletionOption.ResponseContentRead);
            string body;
            using (var reader = new StreamReader(response.Content.ReadAsStream()))
            {
                body = reader.ReadToEnd();
            }

            XmlDocument oDoc = null;
            try
            {
                oDoc = new XmlDocument();
                oDoc.LoadXml(body);
            }
            catch (XmlException)
            {
                oDoc = null;
            }

            // SOAP fault -> exception, like SoapHttpClientProtocol.Invoke threw SoapException.
            XmlNode faultString = oDoc?.SelectSingleNode("//*[local-name()='Fault']/*[local-name()='faultstring']");
            if (faultString != null)
            {
                throw new Exception(faultString.InnerText);
            }

            if (!response.IsSuccessStatusCode || oDoc == null)
            {
                throw new Exception($"The request failed with HTTP status {(int)response.StatusCode} {response.ReasonPhrase}.");
            }

            XmlNode result = oDoc.SelectSingleNode("//*[local-name()='wmFlightAddResponse']/*[local-name()='wmFlightAddResult']");
            return result?.InnerText;
        }

        private static string Escape(string value)
        {
            return System.Security.SecurityElement.Escape(value);
        }
    }
}
