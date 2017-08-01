
using System.IO;
using UnityEngine;

namespace Musikwiedergabegerat {

public abstract class MusicPlayer {
	public abstract void Open(Stream stream);
	public abstract void Close();
	public abstract void Read(float[] data, int channels);
}

} // namespace

