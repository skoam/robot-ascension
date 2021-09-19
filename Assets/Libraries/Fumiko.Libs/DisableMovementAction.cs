using UnityEngine;

namespace Fumiko.Interaction.Interactable.Actions
{
    public class DisableMovementAction : MonoBehaviour, ITriggeredAction
    {
        public string actionName = "Disable Movement Action";

        public SimpleCameraController cameraController;
        public Transform copyLocation;

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
            cameraController.movementDisabled = true;
            cameraController.copyLocation = copyLocation;
        }

        public void Revert(GameObject parent)
        {

        }
    }
}