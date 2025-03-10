using ScriptableObjects;
using UnityEngine;
using UnityEngine.Audio;

namespace Managers
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }

        [Header("Audio Sources")]
        [SerializeField] private AudioSource backgroundMusicSource;
        [SerializeField] private AudioSource soundEffectSource;

        [Header("Audio Mixers")]
        [SerializeField] private AudioMixer audioMixer;
        [SerializeField] private string musicVolumeParameter = "MusicVolume";
        [SerializeField] private string sfxVolumeParameter = "SFXVolume";

        [Header("Audio Clip Database")]
        [SerializeField] private AudioClipDatabase audioClipDatabase;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void PlayAudioClip(string clipName)
        {
            var clipData = audioClipDatabase.GetAudioClip(clipName);
            if (clipData == null) return;

            switch (clipData.clipType)
            {
                case AudioClipType.BackgroundMusic:
                    PlayBackgroundMusic(clipData);
                    break;
                case AudioClipType.SoundEffect:
                case AudioClipType.UISound:
                    PlaySoundEffect(clipData);
                    break;
            }
        }

        private void PlayBackgroundMusic(AudioClipData clipData)
        {
            backgroundMusicSource.clip = clipData.clip;
            backgroundMusicSource.volume = clipData.volume;
            backgroundMusicSource.loop = clipData.loop;
            backgroundMusicSource.priority = clipData.priority;
            backgroundMusicSource.Play();
        }

        private void PlaySoundEffect(AudioClipData clipData)
        {
            soundEffectSource.volume = clipData.volume;
            soundEffectSource.priority = clipData.priority;
            soundEffectSource.PlayOneShot(clipData.clip);
        }

        public void StopBackgroundMusic()
        {
            backgroundMusicSource.Stop();
        }

        public void SetMusicVolume(float volume)
        {
            audioMixer.SetFloat(musicVolumeParameter, Mathf.Log10(volume) * 20);
        }

        public void SetSfxVolume(float volume)
        {
            audioMixer.SetFloat(sfxVolumeParameter, Mathf.Log10(volume) * 20);
        }

        public void MuteMusic(bool mute)
        {
            backgroundMusicSource.mute = mute;
        }

        public void MuteSfx(bool mute)
        {
            soundEffectSource.mute = mute;
        }
    }
}