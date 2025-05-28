using UnityEngine;

public class ControlPanel : MonoBehaviour
{

    public Animator animator;
    private bool doorOpen = false;
    [SerializeField]private bool inRange = false;
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = true;
        }
    }
    void OggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = false;
        }
    }

    void Update()
    {
        if (inRange && !doorOpen && Input.GetKeyDown(KeyCode.F))
        {
            doorOpen = true;
            Debug.Log("Interact with Player");
            animator.SetTrigger("Open");
        }   
    }
}
