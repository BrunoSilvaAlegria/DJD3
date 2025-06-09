using UnityEngine;

public class ProjectileShooter : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform shootPoint;
    public float projectileSpeed = 10f;
    public GameObject objectToDestroy;
    public int fuelConsumption = 50;

    [Header("Audio")]
    [SerializeField] private AudioClip noFuelSound;
    private AudioSource audioSource;

    private PlayerManager playerManager;
    [SerializeField] private GameObject objectToUntag;

    void Start()
    {
        playerManager = FindAnyObjectByType<PlayerManager>();

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        if (projectilePrefab != null && shootPoint != null)
        {
            if (playerManager.currentFuel >= fuelConsumption)
            {
                GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, shootPoint.rotation);
                Rigidbody rb = projectile.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    playerManager.SpendFuel(fuelConsumption);
                    rb.linearVelocity = shootPoint.forward * projectileSpeed;  // use velocity instead of linearVelocity

                    Destroy(objectToDestroy);
                    if (objectToUntag != null)
                    {
                        objectToUntag.tag = "Dead";
                        objectToUntag.layer = LayerMask.NameToLayer("Robot");
                    }
                }
            }
            else
            {
                PlayNoFuelSound();
            }
        }
        else
        {
            Debug.LogWarning("Projectile prefab or shoot point not assigned!");
        }
    }

    private void PlayNoFuelSound()
    {
        if (noFuelSound != null && !audioSource.isPlaying)
        {
            audioSource.PlayOneShot(noFuelSound);
        }
    }
}
