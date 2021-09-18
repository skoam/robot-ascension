using UnityEngine;
using UnityEditor;

using Fumiko.Systems.Debug;
using Fumiko.Systems.Execution;
using Fumiko.Systems.Input;
using Fumiko.Localization;
using Fumiko.UI;

using Fumiko.Interaction.Interactable.Actions;

namespace Fumiko.Interaction.Interactable
{
    public class Interactable : MonoBehaviour
    {
        public string InteractableName = "Unnamed Interactable";

        public InteractableTracking tracking;

        public InteractableSettings settings;

        private ITriggeredAction[] actions;

        private bool activated;
        private bool forceActivation;

        private float pauseInteractable = 0;

        private Transform triggeredFrom;

        private void Start()
        {
            actions = GetComponents<ITriggeredAction>();

            for (int i = 0; i < actions.Length; i++)
            {
                LogSystem.instance.Log(
                    content: $"Initializing Action {actions[i].ActionName}",
                    debugType: DebugType.INTERACTABLE);

                actions[i].Initialize(this.gameObject);

                actions[i].CachedValues.parentInteractableStartPosition = transform.position;
            }

            if (settings.triggerMethod == InteractableTriggerMethods.START)
            {
                forceActivation = true;
            }

            /*if (useHitBoxCalculations)
            {
                hitboxCalculator.initialize(this.transform.gameObject);
            }*/
        }

        public ITriggeredAction[] Actions
        {
            get
            {
                return actions;
            }
        }

        public void Hit(Transform hitOrigin)
        {
            if (!Paused && settings.triggerMethod == InteractableTriggerMethods.HITBOX)
            {
                tracking.hitOrigin = hitOrigin;
                forceActivation = true;
            }
        }

        public void Restore()
        {
            for (int i = 0; i < actions.Length; i++)
            {
                if (actions[i].Settings.restorationMethod == TriggeredActionRestorationMethod.REVERSE)
                {
                    if (actions[i].CachedValues.triggered)
                    {
                        LogSystem.instance.Log(
                            content: $"Reverting Action {actions[i].ActionName}",
                            debugType: DebugType.INTERACTABLE);

                        actions[i].Revert(this.gameObject);

                        actions[i].CachedValues.triggered = false;
                    }
                }
            }

            forceActivation = false;
        }

        public void Run()
        {
            LogSystem.instance.Log(
                content: $"Run {InteractableName}",
                debugType: DebugType.INTERACTABLE
            );

            for (int i = 0; i < actions.Length; i++)
            {
                if (!actions[i].CachedValues.triggered)
                {
                    LogSystem.instance.Log(
                        content: $"Run Action {actions[i].ActionName}",
                        debugType: DebugType.INTERACTABLE);

                    actions[i].Run(this.gameObject);

                    if (actions[i].Settings.runMethod == TriggeredActionRunMethod.ONCE)
                    {
                        actions[i].CachedValues.triggered = true;
                    }

                    Pause(settings.delayBetweenRepeats);
                }
            }
        }

        private void Update()
        {
            if (!ExecutionQuerySystem.instance.ExecutionAllowed(ExecutionType.INTERACTION))
            {
                return;
            }

            if (!tracking.player)
            {
                tracking.player = GameObject.FindGameObjectWithTag(tracking.PlayerTag).transform;
                return;
            }

            CalculateTimers();

            if (InVisibleDistance)
            {
                if (settings.showDescription && DynamicUISystem.instance)
                {
                    string descriptionText =
                        TranslationSystem.instance && TranslationSystem.instance.HasDescription(settings.descriptionText) ?
                        TranslationSystem.instance.GetDescription(settings.descriptionText) : settings.descriptionText;

                    for (int i = 0; i < settings.descriptionTextTuple.Length; i++)
                    {
                        string descriptionTextTuple =
                            TranslationSystem.instance && TranslationSystem.instance.HasDescription(settings.descriptionTextTuple[i]) ?
                            TranslationSystem.instance.GetDescription(settings.descriptionTextTuple[i]) : settings.descriptionTextTuple[i];

                        descriptionText += " (" + descriptionTextTuple + ")";
                    }

                    DynamicUISystem.instance.protectUIElement(UIElements.ITEM, this.gameObject);
                    DynamicUISystem.instance.showUIElement(UIElements.ITEM);
                    DynamicUISystem.instance.changeUIText(UIElements.ITEM, descriptionText);

                    DynamicUISystem.instance.dynamicUIStrings.possibleAction =
                        TranslationSystem.instance && TranslationSystem.instance.HasAction(settings.interactionText) ?
                        TranslationSystem.instance.GetAction(settings.interactionText) : settings.interactionText;
                }
            }
            else
            {
                if (settings.showDescription && DynamicUISystem.instance)
                {
                    DynamicUISystem.instance.unprotectUIElement(UIElements.ITEM, this.gameObject);
                    DynamicUISystem.instance.hideUIElement(UIElements.ITEM);
                }
            }

            if (CanActivate)
            {
                Run();
            }
        }

        private void CalculateTimers()
        {
            if (Paused)
            {
                pauseInteractable -= Time.deltaTime;
            }
        }

        private void Pause(float seconds)
        {
            pauseInteractable = seconds;
        }

        private bool Paused
        {
            get
            {
                return pauseInteractable > 0;
            }
        }

        public bool CanActivate
        {
            get
            {
                if (Paused)
                {
                    return false;
                }

                if (NothingToDo)
                {
                    return false;
                }

                if (forceActivation)
                {
                    forceActivation = false;
                    return true;
                }

                if (InInteractionDistance)
                {
                    if (settings.triggerMethod == InteractableTriggerMethods.KEY_OR_BUTTON &&
                        InputMapSystem.instance &&
                        InputMapSystem.instance.getButtonDown(InputCases.INTERACT))
                    {
                        return true;
                    }

                    if (settings.triggerMethod == InteractableTriggerMethods.DISTANCE)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        public bool InInteractionDistance
        {
            get
            {
                if (NothingToDo)
                {
                    return false;
                }

                Vector3 playerPosition = tracking.player.transform.position;

                if (settings.distanceIn2DSpace)
                {
                    return Vector2.Distance(
                        playerPosition,
                        transform.position
                    ) < settings.interactionAtDistance;
                }

                return Vector3.Distance(
                    playerPosition,
                    transform.position
                ) < settings.interactionAtDistance;
            }
        }

        public bool InVisibleDistance
        {
            get
            {
                if (NothingToDo)
                {
                    return false;
                }

                Vector3 playerPosition = tracking.player.transform.position;

                if (settings.distanceIn2DSpace)
                {
                    return Vector2.Distance(
                        playerPosition,
                        transform.position
                    ) < settings.visibleAtDistance;
                }

                return Vector3.Distance(
                    playerPosition,
                    transform.position
                ) < settings.visibleAtDistance;
            }
        }

        public bool NothingToDo
        {
            get
            {
                bool activeActions = false;

                for (int i = 0; i < actions.Length; i++)
                {
                    if (!actions[i].CachedValues.triggered)
                    {
                        activeActions = true;
                    }
                }

                return !activeActions;
            }
        }

        public bool WasActivated
        {
            get
            {
                return NothingToDo;
            }
        }

        void OnDrawGizmos()
        {
#if UNITY_EDITOR
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(transform.position, settings.visibleAtDistance / 2);

            if (settings.visibleAtDistance != settings.interactionAtDistance)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(transform.position, settings.interactionAtDistance / 2);
            }


            if (settings.triggerMethod == InteractableTriggerMethods.KEY_OR_BUTTON)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(transform.position, 0.05f);
            }
#endif
        }
    }
}