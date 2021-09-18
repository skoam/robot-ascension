namespace Fumiko.Systems.Input
{
    [System.Serializable]
    public class InputCase
    {
        public string description;
        public string axisName;
        public InputCases inputCase;
        public bool invertAxis = false;
        public float minInputValue = 0;
    }
}