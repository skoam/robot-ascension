using UnityEngine;

using Fumiko.Common;

namespace Fumiko.Interaction.Interactable.Actions
{
    public class ActivateGameObjectAction : MonoBehaviour, ITriggeredAction
    {
        public string actionName = "Activate Game Object Action";

        public TriggeredActionSettings settings;
        TriggeredActionCachedValues cachedValues = new TriggeredActionCachedValues();

        public GameObjectStatePair[] targets;

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
            foreach (GameObjectStatePair target in targets)
            {
                if (target.state == EnabledState.ENABLED)
                {
                    target.gameObject.SetActive(true);
                }

                if (target.state == EnabledState.DISABLED)
                {
                    target.gameObject.SetActive(false);
                }
            }
        }

        public void Revert(GameObject parent)
        {
            foreach (GameObjectStatePair target in targets)
            {
                if (target.state == EnabledState.ENABLED)
                {
                    target.gameObject.SetActive(false);
                }

                if (target.state == EnabledState.DISABLED)
                {
                    target.gameObject.SetActive(true);
                }
            }
        }
    }
}