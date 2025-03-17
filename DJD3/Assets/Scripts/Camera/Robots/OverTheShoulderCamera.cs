using UnityEngine;

public class OverTheShoulderCamera : MonoBehaviour
{
    public Transform target; // Player transform
    public float shoulderOffset = 0.5f; // Offset to the right (negative for left shoulder)
    public float heightOffset = 1.5f; // Height adjustment from the player's position
    public float distance = 3.5f; // Default camera distance
    public float minDistance = 1.0f; // Closest it can get when avoiding obstacles
    public float maxDistance = 4.5f; // Max camera distance
    public float sensitivity = 2.0f; // Mouse sensitivity
    public float smoothSpeed = 5.0f; // Camera movement smoothness
    public LayerMask obstacleLayers; // Layers the camera should detect as obstacles
    public LayerMask excludedLayers; // Layers the camera should ignore

    private float yaw = 0.0f;
    private float pitch = 0.0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        if (!target) return;

        // Mouse input for rotation
        yaw += Input.GetAxis("Mouse X") * sensitivity;
        pitch -= Input.GetAxis("Mouse Y") * sensitivity;
        pitch = Mathf.Clamp(pitch, -20f, 50f); // Restrict vertical rotation

        // Calculate rotation
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);

        // Shoulder position with height offset
        Vector3 shoulderPosition = target.position + (Vector3.up * heightOffset) + (target.right * shoulderOffset);
        Vector3 desiredPosition = shoulderPosition - (rotation * Vector3.forward * distance);

        // Check for obstacles while ignoring excluded layers
        int combinedLayerMask = obstacleLayers & ~excludedLayers;
        if (Physics.Raycast(shoulderPosition, (desiredPosition - shoulderPosition).normalized, out RaycastHit hit, maxDistance, combinedLayerMask))
        {
            distance = Mathf.Clamp(hit.distance * 0.9f, minDistance, maxDistance); // Move closer
        }
        else
        {
            distance = Mathf.Lerp(distance, maxDistance, Time.deltaTime * smoothSpeed); // Reset distance smoothly
        }

        // Update final position
        desiredPosition = shoulderPosition - (rotation * Vector3.forward * distance);
        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * smoothSpeed);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * smoothSpeed);
    }
}
