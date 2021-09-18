using System.Collections.Generic;
using UnityEngine;
using Fumiko.Systems.Debug;

namespace Fumiko.Systems.Execution
{
    public class ExecutionQuerySystem : MonoBehaviour
    {
        public static ExecutionQuerySystem instance;

        [Header("Available Sets")]
        public List<ExecutionSet> availableExecutionSets;

        [Header("Current Set")]
        public ExecutionSetNames currentExecutionSet;

        public bool ExecutionAllowed(ExecutionType executionType)
        {
            ExecutionSet set = GetSetByName(currentExecutionSet);

            if (set != null)
            {
                return GetSetByName(currentExecutionSet).isAllowed(executionType);
            }

            return false;
        }

        public ExecutionSet GetSetByName(ExecutionSetNames name)
        {
            for (int i = 0; i < availableExecutionSets.Count; i++)
            {
                if (name == availableExecutionSets[i].name)
                {
                    return availableExecutionSets[i];
                }
            }

            return null;
        }

        public void loadedData()
        {
            CurrentExecutionSet = ExecutionSetNames.ENGINE_INITIALIZATION;
        }

        public ExecutionSetNames CurrentExecutionSet
        {
            get
            {
                return currentExecutionSet;
            }
            set
            {
                currentExecutionSet = value;

                LogSystem.instance.Log(
                    content: "Setting current Execution Set to " + currentExecutionSet,
                    debugType: DebugType.EXECUTION_SYSTEM,
                    errorType: ErrorType.INFO
                );
            }
        }

        private void Awake()
        {
            if (instance != null)
            {
                LogSystem.instance.DuplicateSingletonError();
            }
            else
            {
                instance = this;
            }
        }
    }
}