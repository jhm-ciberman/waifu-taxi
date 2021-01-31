using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; } // Static Instance

    [SerializeField] private Sound[] _sounds = null;
    [SerializeField] private MusicClip[] _music = null;

    private Dictionary<string, Sound> _soundsDictionary;            // Utility Dictionary used for performance at memory cost
	private Dictionary<string, MusicClip> _musicDictionary;         // Utility Dictionary used for performance at memory cost
	private AudioSource _musicSource;
	private List<string> _activePlayingSoundClips;

	public void PlaySound(string soundClipID, bool looped = false, Action<string> soundEndCallback = null)
	{
		if (_soundsDictionary.TryGetValue(soundClipID, out Sound sound))
		{
			sound.Source.loop = looped;
			sound.Source.Play();

			if (soundEndCallback != null)
			{
				sound.SetSoundEndedCallback(soundEndCallback);

				if (!ActiveSoundsContains(soundClipID))
				{
					_activePlayingSoundClips.Add(soundClipID);
				}
			}
		}
		else
		{
			Debug.LogWarning($"Sound Clip ID '{soundClipID}' no existe. <color=red>No me hagas calentar mostro!</color>");
		}
	}

	public void PlayMusic(string musicClipID, bool looped)
	{
		if (_musicDictionary.TryGetValue(musicClipID, out MusicClip musicClip))
		{
			// First stop the current playing music
			_musicSource.Stop();

			// Then configure and play the new music clip
			_musicSource.clip = musicClip.SoundClip;
			_musicSource.volume = musicClip.Volume;
			_musicSource.pitch = musicClip.Pitch;
			_musicSource.loop = looped;
			_musicSource.Play();
		}
		else
		{
			Debug.LogWarning($"Music Clip ID '{musicClipID}' no existe. <color=red>VOS SOS O TE HACÉS!?</color>");
		}
	}

	public void StopSound(string soundClipID, float fadeOutTime = 0.0f)
	{
		if (fadeOutTime < 0.0f)
		{
			Debug.LogError("Fade out time negativo? Pero vo' so' loco viteh?!");
			return;
		}
		
		if (_soundsDictionary.TryGetValue(soundClipID, out Sound sound))
		{
			if (fadeOutTime > 0.0f)
			{
				StartCoroutine(FadeOut(soundClipID, sound.Source, fadeOutTime));
			}
			else
			{
				sound.Source.Stop();
				
				sound.SoundEnded();
				
				_activePlayingSoundClips.Remove(soundClipID);
			}
		}
		else
		{
			Debug.LogWarning($"Sound Clip ID '{soundClipID}' no existe! <color=white>Dame paciencia señor!</color>");
		}
	}

	public void StopPlayingMusic()
	{
		_musicSource.Stop();
	}
	
	protected void Awake()
	{
		// Initialize the static reference
		//if (Instance == null)
		//{
			Instance = this;
			//DontDestroyOnLoad(gameObject);
            
			Initialize();
		//}
		//else
		//{
		//	Destroy(gameObject);
		//}
	}

	protected void Update()
	{
		for (int i = _activePlayingSoundClips.Count - 1; i >= 0; i--)
		{
			string soundClipID = _activePlayingSoundClips[i];
			
			if (_soundsDictionary.TryGetValue(soundClipID, out Sound sound))
			{
				if (!sound.Source.isPlaying)
				{
					// The soundClip is no longer playing
					sound.SoundEnded();

					_activePlayingSoundClips.RemoveAt(i);
				}
			}
		}
	}

	private void Initialize()
	{
		// Initialize the Dictionaries
		_soundsDictionary = new Dictionary<string, Sound>();
		_musicDictionary = new Dictionary<string, MusicClip>();

		// Initialize the utility collection used to store active playing sound ids
		_activePlayingSoundClips = new List<string>();

		// Initialize the music audio source
		_musicSource = gameObject.AddComponent<AudioSource>();

		// Add music clips to the dictionary
		for (int index = 0, length = _music.Length; index < length; index++)
		{
			MusicClip currentMusicClip = _music[index];
			_musicDictionary.Add(currentMusicClip.ID, currentMusicClip);
		}

		// Initialize the sound clips audio sources and the sounds dictionary
		for (int index = 0, length = _sounds.Length; index < length; index++)
		{
			Sound currentSound = _sounds[index];
			currentSound.Initialize(gameObject.AddComponent<AudioSource>());

			// Add the sound to the dictionary
			_soundsDictionary.Add(currentSound.ID, currentSound);
		}
	}
	
	private IEnumerator FadeOut (string soundClipID, AudioSource audioSource, float fadeTime) 
	{
		float startVolume = audioSource.volume;
 
		while (audioSource.volume > 0) {
			audioSource.volume -= startVolume * Time.deltaTime / fadeTime;
 
			yield return null;
		}
 
		audioSource.Stop();
		audioSource.volume = startVolume;

		_activePlayingSoundClips.Remove(soundClipID);
	}

	private bool ActiveSoundsContains(string id)
	{
		foreach (var playingSoundClipID in _activePlayingSoundClips)
		{
			if (playingSoundClipID.Equals(id, StringComparison.Ordinal))
			{
				return true;
			}
		}

		return false;
	}
}

[Serializable]
public class Sound
{
    public string ID;
    public AudioClip SoundClip;

    [Range(0f, 1f)]
    public float Volume = 1.0f;
    [Range(.1f, 3f)]
    public float Pitch = 1.0f;
    public bool Loop;
    [HideInInspector]
    public AudioSource Source;

    private Action<string> _soundEndedCallback;
    
    /// <summary>
    /// Assigns an audio source component reference to this Sound and setups the audio source settings
    /// with the ones from this Sound object.
    /// </summary>
    public void Initialize(AudioSource audioSource)
    {
        Source = audioSource;
        Source.clip = SoundClip;
        Source.volume = Volume;
        Source.pitch = Pitch;
        Source.loop = Loop;
    }

    public void SetSoundEndedCallback(Action<string> callback)
    {
	    _soundEndedCallback = callback;
    }

    public void SoundEnded()
    {   
	    _soundEndedCallback?.Invoke(ID);
	    _soundEndedCallback = null;
    }

}

[Serializable]
public class MusicClip
{
    public string ID;
    public AudioClip SoundClip;
    [Range(0f, 1f)]
    public float Volume;
    [Range(.1f, 3f)]
    public float Pitch;
}
