using UnityEngine;

using Alakajam13th;

namespace Fumiko.Interaction.Interactable.Actions
{
    public class IncreaseProgressAction : MonoBehaviour, ITriggeredAction
    {
        public string actionName = "Increase Progress Action";

        public TriggeredActionSettings settings;
        TriggeredActionCachedValues cachedValues = new TriggeredActionCachedValues();

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
            Game.instance.progress++;
        }

        public void Revert(GameObject parent)
        {

        }
    }
}