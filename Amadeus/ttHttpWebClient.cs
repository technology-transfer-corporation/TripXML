using System.Net;
using System.IO;
using System.IO.Compression;
using TripXMLMain;
using System.Text;
using System;
using System.Reflection;


public class ttHttpWebClient
{

    #region Constants
    private const string SOAP_NS = "http://schemas.xmlsoap.org/soap/envelope/";
    private const string XSD_NS = "http://www.w3.org/2001/XMLSchema";
    private const string XSI_NS = "http://www.w3.org/2001/XMLSchema-instance";
    private const string XSI_NS2 = "http://xml.amadeus.com/ws/2009/01/WBS_Session-2.0.xsd";
    private const string XSI_SEC = "http://xml.amadeus.com/2010/06/Security_v1";
    private const string XSI_TYP = "http://xml.amadeus.com/2010/06/Types_v1";
    private const string XSI_IAT = "http://www.iata.org/IATA/2007/00/IATA2010.1";
    private const string XSI_APP = "http://xml.amadeus.com/2010/06/AppMdw_CommonTypes_v3";
    private const string XSI_LINK = "http://wsdl.amadeus.com/2010/06/ws/Link_v1";
    private const string XSI_SES = "http://xml.amadeus.com/2010/06/Session_v3";
    private const string XSI_FLIR = "http://xml.amadeus.com/"; 
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
    private String Convert(byte[] input, Decoder decoder)
    {
        char[] char_array = new char[decoder.GetCharCount(input, 0, input.Length, true)];

        int cout, bout;
        bool completed;
        decoder.Convert(input, 0, input.Length, char_array, 0, char_array.Length, true, out cout, out bout, out completed);

        return char_array.ToString();
    }

    public string ComposeMessage()
    {
        string message = $"<soap:Envelope xmlns:soap=\'{SOAP_NS}\' xmlns:xsi=\'{XSI_NS}\' xmlns:xsd=\'{XSD_NS}\'><soap:Header xmlns=\"http://webservices.amadeus.com/definitions\">{Header}</soap:Header><soap:Body>{Body}</soap:Body></soap:Envelope>";
        return message;
    }

    public string ComposeMessageSOAP4(string WSAction)
    {
        //sb.Append("<soapenv:Envelope xmlns:soapenv=\'").Append(SOAP_NS).Append("\' xmlns:sec=\'").Append(XSI_SEC).Append("\' xmlns:typ=\'").Append(XSI_TYP);
        //sb.Append("\' xmlns:iat=\'").Append(XSI_IAT).Append("\' xmlns:app=\'").Append(XSI_APP).Append("\' xmlns:link=\'").Append(XSI_LINK);
        //sb.Append("\' xmlns:xsd=\'").Append(XSD_NS).Append("\' xmlns:xsi=\'").Append(XSI_NS);
        //sb.Append("\' xmlns:ses=\'").Append(XSI_SES).Append("\' xmlns:sat=\'").Append(XSI_FLIR).Append(WSAction).Append("\'>");
        //sb.Append("<soapenv:Header").Append(" xmlns:wsa=\'http://www.w3.org/2005/08/addressing\'>").Append(Header).Append("</soapenv:Header>").Append("<soapenv:Body>").Append(Body).Append("</soapenv:Body>").Append("</soapenv:Envelope>").ToString();
        //.Append("\" xmlns:sec=\"").Append(XSI_SEC)
        //.Append("\" xmlns:ses=\"").Append(XSI_SES)

        string message = $"<soapenv:Envelope xmlns:soapenv=\"{SOAP_NS}\" xmlns:wsa=\"http://www.w3.org/2005/08/addressing\"><soapenv:Header>{Header}</soapenv:Header><soapenv:Body>{Body}</soapenv:Body></soapenv:Envelope>";
        return message;
    }

    private string ComposeMessageSOAP2()
    {
        string Message = $"<soapenv:Envelope xmlns:soapenv=\'{SOAP_NS}\' xmlns:awsec=\'{XSI_NS2}\' xmlns:xsd=\'{XSD_NS}\'><soap:Header>{Header}</soap:Header><soap:Body>{Body}</soap:Body></soapenv:Envelope>";
        return Message;
    }

    private void HttpConnect(modCore.TripXMLProviderSystems ttProviderSystems)
    {
        if (!string.IsNullOrEmpty(ttProviderSystems.ProxyURL))
        {
            mHttpRequest = (HttpWebRequest)WebRequest.Create(ttProviderSystems.ProxyURL);
        }
        else
        {
            mHttpRequest = (HttpWebRequest)WebRequest.Create(ServiceURL);
            mHttpRequest.Headers.Add("Content-Encoding: gzip");
            mHttpRequest.Headers.Add("Accept-Encoding: gzip,deflate");
        }

        mHttpRequest.Method = HttpMethod;
        mHttpRequest.ContentType = "text/xml;charset=\"utf-8\"";
        mHttpRequest.Headers.Add("SOAPAction", SoapAction);
        mHttpRequest.UserAgent = "TripXML";
        mHttpRequest.Accept = "text/xml";
        mHttpRequest.KeepAlive = true;
        mHttpRequest.Credentials = CredentialCache.DefaultCredentials;
        mHttpRequest.Timeout = 90000;
    }

    public string SendHttpRequest(modCore.TripXMLProviderSystems ttProviderSystems)
    {
        
        StreamReader oReader = null;
        DateTime StartTime;

        try
        {
            var message = ComposeMessage();

            StartTime = System.DateTime.Now;
            CoreLib.SendTrace(ttProviderSystems.UserID, "AmadeusWSAdapter", "Sent to AmadeusWS", message.Replace("<?xml version=\"1.0\" encoding=\"UTF-8\"?>", ""), ttProviderSystems.LogUUID);
            HttpConnect(ttProviderSystems);

            StreamWriter oWriter = !string.IsNullOrEmpty(ttProviderSystems.ProxyURL)
                ? new StreamWriter(mHttpRequest.GetRequestStream())
                : new StreamWriter(new GZipStream(mHttpRequest.GetRequestStream(), CompressionMode.Compress, false));

            oWriter.Write(message);
            oWriter.Close();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        
        string strResponse;
        try
        {
            var oHttpResponse = (HttpWebResponse)mHttpRequest.GetResponse();

            if (oHttpResponse == null)
            {
                throw new Exception("<Error>Connection problem with Amadeus</Error>");
            }

            if (ttProviderSystems.ProxyURL != "")
                oReader = new StreamReader(oHttpResponse.GetResponseStream());
            else
            {
                Stream stream = oHttpResponse.GetResponseStream();
                Stream decompressionStream = null;
                decompressionStream = new GZipStream(stream, CompressionMode.Decompress, false);
                oReader = new StreamReader(decompressionStream);
            }

            strResponse = oReader.ReadToEnd();

            if (strResponse.Length > 50000)
                CoreLib.SendTrace(ttProviderSystems.UserID, "AmadeusWSAdapter", "Received from AmadeusWS", strResponse.Substring(0, 50000).Replace("<?xml version=\"1.0\" encoding=\"UTF-8\"?>", ""), ttProviderSystems.LogUUID);
            else
                CoreLib.SendTrace(ttProviderSystems.UserID, "AmadeusWSAdapter", "Received from AmadeusWS", strResponse.Replace("<?xml version=\"1.0\" encoding=\"UTF-8\"?>", ""), ttProviderSystems.LogUUID);

            return strResponse;
        }
        catch (Exception ex)
        {
            if (!ex.Message.Contains("SSL/TLS"))
            {
                if (ex.Message == "The operation has timed out")
                {
                    strResponse = "<Error>Time out received from Amadeus</Error>";
                    return strResponse;
                }

                FieldInfo fi = mHttpRequest.GetType().GetField("_HttpResponse", BindingFlags.NonPublic | BindingFlags.Instance);
                if (fi != null)
                {
                    var oHttpResponse = (HttpWebResponse)fi.GetValue(mHttpRequest);
                    Stream stream = oHttpResponse.GetResponseStream();

                    if (ttProviderSystems.ProxyURL != "")
                        oReader = new StreamReader(stream);
                    else
                    {
                        Stream decompressionStream = null;
                        decompressionStream = new GZipStream(stream, CompressionMode.Decompress, false);
                        oReader = new StreamReader(decompressionStream);
                    }

                    strResponse = oReader.ReadToEnd();

                    return strResponse;
                }
            }
            throw new Exception($"Error Getting Response.\r\n{ex.Message}");
        }
        finally
        {
            CoreLib.SendTrace(ttProviderSystems.UserID, "AmadeusWSAdapter", $"AmadeusWS Response Time = {DateTime.Now.Subtract(StartTime).TotalSeconds} seconds.", "", ttProviderSystems.LogUUID);

            if (oReader != null)
            {
                oReader.Close();
            }
            if (mHttpRequest != null)
            {
                mHttpRequest = null;
            }
        }
    }

    public string SendHttpRequestSoap4(modCore.TripXMLProviderSystems ttProviderSystems, string strMessage)
    {
        StreamWriter oWriter = null;
        // Warning!!! Optional parameters not supported
        HttpWebResponse oHttpResponse = null;

        string message = "";
        System.DateTime StartTime;
        try
        {
            message = strMessage;

            StartTime = System.DateTime.Now;
            CoreLib.SendTrace(ttProviderSystems.UserID, "AmadeusWSAdapter", "Sent to AmadeusWS", message.Replace("<?xml version=\"1.0\" encoding=\"UTF-8\"?>", ""), ttProviderSystems.LogUUID);
            HttpConnect(ttProviderSystems);

            oWriter = new StreamWriter(new GZipStream(mHttpRequest.GetRequestStream(), CompressionMode.Compress, false));
            oWriter.Write(message);
        }
        catch (Exception ex)
        {
            addLog($"<M>{message}</M><SendHttpRequest/>", ttProviderSystems.UserID);
            throw new Exception(ex.Message);
        }
        finally
        {
            if (!(oWriter == null))
            {
                oWriter.Close();
            }
        }
        string strResponse;
        try
        {

            oHttpResponse = (HttpWebResponse)mHttpRequest.GetResponse();
            Stream stream = oHttpResponse.GetResponseStream();

            /***********************************/
            Stream decompressionStream = new GZipStream(stream, CompressionMode.Decompress, false);
            var oReader = new StreamReader(decompressionStream);
            /***********************************/
            strResponse = oReader.ReadToEnd();
            oReader.Close();

            if (strResponse.Length > 50000)
                CoreLib.SendTrace(ttProviderSystems.UserID, "AmadeusWSAdapter", "Received from AmadeusWS", strResponse.Substring(0, 50000).Replace("<?xml version=\"1.0\" encoding=\"UTF-8\"?>", ""), ttProviderSystems.LogUUID);
            else
                CoreLib.SendTrace(ttProviderSystems.UserID, "AmadeusWSAdapter", "Received from AmadeusWS", strResponse.Replace("<?xml version=\"1.0\" encoding=\"UTF-8\"?>", ""), ttProviderSystems.LogUUID);

            return strResponse;
        }
        catch (Exception ex)
        {
            addLog($"<M>{message}</M><SendHttpRequest/>", ttProviderSystems.UserID);

            if (ex.Message == "The operation has timed out")
            {
                strResponse = "<Error>Time out received from Amadeus</Error>";
                return strResponse;
            }

            FieldInfo fi = mHttpRequest.GetType().GetField("_HttpResponse", BindingFlags.NonPublic | BindingFlags.Instance);
            if (fi != null)
            {
                oHttpResponse = (HttpWebResponse)fi.GetValue(mHttpRequest);

                if (oHttpResponse == null)
                {
                    addLog($"<EXHRN/>{message}", ttProviderSystems.UserID);
                    strResponse = "<Error>Connection problem with Amadeus</Error>";
                    return strResponse;
                }

                Stream stream = oHttpResponse.GetResponseStream();
                Stream decompressionStream = null;
                decompressionStream = new GZipStream(stream, CompressionMode.Decompress, false);
                var oReader = new StreamReader(decompressionStream);
                strResponse = oReader.ReadToEnd();
                oReader.Close();
                CoreLib.SendTrace(ttProviderSystems.UserID, "AmadeusWSAdapter", "Soap error response", strResponse.Replace("<?xml version=\"1.0\" encoding=\"UTF-8\"?>", ""), ttProviderSystems.LogUUID);

                addLog($"<EXSHR/><M>{message}</M><R>{strResponse}</R>", ttProviderSystems.UserID);
                return strResponse;
            }



            throw new Exception($"Error Getting Response.\r\n{ex.Message}");
        }
        finally
        {
            CoreLib.SendTrace(ttProviderSystems.UserID, "AmadeusWSAdapter", $"AmadeusWS Response Time = {DateTime.Now.Subtract(StartTime).TotalSeconds} seconds.", "", ttProviderSystems.LogUUID);

            if (oHttpResponse != null)
            {
                oHttpResponse.Close();
            }
            if (mHttpRequest != null)
            {
                mHttpRequest = null;
            }
        }
    }

    /// <summary>
    /// This is to log the error in the log
    /// </summary>
    /// <param name="msg"></param>
    /// <param name="username"></param>
    private void addLog(string msg, string username)
    {
        try
        {
            TripXMLTools.TripXMLLog.LogErrorMessage(msg, username, TracerID);
        }
        catch (Exception)
        {
        }
    } 
    #endregion
}
