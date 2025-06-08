using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class BasicMovement : MonoBehaviour
{
    public float moveSpeed = 5f;      // Speed at which the object moves
    public float rotateSpeed = 100f;  // Speed at which the object rotates

    private Rigidbody rb;
    [SerializeField]private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Movement Input
        float moveForward = Input.GetKey(KeyCode.W) ? 1f : (Input.GetKey(KeyCode.S) ? -1f : 0f);
        Vector3 moveDirection = transform.forward * moveForward;

        // Apply movement to Rigidbody
        rb.linearVelocity = new Vector3(moveDirection.x * moveSpeed, rb.linearVelocity.y, moveDirection.z * moveSpeed);

        // Rotation Input
        float rotateDirection = 0f;
        if (Input.GetKey(KeyCode.A))
            rotateDirection = -1f;
        else if (Input.GetKey(KeyCode.D))
            rotateDirection = 1f;

        transform.Rotate(0f, rotateDirection * rotateSpeed * Time.deltaTime, 0f);

        // Update Animator Parameters
        bool isGrounded = Mathf.Abs(rb.linearVelocity.y) < 0.01f;
        bool isMoving = moveForward != 0f;

        animator.SetBool("isWalking", isGrounded && isMoving);
        animator.SetBool("isIdle", isGrounded && !isMoving);
        animator.SetBool("isFalling", !isGrounded);
    }
}
