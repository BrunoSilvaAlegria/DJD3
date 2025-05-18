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

    private Rigidbody rb;

    private void Start()
    {
        playerManager = FindObjectOfType<PlayerManager>();
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;

        if (whereToSpawn != null)
        {
            // Launch the projectile in the direction the spawn point is facing
            rb.linearVelocity = whereToSpawn.forward * launchForce;
        }
        else
        {
            Debug.LogWarning("Spawn point not set on projectile.");
        }
    }

    private void Update()
    {
        // Get mouse input
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // Calculate yaw and pitch
        float yaw = mouseX * rotationSpeed * Time.deltaTime;
        float pitch = -mouseY * rotationSpeed * Time.deltaTime;

        // Apply pitch (around X) and yaw (around Y) only
        transform.Rotate(pitch, yaw, 0f, Space.Self);

        // Lock roll (Z) rotation
        Vector3 euler = transform.eulerAngles;
        euler.z = 0f;
        transform.eulerAngles = euler;
    }


    private void FixedUpdate()
    {
        // Maintain constant forward velocity
        rb.linearVelocity = transform.forward * launchForce;
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
                if (enemy.canTakeOver)
                {
                    playerManager.currentHealth = 3;
                    Destroy(hitObject);
                    ReplaceObject(defaultPrefab);
                }
                else
                {
                    playerManager.currentHealth = 1;
                    ReplaceObject(terrainPrefab);
                }
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

        Destroy(gameObject); // Destroy the projectile after collision
    }

    private void ReplaceObject(GameObject prefab)
    {
        if (prefab != null && whereToSpawn != null)
        {
            Instantiate(prefab, whereToSpawn.position, prefab.transform.rotation);
        }
    }
}
