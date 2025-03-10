using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "AudioClipDatabase", menuName = "MemeHype/AudioClipDatabase", order = 2)]
    public class AudioClipDatabase : ScriptableObject
    {
        public List<AudioClipData> audioClips;

        public AudioClipData GetAudioClip(string clipName)
        {
            return audioClips.Find(clip => clip.clip.name == clipName);
        }
    }
}