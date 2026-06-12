using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Xml;

namespace Galileo
{
    /// <summary>
    /// Hand-rolled SOAP 1.1 client that replaces the four generated
    /// <c>SoapHttpClientProtocol</c> proxies (wsGalileoProd / wsGalileoCopy XmlSelect and
    /// wsGalileoProdIV / wsGalileoCopyIV ImageViewer) for net10.0.
    ///
    /// The request envelopes are byte-identical to what the legacy proxies produced
    /// (verified by capturing the generated .NET Framework 4.8 proxies' output against a
    /// loopback listener): UTF-8 without BOM, no indentation, namespace declarations in
    /// the order soap/xsi/xsd, wrapped document/literal parameters in declared order, and
    /// XmlElement payloads nested inside their wrapper member elements.
    ///
    /// Response handling mirrors the proxies exactly: the returned XmlElement is the first
    /// element child of the result wrapper (SubmitXmlResult / SubmitXmlOnSessionResult /
    /// Responses / RetrievePhotoInformationReturn), string operations return the wrapper's
    /// text content, and a SOAP fault throws <see cref="GalileoSoapFaultException"/> whose
    /// Message carries the faultstring (SoapException parity at every call site).
    /// </summary>
    public class GalileoSoapClient
    {
        #region Constants
        private const string SoapNs = "http://schemas.xmlsoap.org/soap/envelope/";
        private const string XsiNs = "http://www.w3.org/2001/XMLSchema-instance";
        private const string XsdNs = "http://www.w3.org/2001/XMLSchema";

        /// <summary>Namespace of the XMLSelect service (wrapper elements and members).</summary>
        private const string XmlSelectNs = "http://webservices.galileo.com";

        /// <summary>Namespace of the ImageViewer (HotelImage) service.</summary>
        private const string HotelImageNs = "http://webservices.galileo.com/HotelImage";

        /// <summary>
        /// Default endpoint baked into both generated ImageViewer proxies
        /// (wsGalileoProdIV and wsGalileoCopyIV used the same address).
        /// </summary>
        public const string DefaultImageViewerUrl = "https://americas.webservices.travelport.com/B2BGateway/service/ImageViewer";

        /// <summary>SoapHttpClientProtocol default timeout (used by the ImageViewer proxies, which never had Timeout set).</summary>
        private const int DefaultTimeoutMs = 100000;
        #endregion

        #region Declaration
        // One pooled client for the process; pooled connection lifetime keeps DNS fresh.
        // Per-call timeouts are enforced below, so the shared client itself never times out.
        private static readonly HttpClient _httpClient = new HttpClient(new SocketsHttpHandler
        {
            AutomaticDecompression = DecompressionMethods.GZip,
            PooledConnectionLifetime = TimeSpan.FromMinutes(5)
        })
        {
            Timeout = System.Threading.Timeout.InfiniteTimeSpan
        };

        // Legacy parity: the proxies sent the MS Web Services Client Protocol user agent.
        private const string LegacyUserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; MS Web Services Client Protocol 4.0.30319.42000)";

        /// <summary>
        /// SOAPAction prefix. The production proxy (wsGalileoProd) declared
        /// "http://webservices.galileo.com/{Operation}" actions while the copy-system proxy
        /// (wsGalileoCopy) was regenerated from a WSDL that declared
        /// "https://webservices.galileo.com/{Operation}". Preserved verbatim per system.
        /// </summary>
        private readonly string _soapActionPrefix;
        #endregion

        #region Properties (proxy-compatible surface)
        /// <summary>Service endpoint URL (ProviderSystems.URL for XmlSelect operations).</summary>
        public string Url { get; set; }

        /// <summary>
        /// Credentials, looked up per request URI with realm "Basic" — same
        /// CredentialCache.GetCredential prefix-matching semantics as the legacy proxies.
        /// </summary>
        public ICredentials Credentials { get; set; }

        /// <summary>Kept for call-site compatibility; Basic auth is always sent pre-emptively when a credential resolves.</summary>
        public bool PreAuthenticate { get; set; }

        /// <summary>Request timeout in milliseconds (the adapter sets 60000 for XmlSelect operations).</summary>
        public int Timeout { get; set; } = DefaultTimeoutMs;
        #endregion

        /// <param name="url">Endpoint URL.</param>
        /// <param name="production">
        /// True for the Production system (wsGalileoProd parity: http:// SOAPActions),
        /// false for the Copy system (wsGalileoCopy parity: https:// SOAPActions).
        /// </param>
        public GalileoSoapClient(string url, bool production)
        {
            Url = url;
            _soapActionPrefix = production
                ? "http://webservices.galileo.com/"
                : "https://webservices.galileo.com/";
        }

        #region XmlSelect operations
        /// <summary>Begins an XML Select session and returns the session token.</summary>
        public string BeginSession(string Profile)
        {
            return InvokeString("BeginSession", "BeginSessionResult", ("Profile", Profile));
        }

        /// <summary>Ends an XML Select session.</summary>
        public void EndSession(string Token)
        {
            Invoke("EndSession", ("Token", Token));
        }

        /// <summary>Submits an XML request in a sessionless environment.</summary>
        public XmlElement SubmitXml(string Profile, XmlElement Request, XmlElement Filter)
        {
            return InvokeXml("SubmitXml", "SubmitXmlResult", ("Profile", Profile), ("Request", Request), ("Filter", Filter));
        }

        /// <summary>Submits an XML request on the specified session.</summary>
        public XmlElement SubmitXmlOnSession(string Token, XmlElement Request, XmlElement Filter)
        {
            return InvokeXml("SubmitXmlOnSession", "SubmitXmlOnSessionResult", ("Token", Token), ("Request", Request), ("Filter", Filter));
        }

        /// <summary>Submits a terminal (cryptic) transaction on a session and returns the screen result.</summary>
        public string SubmitTerminalTransaction(string Token, string Request, string IntermediateResponse)
        {
            return InvokeString("SubmitTerminalTransaction", "SubmitTerminalTransactionResult",
                ("Token", Token), ("Request", Request), ("IntermediateResponse", IntermediateResponse));
        }

        /// <summary>Sends multiple sessionless transactions within a single call.</summary>
        public XmlElement MultiSubmitXml(string Profile, XmlElement Requests)
        {
            return InvokeXml("MultiSubmitXml", "Responses", ("Profile", Profile), ("Requests", Requests));
        }
        #endregion

        #region ImageViewer operations
        /// <summary>
        /// ImageViewer (HotelImage) RetrievePhotoInformation. Posts to <see cref="Url"/>;
        /// construct the client with <see cref="DefaultImageViewerUrl"/> as the legacy
        /// proxies did. SOAPAction is http-prefixed for both prod and copy proxies.
        /// The legacy adapter never populated the CallContext SOAP header, so none is sent.
        /// </summary>
        public XmlElement RetrievePhotoInformation(XmlElement request)
        {
            XmlElement result = SendCore(
                Url,
                "http://webservices.galileo.com/HotelImage/RetrievePhotoInformation",
                "RetrievePhotoInformation",
                HotelImageNs,
                "RetrievePhotoInformationReturn",
                new (string Name, object Value)[] { ("request", request) });
            return FirstElementChild(result);
        }
        #endregion

        #region Invocation core
        private void Invoke(string operation, params (string Name, object Value)[] members)
        {
            SendCore(Url, _soapActionPrefix + operation, operation, XmlSelectNs, null, members);
        }

        private string InvokeString(string operation, string resultName, params (string Name, object Value)[] members)
        {
            XmlElement result = SendCore(Url, _soapActionPrefix + operation, operation, XmlSelectNs, resultName, members);
            // SoapHttpClientProtocol parity: missing member -> null, empty element -> "".
            return result?.InnerText;
        }

        private XmlElement InvokeXml(string operation, string resultName, params (string Name, object Value)[] members)
        {
            XmlElement result = SendCore(Url, _soapActionPrefix + operation, operation, XmlSelectNs, resultName, members);
            // SoapHttpClientProtocol parity: the proxy returned the first element child of
            // the result wrapper (and silently dropped any further siblings).
            return FirstElementChild(result);
        }

        /// <summary>
        /// Sends one SOAP 1.1 call and returns the result wrapper element
        /// (e.g. &lt;SubmitXmlResult&gt;) from the response, or null when
        /// <paramref name="resultName"/> is null (void operations) or the element is absent.
        /// </summary>
        private XmlElement SendCore(string url, string soapAction, string operation, string operationNamespace,
            string resultName, (string Name, object Value)[] members)
        {
            byte[] envelope = BuildEnvelope(operation, operationNamespace, members);

            using var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Content = new ByteArrayContent(envelope);
            request.Content.Headers.TryAddWithoutValidation("Content-Type", "text/xml; charset=utf-8");
            request.Headers.TryAddWithoutValidation("SOAPAction", "\"" + soapAction + "\"");
            request.Headers.TryAddWithoutValidation("User-Agent", LegacyUserAgent);

            // Pre-emptive Basic auth: the legacy stack resolved the credential from the
            // CredentialCache by URI prefix and "Basic" auth type. When no credential
            // matches (e.g. the ImageViewer URL was never registered in the cache), the
            // request goes out unauthenticated — exactly as before.
            NetworkCredential credential = Credentials?.GetCredential(new Uri(url), "Basic");
            if (credential != null)
            {
                string token = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{credential.UserName}:{credential.Password}"));
                request.Headers.TryAddWithoutValidation("Authorization", $"Basic {token}");
            }

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(Timeout > 0 ? Timeout : DefaultTimeoutMs));
            HttpResponseMessage response;
            try
            {
                response = _httpClient.Send(request, HttpCompletionOption.ResponseContentRead, cts.Token);
            }
            catch (OperationCanceledException ex) when (cts.IsCancellationRequested)
            {
                // Legacy parity: HttpWebRequest surfaced "The operation has timed out".
                throw new WebException("The operation has timed out", ex, WebExceptionStatus.Timeout, null);
            }

            using (response)
            {
                int status = (int)response.StatusCode;

                // SoapHttpClientProtocol only attempted to read a SOAP body for 200/400/500;
                // anything else surfaced as a protocol-level WebException.
                if (status != 200 && status != 400 && status != 500)
                {
                    throw new WebException(
                        $"The request failed with HTTP status {status}: {response.ReasonPhrase}.",
                        WebExceptionStatus.ProtocolError);
                }

                var doc = new XmlDocument();
                try
                {
                    using Stream stream = response.Content.ReadAsStream();
                    doc.Load(stream);
                }
                catch (XmlException ex)
                {
                    if (status != 200)
                    {
                        throw new WebException(
                            $"The request failed with HTTP status {status}: {response.ReasonPhrase}.",
                            WebExceptionStatus.ProtocolError);
                    }
                    throw new InvalidOperationException("Response is not well-formed XML.", ex);
                }

                XmlElement body = FindChild(doc.DocumentElement, "Body", SoapNs)
                    ?? throw new InvalidOperationException("Response is not a valid SOAP envelope.");
                XmlElement first = FirstElementChild(body);

                if (first != null && first.LocalName == "Fault" && first.NamespaceURI == SoapNs)
                {
                    throw CreateFaultException(first);
                }

                if (status != 200)
                {
                    throw new WebException(
                        $"The request failed with HTTP status {status}: {response.ReasonPhrase}.",
                        WebExceptionStatus.ProtocolError);
                }

                if (resultName == null || first == null)
                {
                    return null;
                }

                // first = response wrapper, e.g. <SubmitXmlResponse>; locate the result member.
                return FindChild(first, resultName, operationNamespace) ?? FindChild(first, resultName, null);
            }
        }

        private static GalileoSoapFaultException CreateFaultException(XmlElement fault)
        {
            // SOAP 1.1 fault children (faultcode/faultstring/detail) are unqualified.
            string faultCode = null, faultString = null;
            XmlElement detail = null;
            foreach (XmlNode node in fault.ChildNodes)
            {
                if (node is not XmlElement el) continue;
                switch (el.LocalName)
                {
                    case "faultcode": faultCode = el.InnerText; break;
                    case "faultstring": faultString = el.InnerText; break;
                    case "detail": detail = el; break;
                }
            }
            // SoapException parity: Message == faultstring.
            return new GalileoSoapFaultException(faultString ?? "SOAP fault", faultCode, detail?.OuterXml);
        }
        #endregion

        #region Envelope construction
        /// <summary>
        /// Builds the SOAP 1.1 request envelope, byte-identical to the legacy
        /// SoapHttpClientProtocol wrapped document/literal serialization:
        /// no BOM, no indentation, soap/xsi/xsd prefix declarations in that order,
        /// member elements in declared order inside the operation wrapper, string members
        /// as text elements (empty string =&gt; empty element), XmlElement members written
        /// literally inside their wrapper member element, null members omitted.
        /// </summary>
        private static byte[] BuildEnvelope(string operation, string operationNamespace, (string Name, object Value)[] members)
        {
            using var ms = new MemoryStream();
            var settings = new XmlWriterSettings
            {
                Encoding = new UTF8Encoding(false),
                Indent = false,
                CloseOutput = false
            };
            using (XmlWriter writer = XmlWriter.Create(ms, settings))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("soap", "Envelope", SoapNs);
                writer.WriteAttributeString("xmlns", "soap", null, SoapNs);
                writer.WriteAttributeString("xmlns", "xsi", null, XsiNs);
                writer.WriteAttributeString("xmlns", "xsd", null, XsdNs);
                writer.WriteStartElement("soap", "Body", SoapNs);
                writer.WriteStartElement(operation, operationNamespace);

                foreach ((string name, object value) in members)
                {
                    switch (value)
                    {
                        case null:
                            // minOccurs="0": the legacy serializer omitted null members.
                            break;
                        case string s:
                            writer.WriteStartElement(name, operationNamespace);
                            if (s.Length > 0)
                            {
                                writer.WriteString(s);
                            }
                            writer.WriteEndElement();
                            break;
                        case XmlElement element:
                            writer.WriteStartElement(name, operationNamespace);
                            element.WriteTo(writer);
                            writer.WriteEndElement();
                            break;
                        default:
                            throw new InvalidOperationException($"Unsupported SOAP member type: {value.GetType()}");
                    }
                }

                writer.WriteEndElement(); // operation wrapper
                writer.WriteEndElement(); // soap:Body
                writer.WriteEndElement(); // soap:Envelope
                writer.WriteEndDocument();
            }
            return ms.ToArray();
        }
        #endregion

        #region XML helpers
        private static XmlElement FirstElementChild(XmlElement parent)
        {
            if (parent == null) return null;
            foreach (XmlNode node in parent.ChildNodes)
            {
                if (node is XmlElement el) return el;
            }
            return null;
        }

        private static XmlElement FindChild(XmlElement parent, string localName, string ns)
        {
            if (parent == null) return null;
            foreach (XmlNode node in parent.ChildNodes)
            {
                if (node is XmlElement el && el.LocalName == localName && (ns == null || el.NamespaceURI == ns))
                {
                    return el;
                }
            }
            return null;
        }
        #endregion
    }

    /// <summary>
    /// Replacement for System.Web.Services.Protocols.SoapException: thrown when the
    /// service returns a SOAP fault. <see cref="Exception.Message"/> carries the
    /// faultstring exactly, which is what every legacy call site keys off.
    /// </summary>
    public class GalileoSoapFaultException : Exception
    {
        /// <summary>The SOAP 1.1 faultcode value.</summary>
        public string FaultCode { get; }

        /// <summary>OuterXml of the fault &lt;detail&gt; element, when present.</summary>
        public string Detail { get; }

        public GalileoSoapFaultException(string faultString, string faultCode, string detail)
            : base(faultString)
        {
            FaultCode = faultCode;
            Detail = detail;
        }
    }
}
