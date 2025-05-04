using UnityEngine;

public class RobotStatus : MonoBehaviour
{
    public int maxHealth;
    public int currentHealth;
    public bool canTakeOver = false;

    public int healthToTakeOver;
    [SerializeField] private GameObject objectToDestroy;
    public float invincible = 1.0f; // Time in seconds before the enemy can be hit again

    private float lastHitTime = -Mathf.Infinity;

    public int fuelPerHit = 25;

    private PlayerManager playerManager;

    void Start()
    {
        currentHealth = maxHealth;
        playerManager = FindAnyObjectByType<PlayerManager>();
    }

    void Update()
    {
        if (currentHealth <= healthToTakeOver)
        canTakeOver = true;
    }

    public void GetHit(int damage)
    {
        if (Time.time - lastHitTime < invincible)
        {
            Debug.Log("Hit ignored due to invincibility");
            return;
        }

        lastHitTime = Time.time;
        playerManager.GainFuel(fuelPerHit);
        currentHealth -= damage;
        Debug.Log($"Robot hit for {damage} damage");

        if (currentHealth <= 0)
        {
            Debug.Log("Robot killed");
            Destroy(objectToDestroy);
        }
    }
}
