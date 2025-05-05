using UnityEngine;

public class PlayerDetector : MonoBehaviour
{
    public float detectionRadius = 10f;
    public LayerMask terrainLayer; // Assign this in the Inspector

    public GameObject target;

    // Public getter for external access
    public GameObject Target => target;

    void Update()
    {
        DetectPlayer();
    }

    void DetectPlayer()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius);
        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                Vector3 directionToPlayer = hit.transform.position - transform.position;

                // If no terrain blocks the line of sight
                if (!Physics.Linecast(transform.position, hit.transform.position, terrainLayer))
                {
                    target = hit.gameObject;
                    return;
                }
            }
        }

        // Do not clear target if no player is visible this frame
    }

    // Optional: draw detection range in editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
