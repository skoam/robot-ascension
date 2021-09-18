using UnityEngine;

namespace Fumiko.Interaction.Interactable.Actions
{
    public class StandardAction : MonoBehaviour, ITriggeredAction
    {
        public string actionName = "Standard Triggered Action";

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

        }

        public void Revert(GameObject parent)
        {

        }
    }
}