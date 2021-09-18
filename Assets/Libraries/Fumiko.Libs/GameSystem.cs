using System;
using UnityEngine;
using Fumiko.Systems.Debug;

using System.Collections.Generic;

namespace Fumiko.Systems.GameSystem
{
    public class GameSystem : MonoBehaviour
    {
        public static GameSystem instance;

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

        public delegate void CustomActionFunction();

        public delegate void CustomParameterizedActionFunction(
            ParameterizedArgs args
        );

        public delegate ParameterizedArgs CustomQueryFunction();

        public class CustomParameterizedAction
        {
            public string name = "";
            public CustomParameterizedActionFunction run;

            public CustomParameterizedAction(string _name, Delegate _CustomActionFunction)
            {
                name = _name;
                run = (CustomParameterizedActionFunction)_CustomActionFunction;
            }
        }

        public class CustomAction
        {
            public string name = "";
            public CustomActionFunction run;

            public CustomAction(string _name, Delegate _CustomActionFunction)
            {
                name = _name;
                run = (CustomActionFunction)_CustomActionFunction;
            }
        }

        public class CustomQuery
        {
            public string name = "";
            public CustomQueryFunction run;

            public CustomQuery(string _name, Delegate _CustomQueryFunction)
            {
                name = _name;
                run = (CustomQueryFunction)_CustomQueryFunction;
            }
        }

        public Dictionary<string, CustomAction> customActions = new Dictionary<string, CustomAction>();
        public Dictionary<string, CustomParameterizedAction> customParameterizedActions = new Dictionary<string, CustomParameterizedAction>();

        public Dictionary<string, CustomQuery> customQueries = new Dictionary<string, CustomQuery>();

        public Dictionary<string, int> IntValues = new Dictionary<string, int>();
        public Dictionary<string, string> StringValues = new Dictionary<string, string>();
        public Dictionary<string, GameObject> GameObjects = new Dictionary<string, GameObject>();

        public void AddCustomParameterizedAction(CustomParameterizedAction action)
        {
            customParameterizedActions.Add(action.name, action);
        }

        public void AddCustomAction(CustomAction action)
        {
            customActions.Add(action.name, action);
        }

        public void AddCustomQuery(CustomQuery query)
        {
            customQueries.Add(query.name, query);
        }

        public bool HasCustomQuery(string queryName)
        {
            return customQueries.ContainsKey(queryName);
        }

        public ParameterizedArgs CallCustomQuery(string queryName)
        {
            if (customQueries.ContainsKey(queryName))
            {
                LogSystem.instance.Log(
                    content: $"Querying: {queryName}({customQueries.Count})",
                    debugType: DebugType.GAMESYSTEM_QUERIES
                );

                return customQueries[queryName].run();
            }

            return null;
        }

        public void CallCustomAction(string actionName)
        {
            if (customActions.ContainsKey(actionName))
            {
                LogSystem.instance.Log(
                    content: $"Calling: {actionName}({customActions.Count})",
                    debugType: DebugType.GAMESYSTEM_ACTIONS
                );

                customActions[actionName].run();
            }
        }

        public void CallCustomParameterizedAction(
            string actionName,
            ParameterizedArgs args
        )
        {
            if (customParameterizedActions.ContainsKey(actionName))
            {
                customParameterizedActions[actionName].run(args);
            }
        }

        public void setIntvalue(string valueName, int value)
        {
            if (IntValues.ContainsKey(valueName))
            {
                IntValues[valueName] = value;
            }
            else
            {
                IntValues.Add(valueName, value);
            }
        }

        public void setStringValue(string valueName, string value)
        {
            if (StringValues.ContainsKey(valueName))
            {
                StringValues[valueName] = value;
            }
            else
            {
                StringValues.Add(valueName, value);
            }
        }

        public int? getIntValue(string valueName)
        {
            if (IntValues.ContainsKey(valueName))
            {
                return IntValues[valueName];
            }

            return null;
        }

        public string getStringValue(string valueName)
        {
            if (StringValues.ContainsKey(valueName))
            {
                return StringValues[valueName];
            }

            return null;
        }

        public GameObject getGameObject(string valueName)
        {
            if (GameObjects.ContainsKey(valueName))
            {
                return GameObjects[valueName];
            }

            return null;
        }
    }
}