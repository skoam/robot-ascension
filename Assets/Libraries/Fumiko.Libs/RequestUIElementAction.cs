using UnityEngine;

using Fumiko.UI;

namespace Fumiko.Interaction.Interactable.Actions
{
    public class RequestUIElementAction : MonoBehaviour, ITriggeredAction
    {
        public string actionName = "Request UI Element Action";

        public UIElements element;

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
            DynamicUISystem.instance.protectUIElement(element, this.transform.gameObject);
            DynamicUISystem.instance.showUIElement(element);
        }

        public void Revert(GameObject parent)
        {

        }
    }
}