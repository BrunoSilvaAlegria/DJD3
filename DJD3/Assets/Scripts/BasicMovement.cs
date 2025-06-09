using UnityEngine;

public class BasicMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float rotateSpeed = 100f;

    private Rigidbody rb;
    public Animator animator2;

    private string currentAnim = "";
    private int animLayer = 0;

    [Header("Footstep Settings")]
    public AudioClip footstepClip;
    public float footstepInterval = 0.5f;
    public float pitchMin = 0.9f;
    public float pitchMax = 1.1f;

    private AudioSource footstepAudioSource;
    private float footstepTimer = 0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (animator2 == null)
        {
            Debug.LogWarning("Animator component missing on this GameObject.");
        }

        // Setup audio source for footsteps
        footstepAudioSource = GetComponent<AudioSource>();
        if (footstepAudioSource == null)
        {
            footstepAudioSource = gameObject.AddComponent<AudioSource>();
        }
        footstepAudioSource.playOnAwake = false;
        footstepAudioSource.spatialBlend = 1f;
        footstepAudioSource.clip = footstepClip;
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

        // Apply movement
        if (walkingInput)
        {
            velocity = (Input.GetKey(KeyCode.W) ? 1f : -1f) * transform.forward * moveSpeed;
        }
        else
        {
            velocity = new Vector3(0f, velocity.y, 0f);
        }

        // Apply rotation
        if (Input.GetKey(KeyCode.A))
            rotateDirection = -1f;
        else if (Input.GetKey(KeyCode.D))
            rotateDirection = 1f;

        rb.linearVelocity = new Vector3(velocity.x, rb.linearVelocity.y, velocity.z);
        transform.Rotate(0f, rotateDirection * rotateSpeed * Time.fixedDeltaTime, 0f);

        HandleFootsteps(walkingInput);
    }

    private void HandleFootsteps(bool isWalking)
    {
        if (isWalking && footstepClip != null)
        {
            footstepTimer += Time.fixedDeltaTime;
            if (footstepTimer >= footstepInterval)
            {
                footstepTimer = 0f;
                PlayFootstepSound();
            }
        }
        else
        {
            footstepTimer = footstepInterval;
        }
    }

    private void PlayFootstepSound()
    {
        footstepAudioSource.pitch = Random.Range(pitchMin, pitchMax);
        footstepAudioSource.PlayOneShot(footstepClip);
    }
}
