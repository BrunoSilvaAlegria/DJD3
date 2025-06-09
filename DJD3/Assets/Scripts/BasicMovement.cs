using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class BasicMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotateSpeed = 100f;

    private Rigidbody rb;
    [SerializeField] private Animator animator;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        // Animator assignment is handled externally, so no code here.
    }

    private void FixedUpdate()
    {
        // Input detection
        bool forwardPressed = Input.GetKey(KeyCode.W);
        bool backwardPressed = Input.GetKey(KeyCode.S);
        bool leftPressed = Input.GetKey(KeyCode.A);
        bool rightPressed = Input.GetKey(KeyCode.D);

        // Movement
        float moveForward = forwardPressed ? 1f : (backwardPressed ? -1f : 0f);
        Vector3 moveDirection = transform.forward * moveForward;
        Vector3 velocity = new Vector3(moveDirection.x * moveSpeed, rb.velocity.y, moveDirection.z * moveSpeed);
        rb.velocity = velocity;

        // Rotation
        float rotateDirection = 0f;
        if (leftPressed) rotateDirection = -1f;
        else if (rightPressed) rotateDirection = 1f;

        // Rotate the transform directly
        transform.Rotate(0f, rotateDirection * rotateSpeed * Time.fixedDeltaTime, 0f);

        // Animation
        bool isWalking = new Vector3(rb.velocity.x, 0f, rb.velocity.z).magnitude > 0.1f;
        if (animator != null)
        {
            animator.SetBool("isWalking", isWalking);
        }
    }
}
