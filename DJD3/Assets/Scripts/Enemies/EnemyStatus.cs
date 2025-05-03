using UnityEngine;

public class EnemyStatus : MonoBehaviour
{
    public int maxHealth;
    public int currentHealth;
    public bool canTakeOver;

    [SerializeField] private GameObject objectToDestroy;
    public float invincible = 1.0f; // Time in seconds before the enemy can be hit again

    private float lastHitTime = -Mathf.Infinity;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void GetHit(int damage)
    {
        if (Time.time - lastHitTime < invincible)
        {
            Debug.Log("Hit ignored due to invincibility");
            return;
        }

        lastHitTime = Time.time;
        currentHealth -= damage;
        Debug.Log($"Robot hit for {damage} damage");

        if (currentHealth <= 0)
        {
            Debug.Log("Robot killed");
            Destroy(objectToDestroy);
        }
    }
}
