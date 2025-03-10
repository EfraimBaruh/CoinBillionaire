using UnityEngine;

namespace ScriptableObjects
{
    public enum AudioClipType
    {
        BackgroundMusic,
        SoundEffect,
        UISound
    }

    [CreateAssetMenu(fileName = "AudioClipData", menuName = "MemeHype/AudioClipData", order = 1)]
    public class AudioClipData : ScriptableObject
    {
        [Header("Audio Clip Settings")]
        public AudioClip clip;
        public AudioClipType clipType;
        [Range(0f, 1f)] public float volume = 1f;
        public bool loop = false;
        public int priority = 128;
    }
}