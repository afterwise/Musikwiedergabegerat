
using System.IO;
using UnityEngine;
using MP3Sharp;

namespace Musikwiedergabegerat {

public sealed class Mp3MusicPlayer : MusicPlayer {
	MP3Stream _stream;
	AudioClip _clip;
	byte[] _buffer;
	int _channelCount;
	int _frequency;

	public override void Open(Stream stream) {
		_stream = new MP3Stream(stream);
		_channelCount = _stream.ChannelCount;
		_frequency = _stream.Frequency;
	}

	public override void Close() {
		if (_stream != null) {
			_stream.Close();
			_stream = null;
		}
	}

	public override void Read(float[] data, int channels) {
		int m = data.Length << 1;
		if (_buffer == null || _buffer.Length != m)
			_buffer = new byte[m];
		int n = _stream.Read(_buffer, 0, m);
		int c = _channelCount;
		for (int j = 0; j < c; ++j) {
			int i = j << 1;
			for (; i < n; i += c << 1) {
				int a = (_buffer[i + 0] & 0xff) << 0;
				int b = (_buffer[i + 1] & 0xff) << 8;
				const float k = 1f / 0x7fff;
				data[i >> 1] = (short) (a + b) * k;
			}
			for (; i < m; i += c << 1)
				data[i >> 1] = 0f;
		}
	}
}

} // namespace

