using System;
using System.IO;
using System.Net;
using System.Reflection;
using TripXMLMain;

namespace Galileo
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
        public string TracerID { get; set; } = "";
        #endregion

        #region Methods
        private string ComposeMessage()
        {
            string Message = $"<soapenv:Envelope xmlns:soapenv='{SOAP_NS}'><soapenv:Header>{Header}</soapenv:Header><soapenv:Body>{Body}</soapenv:Body></soapenv:Envelope>";
            return Message;
        }

        private void HttpConnect()
        {
            mHttpRequest = (HttpWebRequest)WebRequest.Create(ServiceURL);
            mHttpRequest.Method = HttpMethod;
            mHttpRequest.ContentType = "text/xml;charset=UTF-8";
            // mHttpRequest.Accept = "gzip, deflate"
            mHttpRequest.Headers.Add("SOAPAction", SoapAction);
            // mHttpRequest.Timeout = 60000   ' 1 Minute
            // mHttpRequest.Headers.Add("Authorization", "Basic uAPI-244313738:k6GgkR27")
            mHttpRequest.Headers.Add("Authorization", "Basic GWS/GWS_P7114864:Pjo38-DYm+");
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
        }

        public string SendHttpRequest(string UserID, string strMessage = "")
        {
            HttpWebResponse oHttpResponse = null;
            string Message = "";
            string strResponse = "";
            DateTime StartTime = DateTime.Now;
            try
            {
                Message = string.IsNullOrEmpty(strMessage)
                    ? strMessage
                    : ComposeMessage();

                CoreLib.SendTrace(UserID, "ttGalileoAdapter", "Sent to Galileo", Message.Replace("<?xml version=\"1.0\" encoding=\"UTF-8\"?>", ""), string.Empty);
                HttpConnect();
                StreamWriter oWriter = new StreamWriter(mHttpRequest.GetRequestStream());
                oWriter.Write(Message);
            }
            catch (Exception ex)
            {
                //GalileoBase.addLog($"<EXOR><M>{Message}</M><SendHttpRequest/>", UserID);
                TripXMLTools.TripXMLLog.LogErrorMessage($"<EXOR><M>{Message}</M><SendHttpRequest/>", UserID, TracerID);
                throw ex;
            }

            try
            {
                oHttpResponse = (HttpWebResponse)mHttpRequest.GetResponse();
                StreamReader oReader = new StreamReader(oHttpResponse.GetResponseStream());
                strResponse = oReader.ReadToEnd();
                CoreLib.SendTrace(UserID, "ttGalileoAdapter", "Received from Galileo", strResponse.Replace("<?xml version=\"1.0\" encoding=\"UTF-8\"?>", ""), string.Empty);
            }
            catch (Exception ex)
            {
                FieldInfo fi;
                fi = mHttpRequest.GetType().GetField("_HttpResponse", BindingFlags.NonPublic | BindingFlags.Instance);
                if (fi != null)
                {
                    oHttpResponse = (HttpWebResponse)fi.GetValue(mHttpRequest);
                    var stream = oHttpResponse.GetResponseStream();
                    StreamReader oReader1 = new StreamReader(stream);
                    strResponse = oReader1.ReadToEnd();
                    return strResponse;
                }
                throw new Exception($"Error Getting Response.\r\n{ex.Message}");
            }
            finally
            {
                CoreLib.SendTrace(UserID, "ttGalileoAdapter", $"Galileo Response Time = {Convert.ToInt32(DateTime.Now.Subtract(StartTime).TotalSeconds)} seconds.", "", string.Empty);

                if (oHttpResponse != null)
                    oHttpResponse.Close();

                if (mHttpRequest != null)
                    mHttpRequest = null;
            }

            return strResponse;
        }
        #endregion

    }
}