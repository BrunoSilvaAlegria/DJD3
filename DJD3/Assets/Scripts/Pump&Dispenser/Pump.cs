using UnityEngine;

public class Pump : MonoBehaviour
{
    [SerializeField] private GameObject interactableObject;
    private bool canInteract = false;
    private GameObject playerInside = null;
    private PlayerManager manager;

    [Header("Audio")]
    [SerializeField] private AudioClip interactSound;
    private AudioSource audioSource;

    private void Start()
    {
        manager = FindObjectOfType<PlayerManager>();

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
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

            PlayInteractSound();

            canInteract = false;
            interactableObject.SetActive(false);
        }
    }

    private void PlayInteractSound()
    {
        if (interactSound != null)
            audioSource.PlayOneShot(interactSound);
    }
}
