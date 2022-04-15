using System;
using System.Text;
using System.Net;
using TripXMLMain;
using System.IO;
using System.Xml;

namespace Travelport
{
    public class ttHttpWebClient
    {
        #region Declaration
        private HttpWebRequest mHttpRequest;
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

        private HttpWebRequest HttpConnect(modCore.TripXMLProviderSystems ttProviderSystems)
        {
            HttpWebRequest serverRequest = (HttpWebRequest)WebRequest.Create(ttProviderSystems.URL + this.ServiceName);

            // We are posting a XML request
            serverRequest.Method = "POST";
            serverRequest.ContentType = "text/xml";

            // Set up the connection to optimize for web services and receive compressed responses.
            ServicePointManager.UseNagleAlgorithm = false;
            ServicePointManager.Expect100Continue = false;
            serverRequest.AutomaticDecompression = DecompressionMethods.GZip;

            // Always add authentication to the header - avoids issue with internal URL's that doesn't require
            // authentication.
            byte[] authBytes = Encoding.UTF8.GetBytes((ttProviderSystems.UserName + ":" + ttProviderSystems.Password).ToCharArray());
            serverRequest.Headers["Authorization"] = $"Basic {Convert.ToBase64String(authBytes)}";
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            return serverRequest;

        }

        public string SendHttpRequest(string strMessage, modCore.TripXMLProviderSystems ttProviderSystems)
        {
            if (strMessage == null)
            {
                throw new ArgumentNullException("request");
            }

            //mHttpRequest = this.CreateRequestObject();
            mHttpRequest = HttpConnect(ttProviderSystems);
            byte[] requestBytes = ComposeMessage();

            //Send request to the server
            Stream stream = mHttpRequest.GetRequestStream();
            stream.Write(requestBytes, 0, requestBytes.Length);
            stream.Close();

            //Receive response
            Stream receiveStream;
            try
            {
                HttpWebResponse webResponse = (HttpWebResponse)mHttpRequest.GetResponse();
                receiveStream = webResponse.GetResponseStream();
            }
            catch (WebException exception)
            {
                this.SetErrorMessage(exception);

                if (exception.Response != null)
                {
                    // Although the request failed, we can still get a response that might
                    // contain a better error message.
                    receiveStream = exception.Response.GetResponseStream();
                }
                else
                {
                    return null;
                }
            }

            // Read output stream
            StreamReader streamReader = new StreamReader(receiveStream, Encoding.UTF8);
            string result = streamReader.ReadToEnd();

            // Remove SOAP elements
            XmlDocument filteredDocument = this.GetResponseDocument(result);

            return filteredDocument.OuterXml.ToString();
        }

        private void SetErrorMessage(WebException exception)
        {
            if (exception.Response != null && ((HttpWebResponse)exception.Response).StatusCode == HttpStatusCode.Unauthorized)
            {
                this.LastError = "The server returned Unauthorized. Please ensure that you are using the correct user name and password.";
            }
            else if (exception.Response != null && ((HttpWebResponse)exception.Response).StatusCode == HttpStatusCode.NotFound)
            {
                this.LastError = "The service could not be found on the server. Please check that you are using the correct URL.";
                this.LastError += Environment.NewLine + Environment.NewLine;
                this.LastError += "The URL will vary depending on the service you want to access.";
            }
            else
            {
                this.LastError = exception.Message;
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
