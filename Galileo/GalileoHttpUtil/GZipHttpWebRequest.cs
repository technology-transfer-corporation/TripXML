using System;
using System.Web;
using System.IO;
using System.Net;
using System.Runtime.Serialization;
using System.Xml;

// #ZipLib was developed by Mike Krueger, and is available under the GNU Public License at:
//		http://www.icsharpcode.net/OpenSource/SharpZipLib/Default.aspx
using ICSharpCode.SharpZipLib.GZip;

namespace Galileo.Web
{
	/// <summary>
	/// HttpWebRequest with gzip decompression
	/// The source for this class is based on this article on dotnetjunkies.com:
	///		http://www.dotnetjunkies.com/Tutorial/46630AE2-1C79-4D5F-827E-6C2857FF1D23.dcik
	/// </summary>
	[Serializable]
	public class GZipHttpWebRequest : WebRequest
	{
		private HttpWebRequest request;

		public GZipHttpWebRequest(Uri uri)
		{
			this.request = (HttpWebRequest)HttpWebRequest.Create(uri);
		}

		public override Stream GetRequestStream()
		{
			if (request.Headers["Content-Encoding"]=="gzip")
			{
				// Compress request stream with GZip
				Stream compressedStream = new GZipOutputStream(request.GetRequestStream());
				return compressedStream;
			}
			else
			{
				return request.GetRequestStream();
			}
		}

		public bool KeepAlive { get { return request.KeepAlive; } set { request.KeepAlive = value; } }
		public Version ProtocolVersion { get { return request.ProtocolVersion; } set { request.ProtocolVersion = value; } }

		// encapsulated WebRequest methods and properties
		public override void Abort()
		{
			request.Abort();
		}

		public override IAsyncResult BeginGetRequestStream(AsyncCallback callback, object state)
		{
			return request.BeginGetRequestStream(callback, state);
		}

		public override IAsyncResult BeginGetResponse(AsyncCallback callback, object state)
		{
			return request.BeginGetResponse(callback, state);
		}

		public override Stream EndGetRequestStream(IAsyncResult asyncResult)
		{
			if (request.Headers["Content-Encoding"]=="gzip")
			{
				// Compress request stream with GZip
				Stream compressedStream = new GZipOutputStream(request.EndGetRequestStream(asyncResult));
				return compressedStream;
			}
			else
			{
				return request.EndGetRequestStream(asyncResult);
			}
		}

		public override WebResponse EndGetResponse(IAsyncResult asyncResult)
		{
			return request.EndGetResponse(asyncResult);
		}

		public override WebResponse GetResponse()
		{
			return request.GetResponse();
		}

		public override string ConnectionGroupName { get { return request.ConnectionGroupName; } set { request.ConnectionGroupName = value; } }
		public override long ContentLength { get { return request.ContentLength; } set { request.ContentLength = value; } }
		public override string ContentType { get { return request.ContentType; } set { request.ContentType = value; } }
		public override ICredentials Credentials { get { return request.Credentials; } set { request.Credentials = value; } }
		public override WebHeaderCollection Headers { get { return request.Headers; } set { request.Headers = value; } }
		public override string Method { get { return request.Method; } set { request.Method = value; } }
		public override bool PreAuthenticate { get { return request.PreAuthenticate; } set { request.PreAuthenticate = value; } }
		public override IWebProxy Proxy { get { return request.Proxy; } set { request.Proxy = value; } }
		public override Uri RequestUri { get { return request.RequestUri; } }
		public override int Timeout { get { return request.Timeout; } set { request.Timeout = value; } }
	}
}
