using UnityEngine;

public class AICombat : MonoBehaviour
{
    public float timeTillHit = 2f;
    public int damageAmount = 1;

    private PlayerManager playerManager;
    private float timer;

    void Start()
    {
        // Find the PlayerManager in the scene
        playerManager = FindObjectOfType<PlayerManager>();

        if (playerManager == null)
        {
            Debug.LogError("PlayerManager not found in the scene!");
        }
    }

    public void PerformHit()
    {
        playerManager.SpendHealth(damageAmount);
    }
}
