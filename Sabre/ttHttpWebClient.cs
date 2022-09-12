using System;
using System.IO;
using System.Net;
using System.Reflection;
using TripXMLMain;

namespace Sabre
{
    public class ttHttpWebClient
    {
        #region Constants
        private const string SOAP_NS = "http://schemas.xmlsoap.org/soap/envelope/";
        private const string XSD_NS = "http://www.w3.org/2001/XMLSchema";
        private const string XSI_NS = "http://www.w3.org/2001/XMLSchema-instance";
        #endregion

        #region Declaration
        private HttpWebRequest mHttpRequest;
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

        private void HttpConnect()
        {
            mHttpRequest = (HttpWebRequest)WebRequest.Create(ServiceURL);
            mHttpRequest.Method = HttpMethod;
            mHttpRequest.ContentType = "text/xml";
            mHttpRequest.Headers.Add("SOAPAction", SoapAction);
            mHttpRequest.Timeout = 60000;   // 1 Minute
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }

        public string SendHttpRequest(string UserID, string strMessage = "", string strUUID = "")
        {
            DateTime StartTime = DateTime.Now;
            try
            {
                string Message = !string.IsNullOrEmpty(strMessage)
                    ? strMessage
                    : ComposeMessage();

                CoreLib.SendTrace(UserID, "ttSabreAdapter", "Sent to Sabre", Message.Replace("<?xml version=\"1.0\" encoding=\"UTF-8\"?>", ""), strUUID);
                HttpConnect();
                var oWriter = new StreamWriter(mHttpRequest.GetRequestStream());
                oWriter.Write(Message);
                oWriter.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            try
            {
                var oHttpResponse = (HttpWebResponse)mHttpRequest.GetResponse();
                StreamReader oReader = new StreamReader(oHttpResponse.GetResponseStream());
                string strResponse = oReader.ReadToEnd();
                oReader.Close();
                oHttpResponse.Close();
                CoreLib.SendTrace(UserID, "ttSabreAdapter", "Received from Sabre", strResponse.Replace("<?xml version=\"1.0\" encoding=\"UTF-8\"?>", ""), strUUID);
                return strResponse;
            }
            catch (Exception ex)
            {
                FieldInfo fi;
                fi = mHttpRequest.GetType().GetField("_HttpResponse", BindingFlags.NonPublic | BindingFlags.Instance);
                if (fi != null)
                {
                    var oHttpResponse = (HttpWebResponse)fi.GetValue(mHttpRequest);
                    
                    if (oHttpResponse == null)
                        throw new Exception($"Error Getting Response.\r\n{ex.Message}");

                    var stream = oHttpResponse.GetResponseStream();
                    var oReader = new StreamReader(stream);
                    string strResponse = oReader.ReadToEnd();
                    oReader.Close();
                    oHttpResponse.Close();
                    CoreLib.SendTrace(UserID, "ttSabreAdapter", "Sabre Exception error", strResponse, strUUID);
                    return strResponse;
                }

                throw new Exception($"Error Getting Response.\r\n{ex.Message}");
            }
            finally
            {
                CoreLib.SendTrace(UserID, "ttSabreAdapter", $"Sabre Response Time = {DateTime.Now.Subtract(StartTime).TotalSeconds} seconds.", "", strUUID);

                if (mHttpRequest != null)
                    mHttpRequest = null;
            }

        } 
        #endregion

    }
}