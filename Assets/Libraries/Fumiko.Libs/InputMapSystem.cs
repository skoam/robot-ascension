using System.Collections.Generic;
using UnityEngine;
using Fumiko.Systems.Debug;

namespace Fumiko.Systems.Input
{
    public class InputMapSystem : MonoBehaviour
    {
        public static InputMapSystem instance;

        public List<InputMap> inputMaps;

        public MouseDisplayState mouseDisplay;

        private Dictionary<InputCases, float> inputValues;

        public bool getButtonDown(InputCases targetInput)
        {
            return getInput(targetInput, InputQueryType.BUTTON_DOWN) == 1;
        }

        public bool getButtonHold(InputCases targetInput)
        {
            return getInput(targetInput, InputQueryType.BUTTON) == 1;
        }

        public bool getButtonUp(InputCases targetInput)
        {
            return getInput(targetInput, InputQueryType.BUTTON_UP) == 1;
        }

        public InputMap getCurrentInputMap()
        {
            for (int i = 0; i < inputMaps.Count; i++)
            {
                if (inputMaps[i].active)
                {
                    return inputMaps[i];
                }
            }

            return null;
        }

        public float getInput(InputCases targetInput, InputQueryType queryType = InputQueryType.BUTTON)
        {
            float inputValue = 0.0f;

            for (int u = 0; u < inputMaps.Count; u++)
            {
                if (inputMaps[u].active)
                {
                    for (int i = 0; i < inputMaps[u].inputs.Length; i++)
                    {
                        InputCase input = inputMaps[u].inputs[i];
                        if (input.inputCase == targetInput)
                        {
                            if (queryType == InputQueryType.BUTTON)
                            {
                                inputValue = UnityEngine.Input.GetButton(input.axisName) ? 1 : inputValue;
                            }

                            if (queryType == InputQueryType.AXIS)
                            {
                                if (UnityEngine.Input.GetAxisRaw(input.axisName) > inputValue)
                                {
                                    inputValue = UnityEngine.Input.GetAxisRaw(input.axisName);
                                }
                            }

                            if (queryType == InputQueryType.AXIS)
                            {
                                if (Mathf.Abs(UnityEngine.Input.GetAxisRaw(input.axisName)) > inputValue)
                                {
                                    inputValue = UnityEngine.Input.GetAxisRaw(input.axisName);

                                    if (input.invertAxis)
                                    {
                                        inputValue *= -1;
                                    }
                                }
                            }

                            if (queryType == InputQueryType.BUTTON_DOWN)
                            {
                                inputValue = UnityEngine.Input.GetButtonDown(input.axisName) ? 1 : inputValue;
                            }

                            if (queryType == InputQueryType.BUTTON_UP)
                            {
                                inputValue = UnityEngine.Input.GetButtonUp(input.axisName) ? 1 : inputValue;
                            }

                            if (Mathf.Abs(inputValue) < input.minInputValue)
                            {
                                inputValue = 0;
                            }
                        }
                    }
                }
            }

            return inputValue;
        }

        public void toggleMouseStateForLinux()
        {
            switch (mouseDisplay)
            {
                case MouseDisplayState.HIDDEN:
                    Cursor.visible = true;
                    Cursor.visible = false;
                    break;

                case MouseDisplayState.VISIBLE:
                    Cursor.visible = false;
                    Cursor.visible = true;
                    break;
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

        private void catchMouse()
        {
            switch (mouseDisplay)
            {
                case MouseDisplayState.HIDDEN:
                    Cursor.lockState = CursorLockMode.Locked;
                    break;

                case MouseDisplayState.VISIBLE:
                    Cursor.lockState = CursorLockMode.None;
                    break;
            }
        }

        private void setMouseState()
        {
            switch (mouseDisplay)
            {
                case MouseDisplayState.HIDDEN:
                    Cursor.visible = false;
                    break;

                case MouseDisplayState.VISIBLE:
                    Cursor.visible = true;
                    break;
            }
        }

        private void Start()
        {
            if (UnityEngine.Input.GetJoystickNames().Length > 0)
            {
                for (int i = 0; i < UnityEngine.Input.GetJoystickNames().Length; i++)
                {
                    UnityEngine.Debug.Log(UnityEngine.Input.GetJoystickNames()[i]);
                }
            }

            inputValues = new Dictionary<InputCases, float>();
            catchMouse();
        }

        private void Update()
        {
            setMouseState();
            catchMouse();
        }
    }
}