using UnityEngine;

public class BasicMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotateSpeed = 100f;

    private Rigidbody rb;
    public Animator animator2;

    private string currentAnim = ""; // Tracks the current animation state name
    private int animLayer = 0;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (animator2 == null)
        {
            Debug.LogWarning("Animator component missing on this GameObject.");
        }
    }

    private void FixedUpdate()
    {
        Vector3 velocity = rb.linearVelocity;
        float rotateDirection = 0f;

        bool walkingInput = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S);
        string targetAnim = walkingInput ? "Walking" : "Idle";

        AnimatorStateInfo stateInfo = animator2.GetCurrentAnimatorStateInfo(animLayer);

        bool sameAnimationPlaying = currentAnim == targetAnim;
        bool animationFinished = stateInfo.normalizedTime >= 1f;

        if (!sameAnimationPlaying || (sameAnimationPlaying && animationFinished))
        {
            // Only update animator parameters if:
            // - We're switching to a different animation OR
            // - We're restarting the same animation but it finished

            if (walkingInput)
            {
                animator2.SetBool("isWalking", true);
                animator2.SetBool("isIdle", false);
            }
            else
            {
                animator2.SetBool("isWalking", false);
                animator2.SetBool("isIdle", true);
            }

            currentAnim = targetAnim;
        }
        // else do nothing (same animation playing and not finished)

        // Apply movement velocity
        if (walkingInput)
        {
            velocity = (Input.GetKey(KeyCode.W) ? 1f : -1f) * transform.forward * moveSpeed;
        }
        else
        {
            velocity = new Vector3(0f, velocity.y, 0f);
        }

        // Rotation input
        if (Input.GetKey(KeyCode.A))
            rotateDirection = -1f;
        else if (Input.GetKey(KeyCode.D))
            rotateDirection = 1f;

        rb.linearVelocity = new Vector3(velocity.x, rb.linearVelocity.y, velocity.z);
        transform.Rotate(0f, rotateDirection * rotateSpeed * Time.fixedDeltaTime, 0f);
    }
}
