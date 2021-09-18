using UnityEngine;

namespace Fumiko.Interaction.Interactable
{
    [System.Serializable]
    public class InteractableTracking
    {
        public string PlayerTag = "Player";

        [Header("Runtime Values")]
        public Transform hitOrigin;
        public Transform player;
    }
}