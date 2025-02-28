using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Fingers : MonoBehaviour
{
    [SerializeField] Transform _placementTarget;
    [SerializeField] Transform _handTransform;
    [SerializeField] float _stepSize = 0.5f;
    [SerializeField] float _fingerSpeed = 5f;
    [SerializeField] float _liftHeight = 0.3f;
    [SerializeField] float _minDistanceTolerance = 0.05f;
    [SerializeField] Fingers _pairedFinger; // The finger that moves opposite to this one

    Vector3 _targetPosition;
    bool _isGrounded = true;

    public enum StepPhases
    {
        RESTING,
        LIFTING,
        MOVING
    }

    public StepPhases _currentPhase = StepPhases.RESTING;
    public UnityEvent<bool> OnPlantedChange;

    void Start()
    {
        _targetPosition = _placementTarget.position; // Set initial position
    }

    void Update()
    {
        if (ShouldStep() && _currentPhase == StepPhases.RESTING && (_pairedFinger == null || _pairedFinger._currentPhase == StepPhases.RESTING))
        {
            _targetPosition = GetLiftPosition();
            _currentPhase = StepPhases.LIFTING;
            OnPlantedChange?.Invoke(false);
        }

        if (_currentPhase == StepPhases.LIFTING && ReachedTargetPosition())
        {
            _targetPosition = _placementTarget.position;
            _currentPhase = StepPhases.MOVING;
        }

        if (_currentPhase == StepPhases.MOVING && ReachedTargetPosition())
        {
            _currentPhase = StepPhases.RESTING;
            OnPlantedChange?.Invoke(true);
        }

        MoveFinger();
    }

    bool ShouldStep()
    {
        return Vector3.Distance(transform.position, _placementTarget.position) > _stepSize;
    }

    bool ReachedTargetPosition()
    {
        return Vector3.Distance(transform.position, _targetPosition) < _minDistanceTolerance;
    }

    void MoveFinger()
    {
        if (_currentPhase != StepPhases.RESTING)
        {
            transform.position = Vector3.MoveTowards(transform.position, _targetPosition, _fingerSpeed * Time.deltaTime);
        }
    }

    Vector3 GetLiftPosition()
    {
        Vector3 midPoint = (transform.position + _placementTarget.position) / 2;
        Vector3 liftPoint = midPoint + (_handTransform.up * _liftHeight);
        return liftPoint;
    }
}
