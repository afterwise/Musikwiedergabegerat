
using System.IO;
using UnityEngine;
using Musikwiedergabegerat;

public sealed class Jukebox : MonoBehaviour {
	MusicPlayer _player;
	AudioSource _source;

	readonly string uri = "https://files.freemusicarchive.org/music%2Fno_curator%2FRyan_Little%2FRyan_Little_-_Singles%2FRyan_Little_-_La_Besitos.mp3";

	void OnEnable() {
		_player = new Mp3MusicPlayer();
		_player.Open(new HttpStream(uri));
		_source = GetComponent<AudioSource>();
		_source.Play();
	}

	void OnDisable() {
		_source.Stop();
		_source = null;
		_player.Close();
		_player = null;
	}

	void OnAudioFilterRead(float[] data, int channels) {
		if (_player != null)
			_player.Read(data, channels);
	}
}

