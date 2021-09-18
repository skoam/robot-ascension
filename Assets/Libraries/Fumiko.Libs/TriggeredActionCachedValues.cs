using UnityEngine;
using System.Collections.Generic;

namespace Fumiko.Interaction.Interactable
{
    [System.Serializable]
    public class TriggeredActionCachedValues
    {
        public bool triggered;

        public Vector3 parentInteractableStartPosition;

        public TriggeredActionRestorationMethod[] originalRestorationMethod;
        public TriggeredActionRunMethod[] originalRunMethod;

        private Dictionary<string, string> customValues = new Dictionary<string, string>();

        public void AddCustomValue(string key, string value)
        {
            customValues.Add(key, value);
        }

        public string GetCustomValue(string key)
        {
            if (customValues.ContainsKey(key))
            {
                return customValues[key];
            }

            return "";
        }

        public bool HasCustomValue(string key)
        {
            return customValues.ContainsKey(key);
        }
    }
}