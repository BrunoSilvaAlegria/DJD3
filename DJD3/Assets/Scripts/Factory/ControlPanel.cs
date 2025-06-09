using UnityEngine;

public class ControlPanel : MonoBehaviour
{
    public Animator animator;
    private bool doorOpen = false;
    [SerializeField] private bool inRange = false;
    [SerializeField] private GameObject gameObject1;

    [Header("Audio")]
    [SerializeField] private AudioClip interactSound;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = true;
            gameObject1.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = false;
            gameObject1.SetActive(false);
        }
    }

    void Update()
    {
        if (inRange && !doorOpen && Input.GetKeyDown(KeyCode.F))
        {
            doorOpen = true;
            Debug.Log("Interact with Player");
            animator.SetTrigger("Open");
            PlayInteractSound();
            gameObject1.SetActive(false);
        }
    }

    private void PlayInteractSound()
    {
        if (interactSound != null)
            audioSource.PlayOneShot(interactSound);
    }
}
