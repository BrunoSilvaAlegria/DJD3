using UnityEngine;

public class Lift : MonoBehaviour
{
    public Animator animator;
    private bool liftUP = false;

    [Header("Audio")]
    [SerializeField] private AudioClip liftSound;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !liftUP)
        {
            liftUP = true;
            Debug.Log("Touched Player");
            animator.SetTrigger("Lift");
            PlayLiftSound();
        }
    }

    private void PlayLiftSound()
    {
        if (liftSound != null)
            audioSource.PlayOneShot(liftSound);
    }
}
