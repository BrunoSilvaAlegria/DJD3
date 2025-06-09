using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    public GameObject terrainPrefab;
    public GameObject defaultPrefab;
    public GameObject heavyPrefab;
    public Transform whereToSpawn;
    private PlayerManager playerManager;

    public float launchForce = 20f; // Speed of the projectile
    public float rotationSpeed = 100f; // How quickly it rotates with the mouse
    public int drainAmount;

    private Rigidbody rb;

    [Header("Audio")]
    [SerializeField] private AudioClip startSound;
    [SerializeField] private AudioClip loopSound;
    private AudioSource startAudioSource;
    private AudioSource loopAudioSource;

    private void Start()
    {
        playerManager = FindObjectOfType<PlayerManager>();
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;

        // Setup audio sources
        startAudioSource = gameObject.AddComponent<AudioSource>();
        loopAudioSource = gameObject.AddComponent<AudioSource>();

        if (startSound != null)
        {
            startAudioSource.clip = startSound;
            startAudioSource.playOnAwake = false;
            startAudioSource.loop = false;
            startAudioSource.Play();
        }

        if (loopSound != null)
        {
            loopAudioSource.clip = loopSound;
            loopAudioSource.playOnAwake = false;
            loopAudioSource.loop = true;
            loopAudioSource.Play();
        }

        if (whereToSpawn != null)
        {
            rb.linearVelocity = whereToSpawn.forward * launchForce;
        }
        else
        {
            Debug.LogWarning("Spawn point not set on projectile.");
        }
    }

    private void Update()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        float yaw = mouseX * rotationSpeed * Time.deltaTime;
        float pitch = -mouseY * rotationSpeed * Time.deltaTime;

        transform.Rotate(pitch, yaw, 0f, Space.Self);

        Vector3 euler = transform.eulerAngles;
        euler.z = 0f;
        transform.eulerAngles = euler;
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = transform.forward * launchForce;

        if (playerManager != null && playerManager.currentFuel > 0)
        {
            playerManager.SpendFuel(drainAmount);
        }

        if (playerManager.currentFuel <= 0)
        {
            ReplaceObject(terrainPrefab);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject hitObject = collision.gameObject;
        int terrainLayer = LayerMask.NameToLayer("Terrain");

        if (hitObject.layer == terrainLayer)
        {
            playerManager.currentHealth = 1;
            Debug.Log("Hit terrain");
            ReplaceObject(terrainPrefab);
        }
        else if (hitObject.CompareTag("Default"))
        {
            Debug.Log("Hit default tagged object");
            RobotStatus enemy = hitObject.GetComponent<RobotStatus>();
            if (enemy != null)
            {
                if (!enemy.canTakeOver)
                {
                    playerManager.currentHealth = 3;
                    Destroy(hitObject);
                    ReplaceObject(defaultPrefab);
                }
                Destroy(hitObject);
            }
        }
        else if (hitObject.CompareTag("Heavy"))
        {
            Debug.Log("Hit heavy tagged object");
            RobotStatus enemy = hitObject.GetComponent<RobotStatus>();
            if (enemy != null)
            {
                if (enemy.canTakeOver)
                {
                    playerManager.currentHealth = 5;
                    Destroy(hitObject);
                    ReplaceObject(heavyPrefab);
                }
                else
                {
                    playerManager.currentHealth = 1;
                    ReplaceObject(terrainPrefab);
                }
            }
        }
        else if (hitObject.CompareTag("Dead"))
        {
            Debug.Log("Hit Dead tagged object");
            ReplaceObject(terrainPrefab);
        }
        else
        {
            Debug.Log("Hit unknown object");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject hitObject = other.gameObject;
        if (hitObject.CompareTag("Default"))
        {
            Debug.Log("Hit default tagged object");
            RobotStatus enemy = hitObject.GetComponent<RobotStatus>();
            if (enemy != null && enemy.canTakeOver)
            {
                playerManager.currentFuel += 50;
                Destroy(hitObject);
            }
        }
    }

    private void ReplaceObject(GameObject prefab)
    {
        if (loopAudioSource != null && loopAudioSource.isPlaying)
        {
            loopAudioSource.Stop();
        }

        if (prefab != null && whereToSpawn != null)
        {
            Instantiate(prefab, whereToSpawn.position, prefab.transform.rotation);
        }

        Destroy(gameObject);
    }
}
