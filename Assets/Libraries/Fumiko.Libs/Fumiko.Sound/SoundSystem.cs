using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Audio;
using Fumiko.Systems.Debug;

namespace Fumiko.Sound
{
    public class SoundSystem : MonoBehaviour
    {
        public static SoundSystem instance;

        public AudioSource fxSource;
        public AudioSource musicSource;
        public AudioSource ambientSource;

        [HideInInspector]
        public float fadingTime = 1.0f;

        private bool fadeOut;
        private bool fadeIn;
        private float fadingTargetVolume;
        private Music fadingTarget;

        public float soundFXVolumeMax = 1.0f;
        public float musicVolumeMax = 1.0f;
        public float ambientVolumeMax = 1.0f;

        private AudioClip chainMusicFile;

        public Music currentMusic;

        [HideInInspector]
        public Music originalMusic;
        [HideInInspector]
        public float originalLevel;

        public AudioMixer MainMixer;

        [System.Serializable]
        public struct SoundFiles
        {
            public AudioClip NONE;
            public AudioClip TEST_SOUND_FILE;
        }

        [System.Serializable]
        public struct MusicFiles
        {
            public string NONE;
            public string TEST_MUSIC_FILE;
        }

        [System.Serializable]
        public struct AmbientFiles
        {
            public AudioClip NONE;
            public AudioClip TEST_AMBIENT_FILE;
        }

        [SerializeField]
        private SoundFiles SoundFileLibrary;

        [SerializeField]
        private MusicFiles MusicFileLibrary;

        [SerializeField]
        private AmbientFiles AmbientFileLibrary;

        public List<GameObject> EnemiesNearby;

        public bool forceNoLoop;

        private int entities;

        private bool fadeQueueBackIn;

        private float queueTargetLevel;

        void Update()
        {
            if (musicSource.clip == null)
            {
                return;
            }

            if (forceNoLoop)
            {
                musicSource.loop = false;
            }

            HandleFade();
            HandleQueue();
            HandleCombatMusic();
        }

        private void Awake()
        {
            if (instance != null)
            {
                LogSystem.instance.DuplicateSingletonError();
            }
            else
            {
                instance = this;
            }
        }

        int ActiveEnemiesNearby()
        {
            entities = 0;
            foreach (GameObject entity in EnemiesNearby)
            {
                if (entity.activeInHierarchy)
                {
                    entities++;
                }
            }

            return entities;
        }

        Music CombatMusic
        {
            get
            {
                return Music.NONE;
            }
        }

        void HandleCombatMusic()
        {
            if (ActiveEnemiesNearby() > 0)
            {
                if (currentMusic != CombatMusic && !fading)
                {
                    originalMusic = currentMusic;
                    originalLevel = musicSource.volume;
                    fadeTo(CombatMusic, 2, 0.2f);
                }
            }
            else if (currentMusic == CombatMusic && !fading)
            {
                fadeTo(originalMusic, 2, originalLevel);
            }
        }

        private bool fading;
        void HandleFade()
        {
            if (fadeIn && musicSource.volume < fadingTargetVolume)
            {
                DecreaseVolumeLevel(AudioSources.MUSIC, -fadingTime * Time.deltaTime);
            }
            else if (fadeIn && musicSource.volume >= fadingTargetVolume)
            {
                SetVolumeLevel(AudioSources.MUSIC, fadingTargetVolume);
                fadeIn = false;
                fading = false;
            }

            if (fadeOut)
            {
                DecreaseVolumeLevel(AudioSources.MUSIC, fadingTime * Time.deltaTime);
            }

            if (musicSource.volume <= 0 && fadeOut)
            {
                fadeOut = false;
                fadeIn = true;
                PlayMusic(fadingTarget, 0);
            }
        }

        public void fadeTo(Music music, float time, float volume)
        {
            if (!fading)
            {
                fadingTarget = music;
                fadeOut = true;
                fadingTime = time;
                fadingTargetVolume = volume;
                fading = true;
            }
        }

        [HideInInspector]
        public bool pauseQueue;

        void HandleQueue()
        {
            if (ActiveEnemiesNearby() > 0)
            {
                return;
            }

            if (pauseQueue)
            {
                return;
            }

            if (fadeQueueBackIn && musicSource.volume < queueTargetLevel)
            {
                DecreaseVolumeLevel(AudioSources.MUSIC, -0.1f * Time.deltaTime);
            }
            else if (fadeQueueBackIn && musicSource.volume >= queueTargetLevel)
            {
                SetVolumeLevel(AudioSources.MUSIC, queueTargetLevel);
                fadeQueueBackIn = false;
            }

            if (musicQueue.Count > 0)
            {
                musicSource.loop = false;
            }
            else
            {
                if (!forceNoLoop)
                {
                    musicSource.loop = true;
                }
                return;
            }

            if (musicSource.time >= musicSource.clip.length - 5 && musicSource.time < musicSource.clip.length)
            {
                DecreaseVolumeLevel(AudioSources.MUSIC, 0.1f * Time.deltaTime);
            }

            if (musicSource.time >= musicSource.clip.length || musicSource.time == 0)
            {
                if (musicQueue.Count > 0)
                {
                    PlayMusic(musicQueue[0].music, 0);
                    fadeQueueBackIn = true;
                    queueTargetLevel = musicQueue[0].level;
                    musicQueue.RemoveAt(0);
                }
            }
        }

        public void SetVolumeLevel(AudioSources source, float level)
        {
            if (source == AudioSources.FX)
            {
                if (level > soundFXVolumeMax)
                {
                    level = soundFXVolumeMax;
                }
                fxSource.volume = level;
            }

            if (source == AudioSources.MUSIC)
            {
                if (level > musicVolumeMax)
                {
                    level = musicVolumeMax;
                }
                musicSource.volume = level;
            }

            if (source == AudioSources.AMBIENT)
            {
                if (level > ambientVolumeMax)
                {
                    level = ambientVolumeMax;
                }
                ambientSource.volume = level;
            }
        }

        public void setMaxVolumeLevel(AudioSources source, float level)
        {
            if (source == AudioSources.FX)
            {
                soundFXVolumeMax = level;
            }
            if (source == AudioSources.MUSIC)
            {
                musicVolumeMax = level;
            }
            if (source == AudioSources.AMBIENT)
            {
                ambientVolumeMax = level;
            }
        }

        public void DecreaseVolumeLevel(AudioSources source, float level)
        {
            if (source == AudioSources.FX)
            {
                if (fxSource.volume > soundFXVolumeMax)
                {
                    fxSource.volume = soundFXVolumeMax;
                }
                else
                {
                    fxSource.volume -= level;
                }
            }
            if (source == AudioSources.MUSIC)
            {
                if (musicSource.volume > musicVolumeMax)
                {
                    musicSource.volume = musicVolumeMax;
                }
                else
                {
                    musicSource.volume -= level;
                }
            }
            if (source == AudioSources.AMBIENT)
            {
                if (ambientSource.volume > ambientVolumeMax)
                {
                    ambientSource.volume = ambientVolumeMax;
                }
                else
                {
                    ambientSource.volume -= level;
                }
            }
        }

        public float currentVolume(AudioSources source)
        {
            if (source == AudioSources.FX)
            {
                return fxSource.volume;
            }
            if (source == AudioSources.MUSIC)
            {
                return musicSource.volume;
            }
            if (source == AudioSources.AMBIENT)
            {
                return ambientSource.volume;
            }

            return 0;
        }

        public ResourceRequest wwwToAudio(string filename)
        {
            ResourceRequest audioRequest = Resources.LoadAsync<AudioClip>("Music/" + filename);
            return audioRequest;
        }

        public ResourceRequest loadAsyncMusicFile(Music file)
        {
            return null;
        }

        public AudioClip ambientFile(Ambient file)
        {
            return null;
        }

        public AudioClip soundFile(Sound file)
        {
            return null;
        }

        public void PlaySound(Sound sound, float level = 1)
        {
            return;

            if (level > soundFXVolumeMax)
            {
                level = soundFXVolumeMax;
            }

            fxSource.PlayOneShot(soundFile(sound), level);
        }

        public void SetAmbient(Ambient ambient, float level = 1.0f)
        {
            return;

            ambientSource.clip = ambientFile(ambient);
            SetVolumeLevel(AudioSources.AMBIENT, level);
            ambientSource.Play();
        }

        public float GetHumanReadableVolume(AudioSources source)
        {
            float mixVolume;

            if (source == AudioSources.MUSIC)
            {
                MainMixer.GetFloat("Music", out mixVolume);
            }
            else if (source == AudioSources.AMBIENT)
            {
                MainMixer.GetFloat("Ambient", out mixVolume);
            }
            else if (source == AudioSources.FX)
            {
                MainMixer.GetFloat("FX", out mixVolume);
            }
            else if (source == AudioSources.MAIN)
            {
                MainMixer.GetFloat("Main", out mixVolume);
            }
            else
            {
                mixVolume = -50;
            }

            if (mixVolume > 0)
            {
                return Mathf.Round(100 + mixVolume * 2);
            }
            else
            {
                return Mathf.Round(100 + mixVolume * 2);
            }
        }

        public float VolumeToHumanReadable(float mixVolume)
        {
            if (mixVolume > 0)
            {
                return Mathf.Round(100 + mixVolume * 2);
            }
            else
            {
                return Mathf.Round(100 + mixVolume * 2);
            }
        }

        public void ChangeMixerVolumeLevel(AudioSources source, float amount)
        {
            float mixVolume = getMixerVolumeLevel(source);

            mixVolume += amount;
            mixVolume = Mathf.Clamp(mixVolume, -50, 15);

            if (source == AudioSources.MUSIC)
            {
                MainMixer.SetFloat("Music", mixVolume);
            }
            else if (source == AudioSources.AMBIENT)
            {
                MainMixer.SetFloat("Ambient", mixVolume);
            }
            else if (source == AudioSources.FX)
            {
                MainMixer.SetFloat("FX", mixVolume);
            }
            else if (source == AudioSources.MAIN)
            {
                MainMixer.SetFloat("Main", mixVolume);
            }
        }

        public float getMixerVolumeLevel(AudioSources source)
        {
            float mixVolume;

            if (source == AudioSources.MUSIC)
            {
                MainMixer.GetFloat("Music", out mixVolume);
            }
            else if (source == AudioSources.AMBIENT)
            {
                MainMixer.GetFloat("Ambient", out mixVolume);
            }
            else if (source == AudioSources.FX)
            {
                MainMixer.GetFloat("FX", out mixVolume);
            }
            else if (source == AudioSources.MAIN)
            {
                MainMixer.GetFloat("Main", out mixVolume);
            }
            else
            {
                mixVolume = -50;
            }

            return mixVolume;
        }

        public void PlayMusic(Music music, float level)
        {
            return;

            if (musicSource.isPlaying)
            {
                musicSource.Stop();
            }

            currentMusic = music;

            currentAsyncMusic = null;

            SetVolumeLevel(AudioSources.MUSIC, level);
            StartCoroutine("PlayAsyncMusicFile");
        }

        public ResourceRequest currentAsyncMusic;
        IEnumerator PlayAsyncMusicFile()
        {
            yield return new WaitForSeconds(0.2f);

            if (currentAsyncMusic == null)
            {
                currentAsyncMusic = loadAsyncMusicFile(currentMusic);
                currentAsyncMusic.allowSceneActivation = true;
                StartCoroutine("PlayAsyncMusicFile");
            }
            else if (!currentAsyncMusic.isDone)
            {
                StartCoroutine("PlayAsyncMusicFile");
            }
            else
            {
                musicSource.clip = (AudioClip)currentAsyncMusic.asset;
                musicSource.Play();
                currentAsyncMusic = null;
            }
        }

        private List<MusicQueue> musicQueue = new List<MusicQueue>();
        public void queueMusic(Music music, float level)
        {
            musicQueue.Add(new MusicQueue(music, level));
        }

        public void reset()
        {
            musicQueue.Clear();
            EnemiesNearby.Clear();
            ambientSource.clip = null;
            musicSource.clip = null;
            fxSource.clip = null;
            currentMusic = Music.NONE;
        }
    }
}
