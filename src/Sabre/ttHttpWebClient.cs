using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using TripXMLMain;

namespace Sabre
{
    public class ttHttpWebClient
    {
        #region Constants
        private const string SOAP_NS = "http://schemas.xmlsoap.org/soap/envelope/";
        private const string XSD_NS = "http://www.w3.org/2001/XMLSchema";
        private const string XSI_NS = "http://www.w3.org/2001/XMLSchema-instance";

        /// <summary>
        /// Legacy HttpWebRequest.Timeout was 60000 ms (1 minute). The HttpClient is shared,
        /// so the timeout is enforced per request via a CancellationTokenSource.
        /// </summary>
        private const int REQUEST_TIMEOUT_MS = 60000;   // 1 Minute
        #endregion

        #region Declaration
        // One pooled client for the process; pooled connection lifetime keeps DNS fresh.
        private static readonly HttpClient _httpClient = new HttpClient(new SocketsHttpHandler
        {
            AutomaticDecompression = DecompressionMethods.GZip,
            PooledConnectionLifetime = TimeSpan.FromMinutes(5)
        });

        public string ServiceURL { get; set; } = "";
        public string SoapAction { get; set; } = "";
        public string HttpMethod { get; set; } = "";
        public string Header { get; set; } = "";
        public string Body { get; set; } = "";
        #endregion

        #region Methods
        private string ComposeMessage()
        {
            string message = $"<soap:Envelope xmlns:soap='{SOAP_NS}' xmlns:xsi='{XSI_NS}' xmlns:xsd='{XSD_NS}'><soap:Header>{Header}</soap:Header><soap:Body>{Body}</soap:Body></soap:Envelope>";
            return message;
        }

        public string SendHttpRequest(string UserID, string strMessage = "", string strUUID = "")
        {
            DateTime StartTime = DateTime.Now;
            HttpRequestMessage oHttpRequest = null;
            try
            {
                string Message = !string.IsNullOrEmpty(strMessage)
                    ? strMessage
                    : ComposeMessage();

                CoreLib.SendTrace(UserID, "ttSabreAdapter", "Sent to Sabre", Message.Replace("<?xml version=\"1.0\" encoding=\"UTF-8\"?>", ""), strUUID);

                oHttpRequest = new HttpRequestMessage(new System.Net.Http.HttpMethod(this.HttpMethod), ServiceURL);
                oHttpRequest.Headers.TryAddWithoutValidation("SOAPAction", SoapAction);
                oHttpRequest.Content = new ByteArrayContent(Encoding.UTF8.GetBytes(Message));
                oHttpRequest.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("text/xml");
            }
            catch (Exception ex)
            {
                oHttpRequest?.Dispose();
                throw new Exception(ex.Message);
            }

            try
            {
                using var timeoutCts = new CancellationTokenSource(REQUEST_TIMEOUT_MS);
                using var oHttpResponse = _httpClient.Send(oHttpRequest, HttpCompletionOption.ResponseContentRead, timeoutCts.Token);

                // Read the body regardless of the status code: non-success responses (e.g. Sabre
                // SOAP fault bodies on HTTP 500) still carry the payload the caller needs. The
                // legacy code reached it through a reflection hack on WebException; with
                // HttpClient this is simply the normal path.
                using var oReader = new StreamReader(oHttpResponse.Content.ReadAsStream());
                string strResponse = oReader.ReadToEnd();

                if (!oHttpResponse.IsSuccessStatusCode)
                {
                    CoreLib.SendTrace(UserID, "ttSabreAdapter", "Sabre Exception error", strResponse, strUUID);
                    return strResponse;
                }

                CoreLib.SendTrace(UserID, "ttSabreAdapter", "Received from Sabre", strResponse.Replace("<?xml version=\"1.0\" encoding=\"UTF-8\"?>", ""), strUUID);
                return strResponse;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Getting Response.\r\n{ex.Message}");
            }
            finally
            {
                CoreLib.SendTrace(UserID, "ttSabreAdapter", $"Sabre Response Time = {DateTime.Now.Subtract(StartTime).TotalSeconds} seconds.", "", strUUID);

                oHttpRequest?.Dispose();
            }

        }
        #endregion

    }
}
