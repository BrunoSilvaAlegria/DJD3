using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class HandMovement : MonoBehaviour
{
    Rigidbody _rigidBody;
    [SerializeField] float _forwardSpeed = 3;
    [SerializeField] float _turnSpeed = 1;
    [SerializeField] LayerMask _terrainLayer;
    [SerializeField] float _castHeightAboveBody = 5;
    [SerializeField] float _castSphereRadius = 1;
    [SerializeField] float _forwardCastDistance = 2f;
    [SerializeField] float _groundClearance = 1f;
    Vector2 _currentInput = Vector2.zero;
    [SerializeField] float _inputThreshold = 0.1f; // Added input threshold


    void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
        if (_rigidBody == null)
        {
            Debug.LogError("Rigidbody component missing!");
        }
    }

    void Update()
    {
        OrientBody();
    }

    public void OnMoveInput(CallbackContext context)
    {
        _currentInput = context.ReadValue<Vector2>();
    }

    void OrientBody()
    {
        if (_currentInput.magnitude > _inputThreshold) // Check for input
        {
            RaycastHit hit = GetOrientationUp();
            if (hit.collider)
            {
                Vector3 moveDirection = transform.forward * _currentInput.y;
                if (Vector3.Angle(hit.normal, Vector3.up) > 45f)
                {
                    moveDirection = Vector3.ProjectOnPlane(moveDirection, hit.normal);
                }
                transform.position += moveDirection.normalized * _forwardSpeed * Time.deltaTime;
                Quaternion desiredRotation = Quaternion.LookRotation(Vector3.Cross(transform.right, hit.normal), hit.normal);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, desiredRotation, _turnSpeed * Time.deltaTime);
            }
            else
            {
                ApplyGravity();
            }
        }
    }

    private void FixedUpdate()
    {
        if (_currentInput.magnitude > _inputThreshold) // Check for input
        {
            _rigidBody.transform.Rotate(0, _currentInput.x * _turnSpeed * Time.fixedDeltaTime, 0);
        }
    }

    private RaycastHit GetOrientationUp()
    {
        RaycastHit hit;
        Vector3 castOffset = transform.up * _castHeightAboveBody;
        if (Physics.SphereCast(transform.position + castOffset, _castSphereRadius, -transform.up, out hit, _castHeightAboveBody + 1f, _terrainLayer))
        {
            return hit;
        }
        else
        {
            Debug.LogWarning("No ground detected!");
            return new RaycastHit();
        }
    }

    private void ApplyGravity()
    {
        _rigidBody.AddForce(Physics.gravity, ForceMode.Acceleration);
    }
}
