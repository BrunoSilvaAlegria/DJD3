using UnityEngine;

public class Lift : MonoBehaviour
{
    public Animator animator;
    private bool liftUP = false;
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !liftUP)
        {
            liftUP = true;
            Debug.Log("Touched Player");
            animator.SetTrigger("Lift");
        }
    }
}
