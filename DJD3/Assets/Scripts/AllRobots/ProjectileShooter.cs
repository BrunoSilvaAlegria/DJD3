using UnityEngine;

public class ProjectileShooter : MonoBehaviour
{
    public GameObject projectilePrefab; // Assign your projectile prefab in the Inspector
    public Transform shootPoint; // The position where the projectile is instantiated
    public float projectileSpeed = 10f; // Speed of the projectile
    public GameObject objectToDestroy;
    public int fuelConsumption = 50;
    private PlayerManager playerManager;
    [SerializeField] private GameObject objectToUntag;

    void Start()
    {
        playerManager = FindAnyObjectByType<PlayerManager>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left Mouse Button
        {
            Shoot();
        }
    }

    void Shoot()
    {
        if (projectilePrefab != null && shootPoint != null && playerManager.currentFuel >= fuelConsumption)
        {
            GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, shootPoint.rotation);
            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            if (rb != null)
            {
                playerManager.SpendFuel(fuelConsumption);
                rb.linearVelocity = shootPoint.forward * projectileSpeed;
                Destroy(objectToDestroy);
                if (objectToUntag != null)
                    objectToUntag.tag = "Dead";
                    objectToUntag.layer = LayerMask.NameToLayer("Robot");
            }
        }
        else
        {
            Debug.LogWarning("Projectile prefab or shoot point not assigned!");
        }
    }
}
