using UnityEngine;

namespace Fumiko.Sound
{
    public enum AudioLevelType
    {
        MASTER,
        FIXED,
        FX,
        MUSIC,
        AMBIENT
    }

    public enum AudioType
    {
        FX,
        MUSIC,
        AMBIENT
    }

    [System.Serializable]
    public class AudioSettings
    {
        [Range(0, 1)]
        public float level = 1;

        public AudioType type;
    }
}