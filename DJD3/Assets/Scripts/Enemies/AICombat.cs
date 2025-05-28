using UnityEngine;

public class AICombat : MonoBehaviour
{
    public float timeTillHit = 2f;
    public int damageAmount = 1;

    private PlayerManager playerManager;
    private float timer;
    private bool inRange = false;

    void Start()
    {
        playerManager = FindObjectOfType<PlayerManager>();

        if (playerManager == null)
        {
            Debug.LogError("PlayerManager not found in the scene!");
        }
    }

    public void PerformHit()
    {
        if (inRange)
        {
            playerManager.SpendHealth(damageAmount);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = true;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = false;
        }
    }
}
