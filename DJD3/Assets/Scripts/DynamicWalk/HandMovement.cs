using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class HandMovement : MonoBehaviour
{
  Rigidbody _rigidBody;
  [SerializeField] float _forwardSpeed =3;
  [SerializeField] float _turnSpeed =1;
  [SerializeField] float _correctionspeed =1;
  [SerializeField] float _heightCorrectionSpeed;
  [SerializeField] LayerMask _terrainLayer;
  [SerializeField] float _castDepth = 1.5f;
  Vector2 _currentInput = Vector2.zero;
  [SerializeField] float _groundCastDepth = 1;
  [SerializeField] float _castheightAboveBody = 5;
  [SerializeField] float _castPositionsAdvance = 1;
  [SerializeField] Transform _castPositionsTransform;

  [SerializeField] float _desiredGroundClearance = 1;
  Vector3 forwardPoint;
  int castPos =-1;
  Vector3 averageNormal;
  Vector3 averagePos;
  RaycastHit[] hits;
  [SerializeField] float _castSphereRadius = 1;

  void Start()
  {
    _rigidBody = GetComponent<Rigidbody>();
  }

  void Update()
  {
    OrientBody();
  }

  public void OnMoveInput(CallbackContext context)
  {
    _currentInput = context.ReadValue<Vector2>();
    _castPositionsTransform.position = transform.position + (_currentInput.y * _castPositionsAdvance * transform.forward);
  }

  void OrientBody()
  {
    bool grounded;
    RaycastHit hit = GetOrientationUp();

    if (hit.collider)
    {
        forwardPoint = hit.point + (hit.normal * _desiredGroundClearance);
        transform.position = Vector3.MoveTowards(transform.position, forwardPoint, _forwardSpeed);
        Quaternion desiredRotation = Quaternion.LookRotation(Vector3.Cross(transform.right, hit.normal), hit.normal);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, desiredRotation, _turnSpeed * Time.deltaTime);

    }
    else
    {
        ApplyGravity();
    }
  }
  private void FixedUpdate()
  {
    _rigidBody.transform.Rotate(0, _currentInput.x * _turnSpeed * Time.fixedDeltaTime, 0);
  }

  private RaycastHit GetOrientationUp()
  {
    Vector3 castOffset = (transform.up * _castheightAboveBody) + transform.forward * _currentInput.y * _forwardSpeed * Time.deltaTime;

    RaycastHit hit;
    Physics.SphereCast(transform.position + castOffset, _castSphereRadius, -transform.up, out hit, _castheightAboveBody + _groundCastDepth, _terrainLayer);

    return hit;
  }

  private void ApplyGravity()
  {
    transform.position += Physics.gravity * Time.deltaTime * _rigidBody.mass;
  }
}
