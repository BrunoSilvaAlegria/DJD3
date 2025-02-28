using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainDetector : MonoBehaviour 
{
    Vector3 _hitpos = Vector3.zero;

    [SerializeField] float _castDistance;
    [SerializeField] LayerMask _terrainLayer;
    [SerializeField] float _castHeight = 3;
    [SerializeField] float _sphereRadius = 0.5f;

    public Vector3 GetHitPoint()
    {
        RaycastHit hit;
        Vector3 startPos = transform.position + (transform.up * _castHeight);
        Ray ray = new Ray(startPos, -transform.up);
        
        if (Physics.SphereCast(ray, _sphereRadius, out hit, _castDistance + _castHeight, _terrainLayer))
        {
            _hitpos = hit.point;
        }
        else
        {
            _hitpos = startPos;
        }

        return _hitpos;
    }
}
