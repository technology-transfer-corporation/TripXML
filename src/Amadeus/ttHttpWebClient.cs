using System.Net;
using System.IO;
using System.IO.Compression;
using TripXMLMain;
using System.Text;
using System;
using System.Net.Http;
using System.Threading.Tasks;


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

    // One pooled client for the process (replaces the per-call HttpWebRequest);
    // pooled connection lifetime keeps DNS fresh. Created lazily so that
    // TripXMLMain.modCore.config is populated before the certificate policy is read.
    private static readonly Lazy<HttpClient> _httpClient = new Lazy<HttpClient>(CreateHttpClient);

    public string ServiceURL { get; set; } = "";

    public string SoapAction { get; set; } = "";

    public string HttpMethod { get; set; } = "";

    public string Header { get; set; } = "";

    public string Body { get; set; } = "";

    public string TracerID { get; set; } = "";

    #endregion

    #region Methods

    private static HttpClient CreateHttpClient()
    {
        var handler = new SocketsHttpHandler
        {
            // Legacy code sent "Accept-Encoding: gzip,deflate" and manually decompressed
            // the response; AutomaticDecompression does both based on Content-Encoding.
            AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
            PooledConnectionLifetime = TimeSpan.FromMinutes(5),
            // Legacy: mHttpRequest.Credentials = CredentialCache.DefaultCredentials
            Credentials = CredentialCache.DefaultCredentials
        };

        // The legacy adapter disabled TLS certificate validation process-wide via
        // ServicePointManager.ServerCertificateValidationCallback. Preserve that default,
        // scoped to the Amadeus handler only. Ops can re-enable validation by setting
        // AmadeusSkipCertValidation=false in configuration.
        string skipCertValidation = modCore.config["AmadeusSkipCertValidation"];
        if (skipCertValidation == null || skipCertValidation.Equals("true", StringComparison.OrdinalIgnoreCase))
        {
            handler.SslOptions.RemoteCertificateValidationCallback = (s, c, ch, e) => true;
        }

        // Legacy: mHttpRequest.Timeout = 90000
        return new HttpClient(handler) { Timeout = TimeSpan.FromMilliseconds(90000) };
    }

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

    /// <summary>
    /// Builds the HttpRequestMessage the way the legacy HttpConnect/StreamWriter pair did.
    /// When ProxyURL is configured the legacy code posted the plain (uncompressed) message
    /// to the proxy endpoint and did not add the gzip headers.
    /// </summary>
    private HttpRequestMessage BuildRequest(string url, string message, bool compressBody, bool markCompressed)
    {
        var request = new HttpRequestMessage(
            new System.Net.Http.HttpMethod(string.IsNullOrEmpty(this.HttpMethod) ? "POST" : this.HttpMethod), url);

        byte[] requestBytes = compressBody ? CompressGZip(message) : Encoding.UTF8.GetBytes(message);
        var content = new ByteArrayContent(requestBytes);
        // Legacy: mHttpRequest.ContentType = "text/xml;charset=\"utf-8\""
        content.Headers.TryAddWithoutValidation("Content-Type", "text/xml;charset=\"utf-8\"");
        if (markCompressed)
        {
            // Legacy: mHttpRequest.Headers.Add("Content-Encoding: gzip")
            content.Headers.TryAddWithoutValidation("Content-Encoding", "gzip");
        }
        request.Content = content;

        request.Headers.TryAddWithoutValidation("SOAPAction", SoapAction);
        request.Headers.TryAddWithoutValidation("User-Agent", "TripXML");
        request.Headers.TryAddWithoutValidation("Accept", "text/xml");

        return request;
    }

    private static byte[] CompressGZip(string message)
    {
        using var memoryStream = new MemoryStream();
        using (var gzipStream = new GZipStream(memoryStream, CompressionMode.Compress, false))
        using (var writer = new StreamWriter(gzipStream))
        {
            writer.Write(message);
        }
        return memoryStream.ToArray();
    }

    private static bool IsTimeout(Exception ex)
    {
        // HttpClient surfaces its Timeout as TaskCanceledException (inner TimeoutException).
        return ex is TaskCanceledException || ex is TimeoutException || ex.InnerException is TimeoutException;
    }

    public string SendHttpRequest(modCore.TripXMLProviderSystems ttProviderSystems)
    {
        HttpRequestMessage oRequest;
        DateTime StartTime;

        try
        {
            var message = ComposeMessage();

            StartTime = System.DateTime.Now;
            CoreLib.SendTrace(ttProviderSystems.UserID, "AmadeusWSAdapter", "Sent to AmadeusWS", message.Replace("<?xml version=\"1.0\" encoding=\"UTF-8\"?>", ""), ttProviderSystems.LogUUID);

            bool useProxyEndpoint = !string.IsNullOrEmpty(ttProviderSystems.ProxyURL);
            oRequest = BuildRequest(
                useProxyEndpoint ? ttProviderSystems.ProxyURL : ServiceURL,
                message,
                compressBody: !useProxyEndpoint,
                markCompressed: !useProxyEndpoint);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

        string strResponse;
        try
        {
            using var oHttpResponse = _httpClient.Value.Send(oRequest, HttpCompletionOption.ResponseContentRead);

            using (var oReader = new StreamReader(oHttpResponse.Content.ReadAsStream()))
            {
                strResponse = oReader.ReadToEnd();
            }

            if (!oHttpResponse.IsSuccessStatusCode)
            {
                // Legacy behaviour: GetResponse() threw on a non-2xx status and the catch
                // block returned the response body (read via the reflected _HttpResponse)
                // without the "Received from AmadeusWS" trace.
                return strResponse;
            }

            if (strResponse.Length > 50000)
                CoreLib.SendTrace(ttProviderSystems.UserID, "AmadeusWSAdapter", "Received from AmadeusWS", strResponse.Substring(0, 50000).Replace("<?xml version=\"1.0\" encoding=\"UTF-8\"?>", ""), ttProviderSystems.LogUUID);
            else
                CoreLib.SendTrace(ttProviderSystems.UserID, "AmadeusWSAdapter", "Received from AmadeusWS", strResponse.Replace("<?xml version=\"1.0\" encoding=\"UTF-8\"?>", ""), ttProviderSystems.LogUUID);

            return strResponse;
        }
        catch (Exception ex)
        {
            if (IsTimeout(ex))
            {
                strResponse = "<Error>Time out received from Amadeus</Error>";
                return strResponse;
            }

            throw new Exception($"Error Getting Response.\r\n{ex.Message}");
        }
        finally
        {
            CoreLib.SendTrace(ttProviderSystems.UserID, "AmadeusWSAdapter", $"AmadeusWS Response Time = {DateTime.Now.Subtract(StartTime).TotalSeconds} seconds.", "", ttProviderSystems.LogUUID);

            oRequest.Dispose();
        }
    }

    public string SendHttpRequestSoap4(modCore.TripXMLProviderSystems ttProviderSystems, string strMessage)
    {
        HttpRequestMessage oRequest = null;

        string message = "";
        System.DateTime StartTime;
        try
        {
            message = strMessage;
            StartTime = System.DateTime.Now;
            CoreLib.SendTrace(ttProviderSystems.UserID, "AmadeusWSAdapter", "Sent to AmadeusWS", message.Replace("<?xml version=\"1.0\" encoding=\"UTF-8\"?>", ""), ttProviderSystems.LogUUID);

            // Legacy SendHttpRequestSoap4 always gzip-compressed the body, but HttpConnect
            // only added the gzip headers when no proxy endpoint was configured.
            bool useProxyEndpoint = !string.IsNullOrEmpty(ttProviderSystems.ProxyURL);
            oRequest = BuildRequest(
                useProxyEndpoint ? ttProviderSystems.ProxyURL : ServiceURL,
                message,
                compressBody: true,
                markCompressed: !useProxyEndpoint);
        }
        catch (Exception ex)
        {
            addLog($"<M>{message}</M><SendHttpRequest/>", ttProviderSystems);
            throw new Exception(ex.Message);
        }

        string strResponse;
        try
        {
            using var oHttpResponse = _httpClient.Value.Send(oRequest, HttpCompletionOption.ResponseContentRead);

            using (var oReader = new StreamReader(oHttpResponse.Content.ReadAsStream()))
            {
                strResponse = oReader.ReadToEnd();
            }

            if (!oHttpResponse.IsSuccessStatusCode)
            {
                // Legacy behaviour: GetResponse() threw on a non-2xx status and the catch
                // block read the body off the reflected response, traced and logged it.
                CoreLib.SendTrace(ttProviderSystems.UserID, "AmadeusWSAdapter", "Soap error response", strResponse.Replace("<?xml version=\"1.0\" encoding=\"UTF-8\"?>", ""), ttProviderSystems.LogUUID);

                addLog($"<EXSHR/><M>{message}</M><R>{strResponse}</R>", ttProviderSystems);
                return strResponse;
            }

            if (strResponse.Length > 50000)
                CoreLib.SendTrace(ttProviderSystems.UserID, "AmadeusWSAdapter", "Received from AmadeusWS", strResponse.Substring(0, 50000).Replace("<?xml version=\"1.0\" encoding=\"UTF-8\"?>", ""), ttProviderSystems.LogUUID);
            else
                CoreLib.SendTrace(ttProviderSystems.UserID, "AmadeusWSAdapter", "Received from AmadeusWS", strResponse.Replace("<?xml version=\"1.0\" encoding=\"UTF-8\"?>", ""), ttProviderSystems.LogUUID);

            return strResponse;
        }
        catch (Exception ex)
        {
            addLog($"<M>{message}</M><SendHttpRequest/>", ttProviderSystems);

            if (IsTimeout(ex))
            {
                strResponse = "<Error>Time out received from Amadeus</Error>";
                return strResponse;
            }

            if (ex is HttpRequestException)
            {
                // Legacy: reflected _HttpResponse was null -> connection problem.
                addLog($"<EXHRN/>{message}", ttProviderSystems);
                strResponse = "<Error>Connection problem with Amadeus</Error>";
                return strResponse;
            }

            throw new Exception($"Error Getting Response.\r\n{ex.Message}");
        }
        finally
        {
            CoreLib.SendTrace(ttProviderSystems.UserID, "AmadeusWSAdapter", $"AmadeusWS Response Time = {DateTime.Now.Subtract(StartTime).TotalSeconds} seconds.", "", ttProviderSystems.LogUUID);

            if (oRequest != null)
            {
                oRequest.Dispose();
            }
        }
    }

    /// <summary>
    /// This is to log the error in the log
    /// </summary>
    /// <param name="msg"></param>
    /// <param name="username"></param>
    private void addLog(string msg, modCore.TripXMLProviderSystems provider)
    {
        try
        {
            //TripXMLTools.TripXMLLog.LogErrorMessage(msg, username, TracerID);
            modCore.AddLog(modCore.LogType.Error, msg, provider);
        }
        catch (Exception)
        {
        }
    }
    #endregion
}
