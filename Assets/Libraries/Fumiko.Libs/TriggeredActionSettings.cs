namespace Fumiko.Interaction.Interactable
{
    [System.Serializable]
    public class TriggeredActionSettings
    {
        public TriggeredActionRestorationMethod restorationMethod = TriggeredActionRestorationMethod.REVERSE;
        public TriggeredActionRunMethod runMethod = TriggeredActionRunMethod.ONCE;
    }
}