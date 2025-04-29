using UnityEngine;

public class BasicMovement : MonoBehaviour
{
    public Transform targetObject; // The object you want to move
    public float moveSpeed = 5f;
    public float rotationSpeed = 100f;

    void Update()
    {
        if (targetObject == null)
            return;

        // Move forward/backward
        float moveDirection = 0f;
        if (Input.GetKey(KeyCode.W))
            moveDirection = 1f;
        else if (Input.GetKey(KeyCode.S))
            moveDirection = -1f;

        targetObject.Translate(Vector3.forward * moveDirection * moveSpeed * Time.deltaTime);

        // Rotate left/right
        float rotationDirection = 0f;
        if (Input.GetKey(KeyCode.A))
            rotationDirection = -1f;
        else if (Input.GetKey(KeyCode.D))
            rotationDirection = 1f;

        targetObject.Rotate(Vector3.up * rotationDirection * rotationSpeed * Time.deltaTime);
    }
}
