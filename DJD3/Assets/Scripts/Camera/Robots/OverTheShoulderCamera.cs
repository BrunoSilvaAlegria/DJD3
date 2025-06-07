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

    private Vector3 defaultLocalPosition;
    private Quaternion defaultLocalRotation;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        currentDistance = distance;
        currentShoulderOffset = shoulderOffset;
        currentSensitivity = sensitivity;

        if (objectToToggleWhileAiming)
        {
            objectToToggleWhileAiming.SetActive(false);
            defaultLocalPosition = objectToToggleWhileAiming.transform.localPosition;
            defaultLocalRotation = objectToToggleWhileAiming.transform.localRotation;
        }

        if (objectToRotateWithCamera)
            originalXRotation = objectToRotateWithCamera.localEulerAngles.x;
    }

    void LateUpdate()
    {
        if (!target) return;

        bool isAiming = Input.GetMouseButton(1); // Right Mouse Button

        // Toggle script object visibility
        if (objectToToggleWhileAiming && isAiming != isScriptEnabled)
        {
            objectToToggleWhileAiming.SetActive(isAiming);
            isScriptEnabled = isAiming;
        }

        // Reset spawn object transform when aiming starts
        if (objectToToggleWhileAiming && isAiming && !wasAiming)
        {
            objectToToggleWhileAiming.transform.localPosition = defaultLocalPosition;
            objectToToggleWhileAiming.transform.localRotation = defaultLocalRotation;
        }

        // Handle object rotation with camera
        if (objectToRotateWithCamera)
        {
            if (isAiming)
            {
                objectToRotateWithCamera.rotation = Quaternion.Euler(pitch, yaw, 0f);
                animator?.SetBool("isAiming", true);
            }
            else if (wasAiming)
            {
                Vector3 euler = objectToRotateWithCamera.localEulerAngles;
                euler.x = originalXRotation;
                objectToRotateWithCamera.localEulerAngles = euler;
                animator?.SetBool("isAiming", false);
            }
        }

        // Update aiming state for next frame
        wasAiming = isAiming;

        // Adjust camera distance and offset
        float targetDistance = isAiming ? aimDistance : maxDistance;
        float targetShoulderOffset = isAiming ? aimShoulderOffset : shoulderOffset;
        float targetSensitivity = isAiming ? aimSensitivity : sensitivity;

        currentShoulderOffset = Mathf.Lerp(currentShoulderOffset, targetShoulderOffset, Time.deltaTime * smoothSpeed);
        currentSensitivity = Mathf.Lerp(currentSensitivity, targetSensitivity, Time.deltaTime * smoothSpeed);

        // Mouse input
        yaw += Input.GetAxis("Mouse X") * currentSensitivity;
        pitch -= Input.GetAxis("Mouse Y") * currentSensitivity;
        pitch = Mathf.Clamp(pitch, -20f, 50f);

        // Calculate camera position
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

        // Optional: Draw a debug ray from the projectile spawn point
        if (objectToToggleWhileAiming)
        {
            Debug.DrawRay(objectToToggleWhileAiming.transform.position, objectToToggleWhileAiming.transform.forward * 2f, Color.red);
        }
    }
}
