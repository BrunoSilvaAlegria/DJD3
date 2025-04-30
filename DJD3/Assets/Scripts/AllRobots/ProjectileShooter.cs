using UnityEngine;

public class ProjectileShooter : MonoBehaviour
{
    public GameObject projectilePrefab; // Assign your projectile prefab in the Inspector
    public Transform shootPoint; // The position where the projectile is instantiated
    public float projectileSpeed = 10f; // Speed of the projectile
    public GameObject objectToDestroy;
    [SerializeField] private GameObject objectToUntag;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left Mouse Button
        {
            Shoot();
        }
    }

    void Shoot()
    {
        if (projectilePrefab != null && shootPoint != null)
        {
            GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, shootPoint.rotation);
            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = shootPoint.forward * projectileSpeed;
                Destroy(objectToDestroy);
                if (objectToUntag != null)
                    objectToUntag.tag = "Dead";
            }
        }
        else
        {
            Debug.LogWarning("Projectile prefab or shoot point not assigned!");
        }
    }
}
