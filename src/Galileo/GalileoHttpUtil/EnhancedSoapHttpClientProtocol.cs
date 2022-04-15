using System;
using System.Net;
using System.Text;

namespace Galileo.Web.Services
{
	/// <remarks/>
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	public class EnhancedSoapHttpClientProtocol : System.Web.Services.Protocols.SoapHttpClientProtocol
	{
		public bool Expect100Continue { get { return _expect100Continue; } set { _expect100Continue = value; } } 
		public bool KeepAlive { get { return _keepAlive; } set { _keepAlive = value; } } 
		public bool UseNagleAlgorithm { get { return _useNagleAlgorithm; } set { _useNagleAlgorithm = value; } }
		public bool AcceptGZip { get { return _acceptGZip; } set { _acceptGZip = value; } }
		public bool GZipRequest { get { return _gZipRequest;} set { _gZipRequest = value; } }
		public Version ProtocolVersion { get { return _protocolVersion;} set { _protocolVersion = value; } }

		private bool _expect100Continue;
		private bool _keepAlive;
		private bool _useNagleAlgorithm;
		private bool _acceptGZip;
		private bool _gZipRequest;
		private Version _protocolVersion;

		/// <remarks/>
		public EnhancedSoapHttpClientProtocol() 
		{
			_expect100Continue = false;
			_keepAlive = false;
			_useNagleAlgorithm = false;
			_gZipRequest = true;
			_acceptGZip = true;
			_protocolVersion =  HttpVersion.Version11;
		}

		protected override WebRequest GetWebRequest(Uri uri)
		{
			// The following disables 100-continue for web service requests,
			// and must be done BEFORE the WebRequest object is created.
			// expect100Continue is valid only for HTTP 1.1 and above
			ServicePoint sp = ServicePointManager.FindServicePoint(uri);
			sp.Expect100Continue = _expect100Continue && _protocolVersion >= HttpVersion.Version11;
			sp.UseNagleAlgorithm = _useNagleAlgorithm;

			// Declare locals.
			WebRequest request;

			// Grab the instance of the request that will be used to service this call
			if(_gZipRequest)
			{
			    GZipHttpWebRequest gZipRequest = new GZipHttpWebRequest(uri)
			    {
			        Headers = {["Content-Encoding"] = "gzip"},
			        KeepAlive = _keepAlive && _protocolVersion >= HttpVersion.Version11,
			        ProtocolVersion = _protocolVersion
			    };
			    // keepAlive is valid only for HTTP 1.1 and above
			    request = gZipRequest;
			}
			else
			{
				HttpWebRequest httpRequest = (HttpWebRequest)base.GetWebRequest(uri);
				// keepAlive is valid only for HTTP 1.1 and above
				httpRequest.KeepAlive = _keepAlive && _protocolVersion >= HttpVersion.Version11;
				httpRequest.ProtocolVersion = _protocolVersion;
				request = httpRequest;
			}

			// The SoapHttpClientProtocol does not respect the Preauthenticate property.
			// The following code ensures that we preauthenticate when this property is set to true.
			if(PreAuthenticate)
			{
				NetworkCredential nc = Credentials.GetCredential(uri,"Basic");
				if(nc != null)
				{
					string credentials = $"{nc.UserName}:{nc.Password}";
					string encodedCredentials = Convert.ToBase64String(Encoding.UTF8.GetBytes(credentials));
					request.Headers["Authorization"] = $"Basic {encodedCredentials}";
				}
			}

			if(_acceptGZip)
			{
				request.Headers["Accept-Encoding"] = "gzip";
			}

			// The HttpWebRequest.KeepAlive property controls whether or not
			// the value KeepAlive is sent in the the HTTP 1.1 Connection Header.
			// KeepAlive controls whether the ServicePoint class will cache
			// this socket connection and reuse it for subsequent requests.
			// In some circumstances, connection pooling can provide a
			// performance benefit. However, the ServicePoint's implementation of
			// connection pooling can also result in errors when pooled
			// connections are closed or reset by a server, proxy server, firewall,
			// or other network device that is part of the communications path.
			// For most applications, reliability will be more important than the
			// performance gain achieved by connection pooling, and it is
			// Galileo's recommendation to NOT use connection pooling when
			// invoking web services.

			// The SoapHttpClientProtocol class does not provide any way to
			// override the default value (false) of HttpWebRequest.KeepAlive.
			// This class adds a KeepAlive property and then propogates it to
			// to the HttpWebRequest.KeepAlive property in the code above that creates
			// the WebRequest instance above. 

			// Return the request object.
			return request;
		}

		protected override WebResponse GetWebResponse(WebRequest request)
		{
			if(_acceptGZip)
			{
				HttpWebResponse httpResponse = (HttpWebResponse) request.GetResponse();
				GZipHttpWebResponse gZipResponse = new GZipHttpWebResponse(httpResponse);
				return gZipResponse;
			}
			else
			{
				return base.GetWebResponse(request);
			}
		}

		protected override WebResponse GetWebResponse(WebRequest request, IAsyncResult ar)
		{
			if(_acceptGZip)
			{
				HttpWebResponse httpResponse = (HttpWebResponse) request.EndGetResponse(ar);
				GZipHttpWebResponse gZipResponse = new GZipHttpWebResponse(httpResponse);
				return gZipResponse;
			}
			else
			{
				return base.GetWebResponse(request, ar);
			}
		}
	}
}
