using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainDetector : MonoBehaviour 
{
    Vector3 _hitpos = Vector3.zero;

    [SerializeField] float _castDistance;
    [SerializeField] LayerMask _terrainLayer;
    int _numberOfHits = 0;
    [SerializeField] float _castHeight = 3;


    public Vector3 GetHitPoint()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position + (transform.up * _castHeight), -transform.up);
        Physics.Raycast(ray, out hit, _castDistance + _castHeight,_terrainLayer);
        if (hit.collider != null)
        {
            _hitpos = hit.point;
        }
        else
        {
            _hitpos = transform.position + (transform.up * _castHeight);
        }

        return _hitpos;
    }
}
