using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Xml;
using TripXMLMain;

namespace Travelport
{
    public class ttHttpWebClient
    {
        #region Declaration
        // One pooled client for the process; pooled connection lifetime keeps DNS fresh.
        private static readonly HttpClient _httpClient = new HttpClient(new SocketsHttpHandler
        {
            AutomaticDecompression = DecompressionMethods.GZip,
            PooledConnectionLifetime = TimeSpan.FromMinutes(5)
        });

        private StringBuilder sb = new StringBuilder();
        public string LastError { get; set; }

        /// <summary>
        /// Travel port has AirServices, HotelServices etc. We will pass it according to messege type.
        /// </summary>
        public string ServiceName { get; set; } = "";

        public string HttpMethod { get; set; } = "";

        /// <summary>
        /// This is the content of the Soap Header. At the initial implementation not used as no headers required.
        /// </summary>
        public string Header { get; set; } = "";

        public string Body { get; set; } = "";

        /// <summary>
        /// This is the RezGateway tracer ID
        /// </summary>
        public string TracerID { get; set; } = "";

        #endregion

        #region Methods
        public byte[] ComposeMessage()
        {
            sb = new StringBuilder();
            sb.Append("<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\">");
            sb.Append("<soapenv:Header/>");
            sb.Append("<soapenv:Body>");
            sb.Append(Body);
            sb.Append("</soapenv:Body>");
            sb.Append("</soapenv:Envelope>");

            UTF8Encoding encoding = new UTF8Encoding();
            byte[] requestBytes = encoding.GetBytes(sb.ToString());

            sb.Remove(0, sb.Length);

            return requestBytes;
        }

        public string SendHttpRequest(string message, modCore.TripXMLProviderSystems ttProviderSystems)
        {
            if (string.IsNullOrEmpty(message))
                throw new ArgumentNullException("request");

            byte[] requestBytes = ComposeMessage();
            string result = ProcessRequest(requestBytes, ttProviderSystems);

            // Remove SOAP elements
            XmlDocument filteredDocument = this.GetResponseDocument(result);

            return filteredDocument.OuterXml.ToString();
        }

        private string ProcessRequest(byte[] requestBytes, modCore.TripXMLProviderSystems ttProviderSystems)
        {
            try
            {
                using var request = new HttpRequestMessage(System.Net.Http.HttpMethod.Post, ttProviderSystems.URL + this.ServiceName);
                request.Content = new ByteArrayContent(requestBytes);
                request.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("text/xml");

                // Always add authentication to the header - avoids issue with internal URL's that doesn't require authentication.
                byte[] authBytes = Encoding.UTF8.GetBytes(ttProviderSystems.UserName + ":" + ttProviderSystems.Password);
                request.Headers.TryAddWithoutValidation("Authorization", $"Basic {Convert.ToBase64String(authBytes)}");

                using var response = _httpClient.Send(request, HttpCompletionOption.ResponseContentRead);

                if (!response.IsSuccessStatusCode)
                {
                    // The request failed, but the response body may still carry a better error message
                    // (matches the legacy WebException.Response path).
                    this.SetErrorMessage(response.StatusCode);
                }

                using var receiveStream = response.Content.ReadAsStream();
                using var streamReader = new System.IO.StreamReader(receiveStream, Encoding.UTF8);
                return streamReader.ReadToEnd();
            }
            catch (Exception ex)
            {
                this.LastError = ex.GetBaseException().Message;
                return string.Empty;
            }
        }

        private void SetErrorMessage(HttpStatusCode statusCode)
        {
            if (statusCode == HttpStatusCode.Unauthorized)
            {
                this.LastError = "The server returned Unauthorized. Please ensure that you are using the correct user name and password.";
            }
            else if (statusCode == HttpStatusCode.NotFound)
            {
                this.LastError = "The service could not be found on the server. Please check that you are using the correct URL.";
                this.LastError += Environment.NewLine + Environment.NewLine;
                this.LastError += "The URL will vary depending on the service you want to access.";
            }
            else
            {
                this.LastError = $"The remote server returned an error: ({(int)statusCode}) {statusCode}.";
            }
        }

        /// <summary>
        /// Extracts the xml response from the HTTP/Soap  Response
        /// </summary>
        /// <param name="result">
        /// <returns></returns>
        private XmlDocument GetResponseDocument(string result)
        {
            XmlDocument filteredDocument = null;

            try
            {
                XmlDocument responseXmlDocument = new XmlDocument();
                responseXmlDocument.LoadXml(result);

                XmlNode filteredResponse = responseXmlDocument.SelectSingleNode("//*[local-name()='Body']/*");

                filteredDocument = new XmlDocument();
                filteredDocument.LoadXml(filteredResponse.OuterXml);
            }
            catch (Exception ex)
            {
                if (string.IsNullOrEmpty(this.LastError))
                {
                    if (ex.Message.StartsWith("'xsd' is an undeclared prefix", StringComparison.OrdinalIgnoreCase))
                    {
                        LastError = "The server could not find a valid schema document for the schema referenced in your request.Please check the version number of each schema referenced.";
                    }
                    else
                    {
                        LastError = ex.Message;
                    }
                }
            }

            return filteredDocument;
        }
        #endregion

    }
}
