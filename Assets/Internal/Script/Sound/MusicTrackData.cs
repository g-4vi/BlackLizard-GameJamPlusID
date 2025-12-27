using UnityEngine;
using UnityEngine.Audio;

namespace GameJamPlus.AudioModules {
    [CreateAssetMenu(fileName = "New Music Track Data", menuName = "GameJamPlus/New Music Track")]
    public class MusicTrackData : ScriptableObject {
        public AudioClip clip;
        public AudioMixerGroup mixerGroup;

        [Header("Timing")]
        [Tooltip("Time (in seconds) where the loop starts.")]
        public float loopStart;
        [Tooltip("Time (in seconds) where the loop ends.")]
        public float loopEnd;

        [Header("Volume")]
        [Range(0f, 1f)] public float volume = 1f;
    }
}