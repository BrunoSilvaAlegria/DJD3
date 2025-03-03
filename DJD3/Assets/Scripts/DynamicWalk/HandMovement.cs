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
    [SerializeField] float _groundDistance = 1f;
    [SerializeField] float _groundSnapSpeed = 10f;
    [SerializeField] float _maxSlopeAngle = 60f;
    Vector2 _currentInput = Vector2.zero;
    private float _inputThreshold = 0.1f;
    private Vector3 _velocity = Vector3.zero;

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
        if (_currentInput.magnitude > _inputThreshold)
        {
            RaycastHit hit = GetBestSurface();
            if (hit.collider)
            {
                Vector3 moveDirection = transform.forward * _currentInput.y;
                moveDirection = Vector3.ProjectOnPlane(moveDirection, hit.normal);

                transform.position += moveDirection.normalized * _forwardSpeed * Time.deltaTime;

                Quaternion desiredRotation = Quaternion.LookRotation(Vector3.Cross(transform.right, hit.normal), hit.normal);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, desiredRotation, _turnSpeed * Time.deltaTime);

                MaintainGroundDistance(hit);
            }
            else
            {
                ApplyGravity();
            }
        }
    }

    private void FixedUpdate()
    {
        if (_currentInput.magnitude > _inputThreshold)
        {
            _rigidBody.transform.Rotate(0, _currentInput.x * _turnSpeed * Time.fixedDeltaTime, 0);
        }
    }

    private RaycastHit GetBestSurface()
    {
        RaycastHit hit;
        Vector3 castOffset = transform.up * _castHeightAboveBody;

        // Perform the main SphereCast
        if (Physics.SphereCast(transform.position + castOffset, _castSphereRadius, -transform.up, out hit, _castHeightAboveBody + 1f, _terrainLayer))
        {
            if (Vector3.Angle(hit.normal, Vector3.up) < _maxSlopeAngle)
            {
                return hit;
            }
        }

        // If the main SphereCast failed, do additional raycasts around the object
        Vector3[] directions = { transform.forward, -transform.forward, transform.right, -transform.right };
        foreach (var dir in directions)
        {
            if (Physics.Raycast(transform.position + dir * 0.5f, -transform.up, out hit, _castHeightAboveBody + 1f, _terrainLayer))
            {
                if (Vector3.Angle(hit.normal, Vector3.up) < _maxSlopeAngle)
                {
                    return hit;
                }
            }
        }

        Debug.LogWarning("No valid ground detected!");
        return new RaycastHit();
    }

    private void MaintainGroundDistance(RaycastHit hit)
    {
        Vector3 targetPosition = hit.point + hit.normal * _groundDistance;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _velocity, _groundSnapSpeed * Time.deltaTime);
    }

    private void ApplyGravity()
    {
        _rigidBody.AddForce(Physics.gravity, ForceMode.Acceleration);
    }
}
