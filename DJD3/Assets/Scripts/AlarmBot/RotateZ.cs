using UnityEngine;

public class RotateZ : MonoBehaviour
{
    public float rotationSpeed = 90f; // degrees per second

    void FixedUpdate()
    {
        // Rotate around Z axis
        transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
    }
} 