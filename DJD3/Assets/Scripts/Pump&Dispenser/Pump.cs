using UnityEngine;

public class Pump : MonoBehaviour
{
    [SerializeField] private GameObject interactableObject;
    private bool canInteract = false;
    private GameObject playerInside = null;
    private PlayerManager manager;

    private void Start()
    {
        manager = FindObjectOfType<PlayerManager>();
        // Find the SpawnPoint child by name
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canInteract = true;
            playerInside = other.gameObject;
            interactableObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canInteract = false;
            playerInside = null;
            interactableObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (canInteract && Input.GetKeyDown(KeyCode.F) && playerInside != null)
        {
            Debug.Log("Interaction triggered!");
            manager.GainFuel(100);

            canInteract = false;
            interactableObject.SetActive(false);
        }
    }
}
