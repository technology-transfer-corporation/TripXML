using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using TripXMLMain;

namespace Worldspan
{
    public class ttHttpWebClient
    {
        #region Declaration
        // One pooled client for the process; pooled connection lifetime keeps DNS fresh.
        // No global Timeout here - the legacy 3-minute timeout is applied per request below.
        private static readonly HttpClient _httpClient = new HttpClient(new SocketsHttpHandler
        {
            AutomaticDecompression = DecompressionMethods.GZip,
            PooledConnectionLifetime = TimeSpan.FromMinutes(5)
        })
        {
            // Disable the client-wide 100 s default so the per-request 180 s token below
            // is the only timeout (matches legacy HttpWebRequest.Timeout = 180000).
            Timeout = Timeout.InfiniteTimeSpan
        };

        public string ServiceURL { get; set; } = "";
        public string SoapAction { get; set; } = "";
        public string HttpMethod { get; set; } = "";
        public string Header { get; set; } = "";
        public string Body { get; set; } = "";
        #endregion

        #region Methods
        private string GetEnvelop(string conversationID)
        {
            string strEnvelop = "<SOAP-ENV:Envelope xmlns:SOAP-ENV=\"http://schemas.xmlsoap.org/soap/envelope/\">" +
            $"<SOAP-ENV:Header>{Header}</SOAP-ENV:Header>" +
            "<SOAP-ENV:Body><ns1:ProviderTransaction xmlns:ns1=\"xxs\">";
            if (!string.IsNullOrEmpty(conversationID))
                strEnvelop += $"<CONTEXT>{conversationID}</CONTEXT>";

            strEnvelop += $"<REQ>{Body}</REQ></ns1:ProviderTransaction></SOAP-ENV:Body></SOAP-ENV:Envelope>";

            return strEnvelop;
        }

        private string GetSessionEnvelop()
        {
            string strEnvelop = "<SOAP-ENV:Envelope xmlns:SOAP-ENV=\"http://schemas.xmlsoap.org/soap/envelope/\">" +
            $"<SOAP-ENV:Header>{Header}</SOAP-ENV:Header><SOAP-ENV:Body>{Body}</SOAP-ENV:Body></SOAP-ENV:Envelope>";

            return strEnvelop;
        }

        public string SendHttpRequest(string userID, string token, string uuid)
        {
            string strResponse;
            var startTime = DateTime.Now;
            HttpResponseMessage oHttpResponse;
            try
            {
                string strRequest = Body.Contains("GetProviderSession") | Body.Contains("ReleaseProviderSession")
                    ? GetSessionEnvelop()
                    : GetEnvelop(token);

                CoreLib.SendTrace(userID, "ttWorldspanAdapter", "Sent to Worldspan", strRequest, uuid);

                using var oHttpRequest = new HttpRequestMessage(new System.Net.Http.HttpMethod(HttpMethod), ServiceURL);
                oHttpRequest.Content = new StringContent(strRequest, Encoding.UTF8, "text/xml");

                // Legacy HttpWebRequest.Timeout was 180000 ms (3 minutes); enforce it per request
                // so the shared HttpClient keeps its default (no short global timeout).
                using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(180000));
                oHttpResponse = _httpClient.Send(oHttpRequest, HttpCompletionOption.ResponseContentRead, cts.Token);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error connecting to Worldspan. Worldspan system may be down.\r\n{ex.Message}");
            }

            try
            {
                using (oHttpResponse)
                {
                    // Always read the body, success or not - error responses carry the provider's
                    // error payload (this replaces the legacy reflection on "_HttpResponse").
                    var oReader = new StreamReader(oHttpResponse.Content.ReadAsStream());
                    strResponse = oReader.ReadToEnd();
                    oReader.Close();

                    if (!oHttpResponse.IsSuccessStatusCode)
                    {
                        CoreLib.SendTrace(userID, "ttWorldspanAdapter", "Worldspan Exception error", strResponse, uuid);
                        return strResponse;
                    }

                    CoreLib.SendTrace(userID, "ttWorldspanAdapter", "Received from Worldspan", strResponse, uuid);
                    return strResponse;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Getting Response.\r\n{ex.Message}");
            }
            finally
            {
                CoreLib.SendTrace(userID, "ttWorldspanAdapter", $"Worldspan Response Time = {Convert.ToInt32(DateTime.Now.Subtract(startTime).TotalSeconds)} seconds.", "", uuid);
            }
        }
        #endregion
    }
}
