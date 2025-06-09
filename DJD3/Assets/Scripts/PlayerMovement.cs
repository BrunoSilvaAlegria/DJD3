using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private GameObject targetObject;
    [SerializeField] private float _gravityAcceleration;
    [SerializeField] private float _maxFallSpeed;
    [SerializeField] private float _maxForwardSpeed;
    [SerializeField] private float _maxBackwardSpeed;
    [SerializeField] private float _maxStrafeSpeed;
    [SerializeField] private float _jumpSpeed;
    [SerializeField] private Animator animator;
    [SerializeField] private float groundCheckDistance = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Footstep Settings")]
    [SerializeField] private AudioClip footstepClip;
    [SerializeField] private float footstepInterval = 0.5f;
    [SerializeField] private float pitchMin = 0.9f;
    [SerializeField] private float pitchMax = 1.1f;

    private CharacterController _controller;
    private Vector3 _velocityHor;
    private Vector3 _velocityVer;
    private Vector3 _motion;
    private bool _jump;
    private bool _isGrounded;

    private AudioSource _audioSource;
    private float _footstepTimer;

    void Start()
    {
        if (targetObject == null)
        {
            Debug.LogError("Target Object not assigned!");
            enabled = false;
            return;
        }

        _controller = targetObject.GetComponent<CharacterController>();
        if (_controller == null)
        {
            Debug.LogError("Target Object does not have a CharacterController!");
            enabled = false;
            return;
        }

        _audioSource = targetObject.AddComponent<AudioSource>();
        _audioSource.playOnAwake = false;
        _audioSource.spatialBlend = 1f; // 3D sound
        _audioSource.clip = footstepClip;

        _velocityHor = Vector3.zero;
        _velocityVer = Vector3.zero;
        _motion = Vector3.zero;
        _jump = false;

        HideCursor();
    }

    private void HideCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        _isGrounded = IsGrounded();
        Debug.Log("Raycast Grounded = " + _isGrounded);
        UpdateRotation();
        CheckForJump();

        HandleFootsteps();
    }

    private void UpdateRotation()
    {
        float rotation = Input.GetAxis("Mouse X");
        targetObject.transform.Rotate(0f, rotation, 0f);
    }

    private void CheckForJump()
    {
        if (Input.GetButtonDown("Jump") && _isGrounded)
        {
            animator.SetTrigger("jump");
            _jump = true;
        }
    }

    void FixedUpdate()
    {
        UpdateVelocityHor();
        UpdateVelocityVer();
        UpdatePosition();
    }

    private void UpdateVelocityHor()
    {
        float forwardAxis = Input.GetAxis("Forward");
        float strafeAxis = Input.GetAxis("Strafe");

        _velocityHor.x = strafeAxis * _maxStrafeSpeed;

        if (forwardAxis > 0f)
        {
            if (_isGrounded)
            {
                animator.SetBool("isIdle", false);
                animator.SetBool("walk", true);
            }

            _velocityHor.z = forwardAxis * _maxForwardSpeed;
            _velocityHor = Vector3.ClampMagnitude(_velocityHor, _maxForwardSpeed);
        }
        else if (forwardAxis < 0f)
        {
            _velocityHor.z = forwardAxis * _maxBackwardSpeed;
            _velocityHor = Vector3.ClampMagnitude(_velocityHor, _maxBackwardSpeed);

            if (_isGrounded)
            {
                animator.SetBool("isIdle", false);
                animator.SetBool("walk", true);
            }
        }
        else if (_velocityHor.x != 0)
        {
            if (_isGrounded)
            {
                animator.SetBool("isIdle", false);
                animator.SetBool("walk", true);
            }
        }
        else
        {
            _velocityHor.z = 0f;
            animator.SetBool("walk", false);
            animator.SetBool("isIdle", true);
        }
    }

    private void UpdateVelocityVer()
    {
        if (_jump)
        {
            _velocityVer.y = _jumpSpeed;
            _jump = false;
        }
        else if (_isGrounded)
        {
            _velocityVer.y = -0.1f;
        }
        else
        {
            if (_velocityVer.y < -10)
            {
                animator.SetTrigger("fall");
            }

            animator.SetBool("walk", false);
            animator.SetBool("isIdle", false);

            _velocityVer.y += _gravityAcceleration * Time.fixedDeltaTime;
            _velocityVer.y = Mathf.Max(_velocityVer.y, -_maxFallSpeed);
        }
    }

    private void UpdatePosition()
    {
        _motion = (_velocityHor + _velocityVer) * Time.fixedDeltaTime;
        _motion = targetObject.transform.TransformVector(_motion);
        _controller.Move(_motion);
    }

    /// <summary>
    /// Perform a downward raycast to check for ground beneath the character.
    /// </summary>
    private bool IsGrounded()
    {
        Vector3 origin = targetObject.transform.position + Vector3.up * 0.1f;
        return Physics.Raycast(origin, Vector3.down, groundCheckDistance + 0.1f, groundLayer);
    }

    private void HandleFootsteps()
    {
        bool isMoving = _isGrounded && (_velocityHor.x != 0 || _velocityHor.z != 0);

        if (isMoving)
        {
            _footstepTimer += Time.deltaTime;
            if (_footstepTimer >= footstepInterval)
            {
                _footstepTimer = 0f;
                PlayFootstepSound();
            }
        }
        else
        {
            _footstepTimer = footstepInterval; // reset timer so sound plays quickly after starting to walk again
        }
    }

    private void PlayFootstepSound()
    {
        if (footstepClip == null) return;

        _audioSource.pitch = Random.Range(pitchMin, pitchMax);
        _audioSource.PlayOneShot(footstepClip);
    }
}
