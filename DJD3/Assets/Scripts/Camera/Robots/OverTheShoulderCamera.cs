using UnityEngine;

public class OverTheShoulderCamera : MonoBehaviour
{
    public Transform target;
    public float shoulderOffset = 0.5f;
    public float heightOffset = 1.5f;
    public float distance = 3.5f;
    public float minDistance = 1.0f;
    public float maxDistance = 4.5f;
    public float sensitivity = 2.0f;
    public float smoothSpeed = 5.0f;
    public LayerMask obstacleLayers;
    public LayerMask excludedLayers;
    public Animator animator;

    [Header("Aiming Settings")]
    public float aimDistance = 2.0f;
    public float aimShoulderOffset = 0.2f;
    public float aimSensitivity = 1.0f;

    [Header("Object Rotation Sync")]
    public Transform objectToRotateWithCamera;

    [Header("Script Toggle While Aiming")]
    public GameObject objectToToggleWhileAiming;

    private float currentDistance;
    private float currentShoulderOffset;
    private float currentSensitivity;

    private float yaw = 0.0f;
    private float pitch = 0.0f;

    private bool isScriptEnabled = false;
    private bool wasAiming = false;
    private float originalXRotation;


    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        currentDistance = distance;
        currentShoulderOffset = shoulderOffset;
        currentSensitivity = sensitivity;

        if (objectToToggleWhileAiming)
            objectToToggleWhileAiming.active = false;

        if (objectToRotateWithCamera)
            originalXRotation = objectToRotateWithCamera.eulerAngles.x;
    }

    void LateUpdate()
    {
        if (!target) return;

        bool isAiming = Input.GetMouseButton(1); // RMB

        // Toggle script
        if (objectToToggleWhileAiming && isAiming != isScriptEnabled)
        {
            objectToToggleWhileAiming.active = isAiming;
            isScriptEnabled = isAiming;
        }

        // Rotate object or reset X rotation
        if (objectToRotateWithCamera)
        {
            if (isAiming)
            {
                Vector3 currentEuler = objectToRotateWithCamera.eulerAngles;
                currentEuler.x = pitch;
                objectToRotateWithCamera.rotation = Quaternion.Euler(currentEuler);
                animator.SetBool("isAiming", true);

            }
            else if (wasAiming) // Just stopped aiming
            {
                Vector3 currentEuler = objectToRotateWithCamera.eulerAngles;
                currentEuler.x = originalXRotation;
                objectToRotateWithCamera.rotation = Quaternion.Euler(currentEuler);
                animator.SetBool("isAiming", false);
            }
        }

        // Update aiming state
        wasAiming = isAiming;

        float targetDistance = isAiming ? aimDistance : maxDistance;
        float targetShoulderOffset = isAiming ? aimShoulderOffset : shoulderOffset;
        float targetSensitivity = isAiming ? aimSensitivity : sensitivity;

        currentShoulderOffset = Mathf.Lerp(currentShoulderOffset, targetShoulderOffset, Time.deltaTime * smoothSpeed);
        currentSensitivity = Mathf.Lerp(currentSensitivity, targetSensitivity, Time.deltaTime * smoothSpeed);

        yaw += Input.GetAxis("Mouse X") * currentSensitivity;
        pitch -= Input.GetAxis("Mouse Y") * currentSensitivity;
        pitch = Mathf.Clamp(pitch, -20f, 50f);

        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        Vector3 shoulderPosition = target.position + (Vector3.up * heightOffset) + (target.right * currentShoulderOffset);
        Vector3 desiredPosition = shoulderPosition - (rotation * Vector3.forward * currentDistance);

        int combinedLayerMask = obstacleLayers & ~excludedLayers;
        if (Physics.Raycast(shoulderPosition, (desiredPosition - shoulderPosition).normalized, out RaycastHit hit, maxDistance, combinedLayerMask))
        {
            currentDistance = Mathf.Clamp(hit.distance * 0.9f, minDistance, maxDistance);
        }
        else
        {
            currentDistance = Mathf.Lerp(currentDistance, targetDistance, Time.deltaTime * smoothSpeed);
        }

        desiredPosition = shoulderPosition - (rotation * Vector3.forward * currentDistance);
        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * smoothSpeed);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * smoothSpeed);
    }
}
