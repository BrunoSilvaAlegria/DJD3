using UnityEngine;
using System.Collections;

public class CommonCombat : MonoBehaviour
{
    public Animator animator;
    public float punchCooldown = 0.5f;
    [SerializeField] private bool isAttacking = false;
    public PlayerMovement playerMovement; // Assign in Inspector

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
}
