using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using static UnityEngine.InputSystem.InputAction;

public class HandMovement : MonoBehaviour
{
    Rigidbody _rigidBody;
    [SerializeField] float _forwardMoveForce = 1;
    [SerializeField] float _turnTorque = 1;
    [SerializeField] float _maxLinearVelocity = 4;
    [SerializeField] float _maxAngularVelocity = 90;
    Vector2 _currentInput = Vector2.zero;

    void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _rigidBody.maxAngularVelocity = _maxAngularVelocity;
        _rigidBody.maxLinearVelocity = _maxLinearVelocity;
    }

    void Update()
    {

    }
    private void FixedUpdate()
    {
        _rigidBody.AddForce(transform.forward * _forwardMoveForce * Time.fixedDeltaTime * _currentInput.y,ForceMode.Acceleration);
        _rigidBody.AddRelativeTorque(transform.up * _turnTorque * Time.fixedDeltaTime * _currentInput.x,ForceMode.Acceleration);
    }

    public void OnMoveInput(CallbackContext context)
    {
        _currentInput = context.ReadValue<Vector2>();
    }
}