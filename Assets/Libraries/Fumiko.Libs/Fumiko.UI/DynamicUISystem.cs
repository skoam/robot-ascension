using System.Collections.Generic;
using UnityEngine;
using Fumiko.Systems.Execution;
using Fumiko.Systems.Debug;
using Fumiko.Systems.GameSystem;

namespace Fumiko.UI
{
    public class DynamicUISystem : MonoBehaviour
    {
        public static DynamicUISystem instance;

        public DynamicUIStrings dynamicUIStrings = new DynamicUIStrings();

        private List<UIElement> availableElements = new List<UIElement>();

        private Dictionary<GameObject, UIElements> protectedElements = new Dictionary<GameObject, UIElements>();

        public void addUIElement(UIElement item)
        {
            availableElements.Add(item);
        }

        public void changeUIText(UIElements item, string text)
        {
            UIElement element = getUIElement(item);
            if (element != null && element.mainTextRenderer != null)
            {
                element.mainTextRenderer.text = text;
            }
        }

        public UIElement getUIElement(UIElements item)
        {
            for (int i = 0; i < availableElements.Count; i++)
            {
                if (availableElements[i].identifiedBy == item)
                {
                    return availableElements[i];
                }
            }

            return null;
        }

        public void hideUIElement(UIElements item)
        {
            UIElement element = getUIElement(item);
            if (element != null)
            {
                if (!protectedElements.ContainsValue(element.identifiedBy))
                {
                    element.active = false;
                }
            }
        }

        public bool isUIElementActive(UIElements item)
        {
            UIElement element = getUIElement(item);
            if (element != null)
            {
                return element.active;
            }
            return false;
        }

        public void protectUIElement(UIElements item, GameObject owner)
        {
            if (!protectedElements.ContainsKey(owner))
            {
                protectedElements.Add(owner, item);
            }
        }

        public void removeUIElement(UIElement item)
        {
            availableElements.Remove(item);
        }

        public void showUIElement(UIElements item)
        {
            UIElement element = getUIElement(item);
            if (element != null)
            {
                element.active = true;
            }
        }

        public void unprotectUIElement(UIElements item, GameObject owner)
        {
            if (protectedElements.ContainsKey(owner))
            {
                protectedElements.Remove(owner);
            }
        }

        private void Awake()
        {
            if (instance != null)
            {
                LogSystem.instance.DuplicateSingletonError();
            }
            else
            {
                instance = this;
            }
        }

        // Update is called once per frame
        private void Update()
        {
            if (ExecutionQuerySystem.instance.ExecutionAllowed(ExecutionType.MOVEMENT))
            {
                showUIElement(UIElements.LIFE_BAR);
            }
            else
            {
                hideUIElement(UIElements.LIFE_BAR);
            }

            if (ExecutionQuerySystem.instance.ExecutionAllowed(ExecutionType.CHARACTERS))
            {
                if (GameSystem.instance && GameSystem.instance.CallCustomQuery(CommonQueryNames.IsPlayerAlive) != null &&
                    !GameSystem.instance.CallCustomQuery(CommonQueryNames.IsPlayerAlive).IsTrue)
                {
                    showUIElement(UIElements.YOU_DIED);
                }
                else
                {
                    hideUIElement(UIElements.YOU_DIED);
                }
            }
        }

        public class DynamicUIStrings
        {
            public string possibleAction = "";
        }
    }
}