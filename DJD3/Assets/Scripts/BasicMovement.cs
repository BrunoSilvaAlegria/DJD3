using UnityEngine;


public class BasicMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotateSpeed = 100f;

    private Rigidbody rb;
    public Animator animator2;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator2 = GetComponent<Animator>();

        if (animator2 == null)
        {
            Debug.LogWarning("Animator component missing on this GameObject.");
        }
    }

    private void FixedUpdate()
    {
        Vector3 velocity = rb.velocity;
        float rotateDirection = 0f;

        // Movement input
        if (Input.GetKey(KeyCode.W))
        {
            velocity = transform.forward * moveSpeed;
            animator2.SetBool("isWalking", true);
            animator2.SetBool("isWalking", false);
            

        }
        else if (Input.GetKey(KeyCode.S))
        {
            velocity = -transform.forward * moveSpeed;
            animator2.SetBool("isWalking", true);
            animator2.SetBool("isWalking", false);
        }
        else
        {
            // No forward/backward input
            velocity = new Vector3(0f, velocity.y, 0f);
            animator2.SetBool("isIdle", true);
            animator2.SetBool("isIdle", false);
            
                
        }

        // Rotation input
        if (Input.GetKey(KeyCode.A))
        {
            rotateDirection = -1f;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            rotateDirection = 1f;
        }

        // Apply velocity preserving Y velocity (gravity)
        rb.velocity = new Vector3(velocity.x, rb.velocity.y, velocity.z);

        // Apply rotation
        transform.Rotate(0f, rotateDirection * rotateSpeed * Time.fixedDeltaTime, 0f);
    }
}
