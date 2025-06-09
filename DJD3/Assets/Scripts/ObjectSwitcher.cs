using UnityEngine;

public class ObjectSwitcher : MonoBehaviour
{
    public GameObject objectA;
    public GameObject objectB;

    [Header("Sound Settings")]
    public AudioClip switchSound;
    public float pitchMin = 0.95f;
    public float pitchMax = 1.05f;

    private AudioSource audioSource;

    void Start()
    {
        // Get or add an AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1f;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            if (objectA != null && objectB != null)
            {
                bool isAActive = objectA.activeSelf;
                objectA.SetActive(!isAActive);
                objectB.SetActive(isAActive);

                PlaySwitchSound();
            }
        }
    }

    private void PlaySwitchSound()
    {
        if (switchSound != null)
        {
            audioSource.pitch = Random.Range(pitchMin, pitchMax);
            audioSource.PlayOneShot(switchSound);
        }
    }
}
