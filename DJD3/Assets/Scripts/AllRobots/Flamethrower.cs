using UnityEngine;

public class Flamethrower : MonoBehaviour
{
    [SerializeField] private GameObject flameObject;
    [SerializeField] private Transform targetToFollow;

    [SerializeField] private Vector3 positionOffset = Vector3.zero;
    [SerializeField] private Vector3 rotationOffset = Vector3.zero;
    public int fuelRate;

    [Header("Audio")]
    [SerializeField] private AudioClip noFuelSound;
    [SerializeField] private AudioClip flameLoopSound;

    private AudioSource audioSource;

    private PlayerManager playerManager;

    void Start()
    {
        playerManager = FindAnyObjectByType<PlayerManager>();

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.loop = false;
    }

    void FixedUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            if (playerManager.currentFuel > 0)
            {
                if (!flameObject.activeSelf)
                {
                    flameObject.SetActive(true);
                    PlayFlameLoop();
                }

                playerManager.SpendFuel(fuelRate);
            }
            else
            {
                if (flameObject.activeSelf)
                {
                    flameObject.SetActive(false);
                    StopFlameLoop();
                }

                PlayNoFuelSound();
            }
        }
        else
        {
            if (flameObject.activeSelf)
            {
                flameObject.SetActive(false);
                StopFlameLoop();
            }
        }
    }

    void LateUpdate()
    {
        if (targetToFollow != null)
        {
            transform.position = targetToFollow.position + targetToFollow.TransformDirection(positionOffset);
            transform.rotation = targetToFollow.rotation * Quaternion.Euler(rotationOffset);
        }
    }

    private void PlayNoFuelSound()
    {
        if (noFuelSound != null && !audioSource.isPlaying)
        {
            audioSource.loop = false;
            audioSource.Stop();
            audioSource.PlayOneShot(noFuelSound);
        }
    }

    private void PlayFlameLoop()
    {
        if (flameLoopSound != null && (!audioSource.isPlaying || audioSource.clip != flameLoopSound))
        {
            audioSource.clip = flameLoopSound;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    private void StopFlameLoop()
    {
        if (audioSource.isPlaying && audioSource.clip == flameLoopSound)
        {
            audioSource.Stop();
        }
    }
}
