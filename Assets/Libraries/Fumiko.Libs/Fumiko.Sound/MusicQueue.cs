namespace Fumiko.Sound
{
    class MusicQueue
    {
        public Music music;
        public float level;

        public MusicQueue(Music musicFile, float levelValue)
        {
            music = musicFile;
            level = levelValue;
        }
    }
}