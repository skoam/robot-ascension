using UnityEngine;
using Fumiko.Systems.Input;

namespace UnityTemplateProjects
{
    public class SimpleCameraController : MonoBehaviour
    {
        class CameraState
        {
            public float yaw;
            public float pitch;
            public float roll;
            public float x;
            public float y;
            public float z;

            public void SetFromTransform(Transform t)
            {
                pitch = t.eulerAngles.x;
                yaw = t.eulerAngles.y;
                roll = t.eulerAngles.z;
                x = t.position.x;
                y = t.position.y;
                z = t.position.z;
            }

            public void Translate(Vector3 translation)
            {
                Vector3 rotatedTranslation = Quaternion.Euler(0, yaw, roll) * translation;

                x += rotatedTranslation.x;
                y += rotatedTranslation.y;
                z += rotatedTranslation.z;
            }

            public void LerpTowards(CameraState target, float positionLerpPct, float rotationLerpPct)
            {
                yaw = Mathf.Lerp(yaw, target.yaw, rotationLerpPct);
                pitch = Mathf.Lerp(pitch, target.pitch, rotationLerpPct);
                roll = Mathf.Lerp(roll, target.roll, rotationLerpPct);

                x = Mathf.Lerp(x, target.x, positionLerpPct);
                y = Mathf.Lerp(y, target.y, positionLerpPct);
                z = Mathf.Lerp(z, target.z, positionLerpPct);
            }

            public void UpdateTransform(Transform t)
            {
                t.eulerAngles = new Vector3(pitch, yaw, roll);
                t.position = new Vector3(x, y, z);
            }
        }

        CameraState m_TargetCameraState = new CameraState();
        CameraState m_InterpolatingCameraState = new CameraState();

        [Header("Movement Settings")]
        public float boost = 2;
        public float height = 1.5f;
        public float movementSpeed = 3f;

        [Tooltip("Time it takes to interpolate camera position 99% of the way to the target."), Range(0.001f, 1f)]
        public float positionLerpTime = 0.2f;

        [Header("Rotation Settings")]
        [Tooltip("X = Change in mouse position.\nY = Multiplicative factor for camera rotation.")]
        public AnimationCurve mouseSensitivityCurve = new AnimationCurve(new Keyframe(0f, 0.5f, 0f, 5f), new Keyframe(1f, 2.5f, 0f, 0f));

        [Tooltip("Time it takes to interpolate camera rotation 99% of the way to the target."), Range(0.001f, 1f)]
        public float rotationLerpTime = 0.01f;

        [Tooltip("Whether or not to invert our Y axis for mouse input to rotation.")]
        public bool invertY = true;

        void OnEnable()
        {
            m_TargetCameraState.SetFromTransform(transform);
            m_InterpolatingCameraState.SetFromTransform(transform);
        }

        Vector3 GetInputTranslationDirection()
        {
            Vector3 direction = Vector3.zero;

            if (InputMapSystem.instance.getInput(InputCases.MOVEMENT_UP, InputQueryType.AXIS) > 0)
            {
                direction += Vector3.forward;
            }

            if (InputMapSystem.instance.getInput(InputCases.MOVEMENT_DOWN, InputQueryType.AXIS) > 0)
            {
                direction += Vector3.back;
            }

            if (InputMapSystem.instance.getInput(InputCases.MOVEMENT_LEFT, InputQueryType.AXIS) > 0)
            {
                direction += Vector3.left;
            }

            if (InputMapSystem.instance.getInput(InputCases.MOVEMENT_RIGHT, InputQueryType.AXIS) > 0)
            {
                direction += Vector3.right;
            }

            return direction.normalized;
        }

        void Update()
        {
            // Exit Sample  

            if (IsEscapePressed())
            {
                Application.Quit();
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#endif
            }

            var mouseMovement = GetInputLookRotation() * Time.deltaTime;
            if (invertY)
                mouseMovement.y = -mouseMovement.y;

            mouseMovement = Vector3.ClampMagnitude(mouseMovement, 1.5f);

            var mouseSensitivityFactor = mouseSensitivityCurve.Evaluate(mouseMovement.magnitude);

            m_TargetCameraState.yaw += mouseMovement.x * mouseSensitivityFactor;
            m_TargetCameraState.pitch += mouseMovement.y * mouseSensitivityFactor;
            m_TargetCameraState.pitch = Mathf.Clamp(m_TargetCameraState.pitch, -85, 90);

            // Translation
            var translation = GetInputTranslationDirection() * movementSpeed * Time.deltaTime;

            // Speed up movement when shift key held
            if (IsBoostPressed())
            {
                translation *= boost;
            }

            m_TargetCameraState.Translate(translation);

            // Framerate-independent interpolation
            // Calculate the lerp amount, such that we get 99% of the way to our target in the specified time
            var positionLerpPct = 1f - Mathf.Exp((Mathf.Log(1f - 0.99f) / positionLerpTime) * Time.deltaTime);
            var rotationLerpPct = 1f - Mathf.Exp((Mathf.Log(1f - 0.99f) / rotationLerpTime) * Time.deltaTime);
            m_InterpolatingCameraState.LerpTowards(m_TargetCameraState, positionLerpPct, rotationLerpPct);

            m_InterpolatingCameraState.UpdateTransform(transform);
        }

        Vector2 GetInputLookRotation()
        {
            return new Vector2(
                InputMapSystem.instance.getInput(InputCases.LOOK_X, InputQueryType.AXIS),
                InputMapSystem.instance.getInput(InputCases.LOOK_Y, InputQueryType.AXIS));
        }

        bool IsBoostPressed()
        {
            return InputMapSystem.instance.getInput(InputCases.SPECIAL) > 0;

        }

        bool IsEscapePressed()
        {
            return InputMapSystem.instance.getInput(InputCases.MENU) > 0;
        }
    }

}
