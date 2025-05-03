using UnityEngine;
using System.Collections;
using System;

public class CommonCombat : MonoBehaviour
{
    public Animator animator;
    public float punchCooldown = 0.5f;
    [SerializeField] private bool isAttacking = false;
    public PlayerMovement playerMovement; // Assign in Inspector
    public Transform punchOrigin; // Assign to the punch point (e.g., hand)
    public float punchRadius = 1f;
    public float knockbackDistance = 2f;
    public float knockbackDuration = 0.2f;
    public int damage = 1;
    public LayerMask robotLayer; // Set to "Robot" layer in Inspector

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !isAttacking)
        {
            isAttacking = true;
            animator.SetTrigger("Punching");
            StartCoroutine(PunchCooldown());
        }
    }

    public void OnPunchEnd()
    {
        animator.SetTrigger("ReturnArm");

        // Check for any colliders in the Robot layer within punchRadius
        Collider[] hits = Physics.OverlapSphere(punchOrigin.position, punchRadius, robotLayer);

        foreach (Collider hit in hits)
        {
            Transform target = hit.transform;
            if (target.GetComponent<EnemyStatus>() != null)
            {
                EnemyStatus enemy = target.GetComponent<EnemyStatus>();
                enemy.GetHit(damage);
            }
            StartCoroutine(ApplyKnockback(target));
        }
    }

    public void OnReturnEnd()
    {
        Debug.Log("Punched");
    }

    private IEnumerator PunchCooldown()
    {
        yield return new WaitForSeconds(punchCooldown);
        isAttacking = false;
    }

    private IEnumerator ApplyKnockback(Transform target)
    {
        // Calculate direction on X and Z axes only (ignore Y)
        Vector3 direction = (target.position - transform.position);
        direction.y = 0f;  // Zero out the Y-axis to prevent vertical movement
        direction.Normalize();

        Vector3 start = target.position;
        Vector3 end = start + direction * knockbackDistance;

        float elapsed = 0f;

        while (elapsed < knockbackDuration)
        {
            try
            {
                // Apply knockback only in the X and Z axes
                target.position = new Vector3(
                    Mathf.Lerp(start.x, end.x, elapsed / knockbackDuration),
                    target.position.y, // Keep the original Y position
                    Mathf.Lerp(start.z, end.z, elapsed / knockbackDuration)
                    );
                elapsed += Time.deltaTime;
                
            }
            catch (Exception ex)
            {
                Debug.Log("Target Destroyed, cant knockback");
            }
            yield return null;

        }

        // Final position to ensure exact knockback distance
        target.position = new Vector3(end.x, target.position.y, end.z);
    }

    // Optional: Visualize the punch radius in the editor
    private void OnDrawGizmosSelected()
    {
        if (punchOrigin != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(punchOrigin.position, punchRadius);
        }
    }
}
