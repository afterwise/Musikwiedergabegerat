
using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace Musikwiedergabegerat {

public sealed class HttpStream : Stream {
	HttpWebRequest _request;
	HttpWebResponse _response;
	Stream _stream;

	public override bool CanSeek { get { return _stream.CanSeek; }}
	public override bool CanRead { get { return _stream.CanRead; }}
	public override bool CanWrite { get { return _stream.CanWrite; }}
	public override long Length { get { return _stream.Length; }}
	
	public override long Position {
		get { return _stream.Position; }
		set { _stream.Position = value; }
	}

	public HttpStream(string uri) {
		ServicePointManager.ServerCertificateValidationCallback = OnValidateCertificate;
		_request = (HttpWebRequest) WebRequest.Create(uri);
		_response = (HttpWebResponse) _request.GetResponse();
		_stream = new BufferedStream(_response.GetResponseStream());
	}

	static bool OnValidateCertificate(
			object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors errors) {
		if (errors != SslPolicyErrors.None)
			for (int i = 0; i < chain.ChainStatus.Length; ++i)
				if (chain.ChainStatus[i].Status != X509ChainStatusFlags.RevocationStatusUnknown) {
					chain.ChainPolicy.RevocationFlag = X509RevocationFlag.EntireChain;
					chain.ChainPolicy.RevocationMode = X509RevocationMode.Online;
					chain.ChainPolicy.UrlRetrievalTimeout = new TimeSpan(0, 1, 0);
					chain.ChainPolicy.VerificationFlags = X509VerificationFlags.AllFlags;
					if (!chain.Build((X509Certificate2) cert))
						return false;
				}
		return true;
	}

	public override void SetLength(long length) {
		_stream.SetLength(length);
	}

	public override long Seek(long offset, SeekOrigin origin) {
		return _stream.Seek(offset, origin);
	}

	public override int Read(byte[] buffer, int offset, int count) {
		return _stream.Read(buffer, offset, count);
	}

	public override void Write(byte[] buffer, int offset, int count) {
		_stream.Write(buffer, offset, count);
	}

	public override void Flush() {
		_stream.Flush();
	}
}

} // namespace Musikwiedergabegerat

