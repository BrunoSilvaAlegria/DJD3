using UnityEngine;

public class InteractionTrigger : MonoBehaviour
{
    public GameObject objectToShow;          
    public GameObject objectToToggle;        
    public MonoBehaviour scriptToToggle;     
    public bool canInteract { get; private set; } = false;

    private MeshRenderer meshRenderer;
    private bool toggled = false;

    [Header("Audio")]
    [SerializeField] private AudioClip interactSound;
    private AudioSource audioSource;

    private void Start()
    {
        if (objectToShow != null)
        {
            meshRenderer = objectToShow.GetComponent<MeshRenderer>();
            if (meshRenderer != null)
                meshRenderer.enabled = false;
        }

        if (objectToToggle != null)
            objectToToggle.SetActive(false);

        if (scriptToToggle != null)
            scriptToToggle.enabled = true;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    private void Update()
    {
        if (canInteract && Input.GetKeyDown(KeyCode.F))
        {
            toggled = !toggled;

            if (objectToToggle != null)
                objectToToggle.SetActive(toggled);

            if (scriptToToggle != null)
                scriptToToggle.enabled = !toggled;

            Cursor.visible = toggled;
            Cursor.lockState = toggled ? CursorLockMode.None : CursorLockMode.Locked;

            PlayInteractSound();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (meshRenderer != null)
                meshRenderer.enabled = true;

            canInteract = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (meshRenderer != null)
                meshRenderer.enabled = false;

            canInteract = false;

            toggled = false;

            if (objectToToggle != null)
                objectToToggle.SetActive(false);

            if (scriptToToggle != null)
                scriptToToggle.enabled = true;

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    private void PlayInteractSound()
    {
        if (interactSound != null)
            audioSource.PlayOneShot(interactSound);
    }
}
