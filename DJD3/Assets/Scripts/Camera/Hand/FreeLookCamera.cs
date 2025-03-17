using UnityEngine;

public class FreeLookCamera : MonoBehaviour
{
    public Transform target; // The player or object the camera follows
    public float distance = 4.0f; // Default camera distance
    public float minDistance = 1.0f; // Minimum distance when avoiding obstacles
    public float maxDistance = 6.0f; // Maximum distance
    public float sensitivity = 2.0f; // Mouse sensitivity
    public float smoothSpeed = 5.0f; // Smoothness of camera movement
    public LayerMask obstacleLayers; // Layers considered as obstacles

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

        // Get mouse input
        yaw += Input.GetAxis("Mouse X") * sensitivity;
        pitch -= Input.GetAxis("Mouse Y") * sensitivity;
        pitch = Mathf.Clamp(pitch, -30f, 80f); // Limit vertical rotation

        // Calculate desired camera position
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        Vector3 desiredPosition = target.position - (rotation * Vector3.forward * distance);

        // Check for obstacles
        if (Physics.Raycast(target.position, (desiredPosition - target.position).normalized, out RaycastHit hit, maxDistance, obstacleLayers))
        {
            distance = Mathf.Clamp(hit.distance * 0.9f, minDistance, maxDistance); // Move closer
        }
        else
        {
            distance = Mathf.Lerp(distance, maxDistance, Time.deltaTime * smoothSpeed); // Return to normal distance
        }

        // Final camera position
        desiredPosition = target.position - (rotation * Vector3.forward * distance);
        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * smoothSpeed);
        transform.LookAt(target.position); // Keep looking at the player
    }
}
