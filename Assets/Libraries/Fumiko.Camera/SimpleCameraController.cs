using UnityEngine;
using Fumiko.Systems.Input;

public class SimpleCameraController : MonoBehaviour
{
    class CameraState
    {
        public float yaw;
        public float pitch;
        public float roll;

        public Vector3 position = Vector3.zero;

        private Transform myTransform;
        private float myHeight;

        private float newX;
        private float newY;
        private float newZ;

        public bool movementDisabled;
        public Transform copyLocation;

        public void SetFromTransform(Transform t, float height)
        {
            pitch = t.eulerAngles.x;
            yaw = t.eulerAngles.y;
            roll = t.eulerAngles.z;
            position.x = t.position.x;
            position.y = t.position.y;
            position.z = t.position.z;
            myTransform = t;
            myHeight = height;
        }

        public void Translate(Vector3 translation)
        {
            Vector3 rotatedTranslation = Quaternion.Euler(0, yaw, roll) * translation;

            position = GetValidPosition(position + rotatedTranslation);
        }

        public Vector3 GetValidPosition(Vector3 newPosition)
        {
            Vector3 validPosition = position;

            RaycastHit[] hits = Physics.RaycastAll(
                newPosition,
                Vector3.down,
                myHeight + myHeight / 2f
            );

            bool notGround = false;
            Vector3 groundPoint = Vector3.zero;

            if (hits.Length == 0)
            {
                notGround = true;
            }

            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].transform.tag != "Ground")
                {
                    notGround = true;
                    i = hits.Length;
                }
                else
                {
                    groundPoint = hits[i].point;
                }
            }

            if (!notGround)
            {
                validPosition.x = newPosition.x;
                validPosition.y = groundPoint.y + myHeight;
                validPosition.z = newPosition.z;
            }

            return validPosition;
        }

        public void LerpTowards(CameraState target, float positionLerpPct, float rotationLerpPct)
        {
            yaw = Mathf.Lerp(yaw, target.yaw, rotationLerpPct);
            pitch = Mathf.Lerp(pitch, target.pitch, rotationLerpPct);
            roll = Mathf.Lerp(roll, target.roll, rotationLerpPct);

            position.x = Mathf.Lerp(position.x, target.position.x, positionLerpPct);
            position.y = Mathf.Lerp(position.y, target.position.y, positionLerpPct);
            position.z = Mathf.Lerp(position.z, target.position.z, positionLerpPct);
        }

        public void UpdateTransform(Transform t)
        {
            t.eulerAngles = new Vector3(pitch, yaw, roll);
            t.position = new Vector3(position.x, position.y, position.z);
        }
    }

    public bool movementDisabled;
    public Transform copyLocation;

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
    public AnimationCurve mouseSensitivityCurveX = new AnimationCurve(
        new Keyframe(0f, 0.1f, 0f, 2f),
        new Keyframe(0.3f, 0.3f, 0f, 0f),
        new Keyframe(0.7f, 0.4f, 0f, 3f),
        new Keyframe(1f, 1f, 3f, 0f)
    );

    public AnimationCurve mouseSensitivityCurveY = new AnimationCurve(
        new Keyframe(0f, 0.1f, 0f, 1.5f),
        new Keyframe(0.3f, 0.2f, 0f, 0f),
        new Keyframe(0.7f, 0.3f, 0f, 1.5f),
        new Keyframe(1f, 0.7f, 1.5f, 0f)
    );

    [Tooltip("Time it takes to interpolate camera rotation 99% of the way to the target."), Range(0.001f, 1f)]
    public float rotationLerpTime = 0.1f;

    public float rotationSpeedX = 80f;
    public float rotationSpeedY = 70f;

    private float maxMouseMagnitudeX = 0.1f;
    private float maxMouseMagnitudeY = 0.1f;

    [Tooltip("Whether or not to invert our Y axis for mouse input to rotation.")]
    public bool invertY = true;

    void OnEnable()
    {
        m_TargetCameraState.SetFromTransform(transform, height);
        m_InterpolatingCameraState.SetFromTransform(transform, height);
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

        if (direction != Vector3.zero)
        {
            direction = direction.normalized;
        }

        return direction;
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

        Vector3 mouseMovement = GetInputLookRotation();
        if (invertY)
            mouseMovement.y = -mouseMovement.y;

        mouseMovement = Vector3.ClampMagnitude(mouseMovement, 100f);

        if (Mathf.Abs(mouseMovement.x) > maxMouseMagnitudeX)
        {
            maxMouseMagnitudeX = Mathf.Abs(mouseMovement.x);
        }

        if (Mathf.Abs(mouseMovement.y) > maxMouseMagnitudeY)
        {
            maxMouseMagnitudeY = Mathf.Abs(mouseMovement.y);
        }

        mouseMovement.x /= maxMouseMagnitudeX;
        mouseMovement.y /= maxMouseMagnitudeY;

        float mouseSensitivityFactorX = mouseSensitivityCurveX.Evaluate(Mathf.Abs(mouseMovement.x));
        float mouseSensitivityFactorY = mouseSensitivityCurveY.Evaluate(Mathf.Abs(mouseMovement.y));

        m_TargetCameraState.yaw += mouseMovement.x * mouseSensitivityFactorX * rotationSpeedX;
        m_TargetCameraState.pitch += mouseMovement.y * mouseSensitivityFactorY * rotationSpeedY;
        m_TargetCameraState.pitch = Mathf.Clamp(m_TargetCameraState.pitch, -85, 90);

        m_TargetCameraState.movementDisabled = movementDisabled;
        m_TargetCameraState.copyLocation = copyLocation;

        if (!movementDisabled)
        {
            // Translation
            Vector3 translation = GetInputTranslationDirection() * movementSpeed * Time.deltaTime;

            // Speed up movement when shift key held
            if (IsBoostPressed())
            {
                translation *= boost;
            }

            m_TargetCameraState.Translate(translation);
        }
        else if (copyLocation)
        {
            m_TargetCameraState.position = copyLocation.position;
        }

        // Framerate-independent interpolation
        // Calculate the lerp amount, such that we get 99% of the way to our target in the specified time
        float positionLerpPct = 1f - Mathf.Exp((Mathf.Log(1f - 0.99f) / positionLerpTime) * Time.deltaTime);
        float rotationLerpPct = 1f - Mathf.Exp((Mathf.Log(1f - 0.99f) / rotationLerpTime) * Time.deltaTime);
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
