using System;
using System.IO;
using System.Net;
using System.Reflection;
using TripXMLMain;

namespace Worldspan
{
    public class ttHttpWebClient
    {
        #region Declaration
        private HttpWebRequest mHttpRequest;
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

        private void HttpConnect()
        {
            mHttpRequest = (HttpWebRequest)WebRequest.Create(ServiceURL);
            mHttpRequest.Method = HttpMethod;
            mHttpRequest.ContentType = "text/xml; charset=utf-8";
            mHttpRequest.Timeout = 180000;   // 3 Minutes
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }

        public string SendHttpRequest(string userID, string token, string uuid)
        {
            string strResponse;
            var startTime = DateTime.Now;
            try
            {
                string strRequest = Body.Contains("GetProviderSession") | Body.Contains("ReleaseProviderSession")
                    ? GetSessionEnvelop()
                    : GetEnvelop(token);

                CoreLib.SendTrace(userID, "ttWorldspanAdapter", "Sent to Worldspan", strRequest, uuid);
                HttpConnect();
                mHttpRequest.ContentLength = strRequest.Length;
                var oWriter = new StreamWriter(mHttpRequest.GetRequestStream());
                oWriter.Write(strRequest);
                oWriter.Close();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error connecting to Worldspan. Worldspan system may be down.\r\n{ex.Message}");
            }

            try
            {
                HttpWebResponse oHttpResponse = (HttpWebResponse)mHttpRequest.GetResponse();
                var oReader = new StreamReader(oHttpResponse.GetResponseStream() ?? throw new InvalidOperationException());
                strResponse = oReader.ReadToEnd();
                oReader.Close();
                CoreLib.SendTrace(userID, "ttWorldspanAdapter", "Received from Worldspan", strResponse, uuid);
                return strResponse;
            }
            catch (Exception ex)
            {
                FieldInfo fi;
                fi = mHttpRequest.GetType().GetField("_HttpResponse", BindingFlags.NonPublic | BindingFlags.Instance);
                if (fi != null)
                {
                    HttpWebResponse oHttpResponse = (HttpWebResponse)fi.GetValue(mHttpRequest);
                    var stream = oHttpResponse?.GetResponseStream();
                    var oReader = new StreamReader(stream ?? throw new InvalidOperationException());
                    strResponse = oReader.ReadToEnd();
                    oReader.Close();
                    CoreLib.SendTrace(userID, "ttWorldspanAdapter", "Worldspan Exception error", strResponse, uuid);
                    return strResponse;
                }

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