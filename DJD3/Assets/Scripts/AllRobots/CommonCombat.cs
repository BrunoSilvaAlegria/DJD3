using UnityEngine;
using System.Collections;

public class CommonCombat : MonoBehaviour
{
    public Animator animator;
    public float punchCooldown = 0.5f; // Cooldown time in seconds
    [SerializeField] private bool isAttacking = false;
    public GameObject projectilePrefab; // Assign your projectile prefab in the Inspector
    public Transform shootPoint; // The position where the projectile is instantiated
    public float projectileSpeed = 10f; // Speed of the projectile
    public PlayerMovement playerMovement; // Assign in Inspector
    public ChangeCharacter characterSwitch; // Assign in Inspector
    public GameObject objectToDestroy;
    [SerializeField] private GameObject flameObject;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !isAttacking)
        {
            isAttacking = true;
            animator.SetTrigger("Punching");
            StartCoroutine(PunchCooldown()); // Start cooldown immediately
        }

        if (Input.GetMouseButtonDown(0)) // Left Mouse Button
        {
            Shoot();
        }
    }

    public void OnPunchEnd()
    {
        animator.SetTrigger("ReturnArm"); // Play the return animation
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

    void Shoot()
    {
        if (projectilePrefab != null && shootPoint != null)
        {
            GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, shootPoint.rotation);
            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = shootPoint.forward * projectileSpeed; // Launch the projectile
                Destroy(objectToDestroy);
            }
        }
        else
        {
            Debug.LogWarning("Projectile prefab or shoot point not assigned!");
        }
    }
}
