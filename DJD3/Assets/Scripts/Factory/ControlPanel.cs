using UnityEngine;

public class ControlPanel : MonoBehaviour
{

    public Animator animator;
    private bool doorOpen = false;
    [SerializeField]private bool inRange = false;

    [SerializeField] private GameObject gameObject1;
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = true;
            gameObject1.active = true;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = false;
            gameObject1.active = false;
        }
    }

    void Update()
    {
        if (inRange && !doorOpen && Input.GetKeyDown(KeyCode.F))
        {
            doorOpen = true;
            Debug.Log("Interact with Player");
            animator.SetTrigger("Open");
            gameObject1.active = false;
        }   
    }
}
