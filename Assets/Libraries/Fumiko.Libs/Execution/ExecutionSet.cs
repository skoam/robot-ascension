namespace Fumiko.Systems.Execution
{
    [System.Serializable]
    public class ExecutionSet
    {
        public ExecutionSetNames name;
        public ExecutionType[] allowed;

        public bool isAllowed(ExecutionType type)
        {
            for (int i = 0; i < allowed.Length; i++)
            {
                if (allowed[i] == type)
                {
                    return true;
                }
            }

            return false;
        }
    }
}