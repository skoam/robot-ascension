using UnityEngine;

using Fumiko.Sound;

namespace Fumiko.Interaction.Interactable.Actions
{
    public class PlaySoundAction : MonoBehaviour, ITriggeredAction
    {
        public string actionName = "Play Sound Action";

        public TriggeredActionSettings settings;
        TriggeredActionCachedValues cachedValues = new TriggeredActionCachedValues();

        public Sound.Sound sound;
        public float level = 1;

        public string ActionName
        {
            get
            {
                return actionName;
            }
        }

        public TriggeredActionSettings Settings
        {
            get
            {
                return settings;
            }
        }

        public TriggeredActionCachedValues CachedValues
        {
            get
            {
                return cachedValues;
            }
        }

        public void Initialize(GameObject parent)
        {

        }

        public void Run(GameObject parent)
        {
            SoundSystem.instance.PlaySound(
                sound,
                level
            );
        }

        public void Revert(GameObject parent)
        {

        }
    }
}