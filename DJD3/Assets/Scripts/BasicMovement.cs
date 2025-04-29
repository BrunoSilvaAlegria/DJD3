using UnityEngine;

public class BasicMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Speed at which the object moves
    public float rotateSpeed = 100f; // Speed at which the object rotates

    private Rigidbody rb;

    void Start()
    {
        // Get the Rigidbody component attached to the object
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Movement
        float moveForward = Input.GetKey(KeyCode.W) ? 1f : (Input.GetKey(KeyCode.S) ? -1f : 0f);
        Vector3 moveDirection = transform.forward * moveForward;

        // Apply movement to the object
        rb.linearVelocity = new Vector3(moveDirection.x * moveSpeed, rb.linearVelocity.y, moveDirection.z * moveSpeed);

        // Rotation
        float rotateDirection = 0f;
        if (Input.GetKey(KeyCode.A))
            rotateDirection = -1f;
        else if (Input.GetKey(KeyCode.D))
            rotateDirection = 1f;

        // Apply rotation to the object
        transform.Rotate(0f, rotateDirection * rotateSpeed * Time.deltaTime, 0f);
    }
}
