namespace Fumiko.Systems.Input
{
    [System.Serializable]
    public class InputMap
    {
        public string description = "Unnamed InputMap";
        public bool active = false;
        public InputCase[] inputs;
        public bool usesJoystick = false;
    }
}