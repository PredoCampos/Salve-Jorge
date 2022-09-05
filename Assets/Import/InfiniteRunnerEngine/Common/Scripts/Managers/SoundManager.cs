using UnityEngine;
using System.Collections;
using MoreMountains.Tools;
using System;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine.Audio;

namespace MoreMountains.InfiniteRunnerEngine
{	
	[Serializable]
	public class SoundSettings
	{
		public bool MusicOn = true;
		public bool SfxOn = true;
	}

    public struct IRESfxEvent
    {
        public delegate void Delegate(AudioClip clipToPlay, AudioMixerGroup audioGroup = null, float volume = 1f, float pitch = 1f);
        static private event Delegate OnEvent;

        static public void Register(Delegate callback)
        {
            OnEvent += callback;
        }

        static public void Unregister(Delegate callback)
        {
            OnEvent -= callback;
        }

        static public void Trigger(AudioClip clipToPlay, AudioMixerGroup audioGroup = null, float volume = 1f, float pitch = 1f)
        {
            OnEvent?.Invoke(clipToPlay, audioGroup, volume, pitch);
        }
    }


    /// <summary>
    /// This persistent singleton handles sound playing
    /// </summary>
    [AddComponentMenu("Infinite Runner Engine/Managers/Sound Manager")]
	public class SoundManager : MMPersistentSingleton<SoundManager>
	{	
		[Header("Settings")]
		public SoundSettings Settings;

		[Header("Music")]
		/// true if the music is enabled	
		//public bool MusicOn=true;
		/// the music volume
		[Range(0,1)]
		public float MusicVolume=0.3f;

		[Header("Sound Effects")]
		/// true if the sound fx are enabled
		//public bool SfxOn=true;
		/// the sound fx volume
		[Range(0,1)]
		public float SfxVolume=1f;

		[Header("Pause")]
		public bool MuteSfxOnPause = true;

		protected const string _saveFolderName = "InfiniteRunnerEngine/";
		protected const string _saveFileName = "sound.settings";

        [Header("Tests")]
        [MMInspectorButton("ToggleMusic")]
        public bool MusicToggleButton;
        [MMInspectorButton("ToggleSfx")]
        public bool SfxToggleButton;

	    protected AudioSource _backgroundMusic;	
		protected List<AudioSource> _loopingSounds;
			
		/// <summary>
		/// Plays a background music.
		/// Only one background music can be active at a time.
		/// </summary>
		/// <param name="Clip">Your audio clip.</param>
		public virtual void PlayBackgroundMusic(AudioSource Music)
		{
			// if the music's been turned off, we do nothing and exit
			if (!Settings.MusicOn)
            {
                return;
            }				
			// if we already had a background music playing, we stop it
			if (_backgroundMusic!=null)
            {
                _backgroundMusic.Stop();
            }				
			// we set the background music clip
			_backgroundMusic=Music;
			// we set the music's volume
			_backgroundMusic.volume=MusicVolume;
			// we set the loop setting to true, the music will loop forever
			_backgroundMusic.loop=true;
			// we start playing the background music
			_backgroundMusic.Play();		
		}	

        /// <summary>
        /// Stops the background music, if there's one
        /// </summary>
        public virtual void StopBackgroundMusic()
        {
            if (_backgroundMusic != null)
            {
                _backgroundMusic.Stop();
            }
        }
		
		/// <summary>
		/// Plays a sound
		/// </summary>
		/// <returns>An audiosource</returns>
		/// <param name="sfx">The sound clip you want to play.</param>
		/// <param name="location">The location of the sound.</param>
		/// <param name="loop">If set to true, the sound will loop.</param>
		public virtual AudioSource PlaySound(AudioClip sfx, Vector3 location, bool loop=false)
		{
			if (!Settings.SfxOn)
				return null;
			// we create a temporary game object to host our audio source
			GameObject temporaryAudioHost = new GameObject("TempAudio");
			// we set the temp audio's position
			temporaryAudioHost.transform.position = location;
			// we add an audio source to that host
			AudioSource audioSource = temporaryAudioHost.AddComponent<AudioSource>() as AudioSource; 
			// we set that audio source clip to the one in paramaters
			audioSource.clip = sfx; 
			// we set the audio source volume to the one in parameters
			audioSource.volume = SfxVolume;
			// we set our loop setting
			audioSource.loop = loop;
			// we start playing the sound
			audioSource.Play(); 

			if (!loop)
			{
				// we destroy the host after the clip has played
				Destroy(temporaryAudioHost, sfx.length);
			}
			else
			{
				_loopingSounds.Add (audioSource);
			}

			// we return the audiosource reference
			return audioSource;
		}

		/// <summary>
		/// Stops the looping sounds if there are any
		/// </summary>
		/// <param name="source">Source.</param>
		public virtual void StopLoopingSound(AudioSource source)
		{
			if (source != null)
			{
				_loopingSounds.Remove (source);
				Destroy(source.gameObject);
			}
		}

        /// <summary>
        /// Sets the music to the specified setting, and stops background music if there's one playing
        /// </summary>
        /// <param name="status"></param>
		protected virtual void SetMusic(bool status)
		{
			Settings.MusicOn = status;
			SaveSoundSettings ();
            if ((Settings.MusicOn) && (_backgroundMusic != null))
            {
                _backgroundMusic.mute = false;
            }
            if ((!Settings.MusicOn) && (_backgroundMusic != null))
            {
                _backgroundMusic.mute = true;
            }
        }

		protected virtual void SetSfx(bool status)
		{
			Settings.SfxOn = status;
			SaveSoundSettings ();
		}

		public virtual void MusicOn()
        {
            SetMusic (true);
        }

		public virtual void MusicOff()
        {
            SetMusic (false);
        }

		public virtual void SfxOn()
        {
            SetSfx (true);
        }

		public virtual void SfxOff()
        {
            SetSfx (false);
        }

        public virtual void ToggleMusic()
        {
            SetMusic(!Settings.MusicOn);
        }

        public virtual void ToggleSfx()
        {
            SetSfx(!Settings.SfxOn);
        }

		protected virtual void SaveSoundSettings()
		{
			MMSaveLoadManager.Save(Settings, _saveFileName, _saveFolderName);
		}

		protected virtual void LoadSoundSettings()
		{
			SoundSettings settings = (SoundSettings)MMSaveLoadManager.Load(typeof(SoundSettings), _saveFileName, _saveFolderName);
			if (settings != null)
			{
				Settings = settings;
			}
		}

        public virtual AudioSource GetBackgroundMusic()
        {
            return _backgroundMusic;
        }

        protected virtual void ResetSoundSettings()
		{
			MMSaveLoadManager.DeleteSave(_saveFileName, _saveFolderName);
		}

        public virtual void OnMMSfxEvent(AudioClip clipToPlay, AudioMixerGroup audioGroup = null, float volume = 1f, float pitch = 1f)
        {
            PlaySound(clipToPlay, this.transform.position);
        }

        protected virtual void MuteAllSfx()
		{
			foreach(AudioSource source in _loopingSounds)
			{
				if (source != null)
				{
					source.mute = true;	
				}
			}
		}

		protected virtual void UnmuteAllSfx()
		{
			foreach(AudioSource source in _loopingSounds)
			{
				if (source != null)
				{
					source.mute = false;	
				}
			}
		}

		protected virtual void OnEnable()
		{
            IRESfxEvent.Register(OnMMSfxEvent);
            LoadSoundSettings ();
			_loopingSounds = new List<AudioSource> ();
		}

		protected virtual void OnDisable()
		{
			if (_enabled)
            {
                IRESfxEvent.Unregister(OnMMSfxEvent);
            }
		}
	}
}