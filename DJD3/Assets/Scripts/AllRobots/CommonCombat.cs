using UnityEngine;
using System.Collections;

public class Combat : MonoBehaviour
{
    public Animator animator;
    public float punchCooldown = 0.5f; // Cooldown time in seconds
    [SerializeField] private bool isAttacking = false;
    public Transform raycastOrigin; // Assign this in the Inspector
    public float raycastDistance = 100f; // Adjust as needed
    public GameObject hitEffectPrefab; // Assign in Inspector
    
    // Reference to the scripts you want to disable
    public PlayerMovement playerMovement; // Assign in Inspector
    public ChangeCharacter characterSwitch; // Assign in Inspector
    
    // Reference to the object you want to disable
    public GameObject objectToDisable; // Assign in Inspector
    public GameObject cameraToDisable; // Assing in Inspector
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
            ShootRaycast();
        }
    }

    // This function is called at the end of the punch animation using an Animation Event
    public void OnPunchEnd()
    {
        animator.SetTrigger("ReturnArm"); // Play the return animation
    }

    // This function is called at the end of the return animation using an Animation Event
    public void OnReturnEnd()
    {
        Debug.Log("Punched");
    }

    private IEnumerator PunchCooldown()
    {
        yield return new WaitForSeconds(punchCooldown);
        isAttacking = false;
    }

    private void ShootRaycast()
    {
        if (raycastOrigin == null)
        {
            Debug.LogWarning("Raycast origin not assigned!");
            return;
        }

        RaycastHit hit;
        if (Physics.Raycast(raycastOrigin.position, raycastOrigin.forward, out hit, raycastDistance))
        {
            Debug.Log("Hit: " + hit.collider.name);
            
            // Instantiate effect at hit point
            if (hitEffectPrefab != null)
            {
                Instantiate(hitEffectPrefab, hit.point, Quaternion.identity);
                
                // Disable the select scripts after instantiating the effect
                if (playerMovement != null)
                {
                    playerMovement.enabled = false;
                }

                if (characterSwitch != null)
                {
                    characterSwitch.enabled = false;
                }

                // Disable the specific object
                if (objectToDisable != null)
                {
                    objectToDisable.SetActive(false);
                    cameraToDisable.SetActive(false);
                }
            }
            else
            {
                Debug.LogWarning("Hit effect prefab not assigned!");
            }
        }
        else
        {
            Debug.Log("No hit");
        }
    }
}
