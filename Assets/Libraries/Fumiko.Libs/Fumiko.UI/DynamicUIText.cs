using UnityEngine;
using UnityEngine.UI;
using Fumiko.Systems.Input;
using Fumiko.Systems.Execution;
using Fumiko.Localization;

namespace Fumiko.UI
{
    public class DynamicUIText : MonoBehaviour
    {
        public InputCases inputType;

        [Header("Input Hint")]
        public bool isInputHint;

        [Header("Visible Text")]
        public string template = "NONE";

        private float currentInterval = 0;
        private Text target;
        private float updateInterval = 0.05f;

        private void Start()
        {
            target = GetComponent<Text>();
        }

        private void Update()
        {
            if (!ExecutionQuerySystem.instance.ExecutionAllowed(ExecutionType.INPUT))
            {
                return;
            }

            if (target == null)
            {
                return;
            }

            if (currentInterval > updateInterval)
            {
                currentInterval = 0;
            }
            else
            {
                currentInterval += Time.deltaTime;
                return;
            }

            string inputKey = "";
            for (int i = 0; i < InputMapSystem.instance.inputMaps.Count; i++)
            {
                if (InputMapSystem.instance.inputMaps[i].active)
                {
                    for (int u = 0; u < InputMapSystem.instance.inputMaps[i].inputs.Length; u++)
                    {
                        if (InputMapSystem.instance.inputMaps[i].inputs[u].inputCase == inputType)
                        {
                            inputKey = InputMapSystem.instance.inputMaps[i].inputs[u].axisName;
                        }
                    }
                }
            }

            string actionText = TranslationSystem.instance.HasAction(DynamicUISystem.instance.dynamicUIStrings.possibleAction) ?
                TranslationSystem.instance.GetAction(DynamicUISystem.instance.dynamicUIStrings.possibleAction) : DynamicUISystem.instance.dynamicUIStrings.possibleAction;

            target.text = template
                .Replace("{action}", actionText)
                .Replace("{input}", inputKey);
        }
    }
}