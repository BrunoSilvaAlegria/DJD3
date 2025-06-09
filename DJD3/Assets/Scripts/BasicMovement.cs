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
    }

    void Update()
    {
        // Input detection
        bool forwardPressed = Input.GetKey(KeyCode.W);
        bool backwardPressed = Input.GetKey(KeyCode.S);
        bool isMovingInput = forwardPressed || backwardPressed;

        // Movement
        float moveForward = forwardPressed ? 1f : (backwardPressed ? -1f : 0f);
        Vector3 moveDirection = transform.forward * moveForward;
        rb.linearVelocity = new Vector3(moveDirection.x * moveSpeed, rb.linearVelocity.y, moveDirection.z * moveSpeed);

        // Rotation
        float rotateDirection = 0f;
        if (Input.GetKey(KeyCode.A)) rotateDirection = -1f;
        else if (Input.GetKey(KeyCode.D)) rotateDirection = 1f;

        transform.Rotate(0f, rotateDirection * rotateSpeed * Time.deltaTime, 0f);


        if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) && !isWalking)
        {
            animator.SetBool("isIdle", false);
            animator.SetBool("isWalking", true);
        }
        else if (!(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) && !isIdle)
        {
            animator.SetBool("isWalking", false);
            animator.SetBool("isIdle", true);
        }
    }
}
