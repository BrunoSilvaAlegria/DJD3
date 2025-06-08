using UnityEngine;

public class SideScrollCamera : MonoBehaviour
{
    public Transform player;              // Reference to the player
    public float screenEdgeThreshold = 0.2f; // Percentage of screen width near the edges (0.2 = 20%)
    public float cameraSpeed = 5f;        // How fast the camera moves
    public float minX = -10f;             // Left limit
    public float maxX = 50f;              // Right limit

    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void LateUpdate()
    {
        if (player == null || mainCamera == null)
            return;

        Vector3 playerViewportPos = mainCamera.WorldToViewportPoint(player.position);
        float cameraMove = 0f;

        // Move camera left
        if (playerViewportPos.x < screenEdgeThreshold)
        {
            cameraMove = player.position.x - mainCamera.ViewportToWorldPoint(new Vector3(screenEdgeThreshold, 0, playerViewportPos.z)).x;
        }
        // Move camera right
        else if (playerViewportPos.x > 1f - screenEdgeThreshold)
        {
            cameraMove = player.position.x - mainCamera.ViewportToWorldPoint(new Vector3(1f - screenEdgeThreshold, 0, playerViewportPos.z)).x;
        }

        // Update position
        if (cameraMove != 0f)
        {
            Vector3 newPos = transform.position + new Vector3(cameraMove, 0f, 0f);
            newPos.x = Mathf.Clamp(newPos.x, minX, maxX); // Clamp within bounds
            transform.position = Vector3.Lerp(transform.position, newPos, Time.deltaTime * cameraSpeed);
        }
    }
}
