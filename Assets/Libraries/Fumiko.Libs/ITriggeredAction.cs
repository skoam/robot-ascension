using UnityEngine;

namespace Fumiko.Interaction.Interactable
{
    public interface ITriggeredAction
    {
        TriggeredActionCachedValues CachedValues { get; }
        TriggeredActionSettings Settings { get; }

        string ActionName { get; }

        void Initialize(GameObject parent);
        void Run(GameObject parent);
        void Revert(GameObject parent);
    }
}