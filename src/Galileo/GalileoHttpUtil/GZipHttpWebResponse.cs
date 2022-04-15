using System;
using System.IO;
using System.Net;

// #ZipLib was developed by Mike Krueger, and is available under the GNU Public License at:
//		http://www.icsharpcode.net/OpenSource/SharpZipLib/Default.aspx
using ICSharpCode.SharpZipLib.GZip;

namespace Galileo.Web
{
	/// <summary>
	/// HttpWebResponse with gzip decompression
	/// The source for this class is based on this article on dotnetjunkies.com:
	///		http://www.dotnetjunkies.com/Tutorial/46630AE2-1C79-4D5F-827E-6C2857FF1D23.dcik
	/// </summary>
	[Serializable]
	public class GZipHttpWebResponse : WebResponse
	{
		private Stream mDecompressedStream;
		private string _contentType;
		private long _contentLength;
		private string _contentEncoding;
		private readonly Uri _responseUri;
		private readonly WebHeaderCollection _httpResponseHeaders;

		//		/// <summary>
		//		/// default constructor from WebResponse
		//		/// </summary>
		//		/// <param name="serializationInfo"></param>
		//		/// <param name="streamingContext"></param>
		//		protected GZipHttpWebResponse(
		//			SerializationInfo serializationInfo,
		//			StreamingContext streamingContext
		//			) : base (serializationInfo, streamingContext)
		//		{
		//		}
		//		
		public override long ContentLength
		{
			set 
			{
				_contentLength = value;
			}

			get
			{
				return _contentLength;
			}
		}
		
		public override string ContentType
		{
			set 
			{
				_contentType = value;
			}

			get 
			{
				return _contentType;
			}
		}

		public override WebHeaderCollection Headers
		{
			get
			{
				return _httpResponseHeaders;
			}
		}

		public string ContentEncoding
		{
			set 
			{
				_contentEncoding = value;
			}

			get 
			{
				return _contentEncoding;
			}
		}

		public GZipHttpWebResponse(HttpWebResponse response)
		{
			try
			{
				_contentType = response.ContentType;
				_contentLength = response.ContentLength;
				_contentEncoding = response.ContentEncoding;
				_responseUri = response.ResponseUri;
				_httpResponseHeaders = response.Headers;
				if (_contentEncoding=="gzip")
				{
					// Decompress response stream with GZip
					const int BUF_SIZE = 16384;
					Stream compressedStream = new GZipInputStream(response.GetResponseStream(), BUF_SIZE);
					mDecompressedStream = new MemoryStream();
	
					var writeData = new byte[BUF_SIZE];
					int size = compressedStream.Read(writeData, 0, BUF_SIZE);
					while (size > 0)
					{
						mDecompressedStream.Write(writeData, 0, size);
						size = compressedStream.Read(writeData, 0, BUF_SIZE);
					}
					mDecompressedStream.Seek(0, SeekOrigin.Begin);
					_contentEncoding = "";
					_httpResponseHeaders.Remove("Content-Encoding");
					_contentLength = mDecompressedStream.Length;
					// We have decompressed the entire response, so we no longer need the compressed version.
					response.Close();
				}
				else
				{
					mDecompressedStream = response.GetResponseStream();
				}
			}
			catch(Exception)
			{
				// We got an exception processing the response. Clean up.
				response.Close();
				Close();
				throw;
			}
		}

		~GZipHttpWebResponse()
		{
			Close();
		}

		public override Stream GetResponseStream()
		{
			return mDecompressedStream;
		}

		public override sealed void Close()
		{
			if(mDecompressedStream != null)
			{
				mDecompressedStream.Close();
				mDecompressedStream = null;
			}
		}

		public override Uri ResponseUri { get { return _responseUri; } }
	}
}
