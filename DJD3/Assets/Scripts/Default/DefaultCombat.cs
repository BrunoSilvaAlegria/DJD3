using UnityEngine;
using System.Collections;

public class DefaultCombat : MonoBehaviour
{
    [SerializeField]private Animator animator;
    [SerializeField]private float punchCooldown = 1f; // Cooldown time in seconds
    [SerializeField]private bool isAttacking = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && !isAttacking)
        {
            isAttacking = true;
            animator.SetTrigger("DoublePunching");
            StartCoroutine(DoublePunchCooldown()); // Start cooldown immediately
        }
    }

    // This function is called at the end of the punch animation using an Animation Event
    public void OnDoublePunchEnd()
    {
        animator.SetTrigger("ArmsReturning"); // Play the return animation
    }

    // This function is called at the end of the return animation using an Animation Event
    public void OnReturnEnd()
    {
        Debug.Log("Double Punched");
    }

    private IEnumerator DoublePunchCooldown()
    {
        yield return new WaitForSeconds(punchCooldown);
        isAttacking = false;
    }
}
