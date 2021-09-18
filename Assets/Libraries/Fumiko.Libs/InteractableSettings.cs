using UnityEngine;
using Fumiko.Systems.Input;

namespace Fumiko.Interaction.Interactable
{
    [System.Serializable]
    public class InteractableSettings
    {
        public float visibleAtDistance = 2.0f;

        public InteractableLayer interactableLayer;
        public InteractableTriggerMethods triggerMethod;

        public float interactionAtDistance = 0.0f;

        public float delayBetweenIndividualActions = 0.0f;
        public float delayBetweenRepeats = 0.5f;

        public bool distanceIn2DSpace;

        [Header("Description")]
        public bool showDescription;
        public string interactionText = "Action";
        public string descriptionText = "Description";
        public string[] descriptionTextTuple;

        [Header("Runtime Values")]
        public bool locked;
    }
}