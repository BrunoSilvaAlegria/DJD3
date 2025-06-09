using UnityEngine;

public class Dispenser : MonoBehaviour
{
    [SerializeField] private GameObject interactableObject;
    [SerializeField] private GameObject replacementPrefab;

    private bool canInteract = false;
    private GameObject playerInside = null;
    private Transform spawnPoint;

    [Header("Audio")]
    [SerializeField] private AudioClip interactSound;
    private AudioSource audioSource;

    private void Start()
    {
        Transform child = transform.Find("SpawnPoint");
        spawnPoint = child;

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

            PlayInteractSound();

            if (playerInside.layer == LayerMask.NameToLayer("inControll"))
            {
                PlayerManager manager = FindObjectOfType<PlayerManager>();
                if (manager != null)
                {
                    manager.GainHealth(25);
                }
                else
                {
                    Debug.LogWarning("PlayerManager not found in the scene.");
                }
            }
            else
            {
                Vector3 spawnPos = spawnPoint != null ? spawnPoint.position : playerInside.transform.position;
                Destroy(playerInside);
                if (replacementPrefab != null)
                {
                    Instantiate(replacementPrefab, spawnPos, Quaternion.identity);
                }
                else
                {
                    Debug.LogWarning("Replacement prefab not assigned.");
                }
            }

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
