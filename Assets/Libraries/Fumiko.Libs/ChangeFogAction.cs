using UnityEngine;

namespace Fumiko.Interaction.Interactable.Actions
{
    public class ChangeFogAction : MonoBehaviour, ITriggeredAction
    {
        public string actionName = "Change Fog Action";

        public float density = 0.01f;
        public Color color = Color.black;

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
            RenderSettings.fogDensity = density;
            RenderSettings.fogColor = color;
        }

        public void Revert(GameObject parent)
        {

        }
    }
}