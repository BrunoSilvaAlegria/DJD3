using System.Collections.Generic;
using UnityEngine;

public class CatchPlayerOnTrigger : MonoBehaviour
{
    public float detectionRadius = 10f;
    public LayerMask detectionLayer;

    public Material caughtMaterial;
    public List<Light> spotlightsToChange;
    public RotateZ rotateZ;

    public Animator animatorObject1;
    public Animator animatorObject2;

    public string animationState1 = "Door1";
    public string animationState2 = "Door2";

    private bool hasSpawned = false;
    private int obstacleCount = 0;

    public float obstacleCooldown = 2f;
    private float cooldownTimer = 0f;
    private bool inCooldown = false;

    public GameObject objectToSpawn;
    public Transform[] spawnPoints;

    private GameObject detectedTarget;

    [Header("Audio")]
    [SerializeField] private AudioClip idleLoopSound;
    [SerializeField] private AudioClip detectedLoopSound;
    private AudioSource audioSource;

    private void Start()
    {
        if (caughtMaterial == null)
            Debug.LogWarning("Caught material is not assigned.");

        if (animatorObject1 == null || animatorObject2 == null)
            Debug.LogWarning("One or both Animator references are missing.");

        // Setup Audio
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.loop = true;
        audioSource.playOnAwake = false;

        // 3D Spatial Sound Settings
        audioSource.spatialBlend = 1f;
        audioSource.minDistance = 5f;
        audioSource.maxDistance = 20f;
        audioSource.rolloffMode = AudioRolloffMode.Linear;

        if (idleLoopSound != null)
        {
            audioSource.clip = idleLoopSound;
            audioSource.Play();
        }
    }

    private void Update()
    {
        if (inCooldown)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0f)
            {
                inCooldown = false;
                Debug.Log("Detector cooldown complete. Ready to detect again.");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            obstacleCount++;
            Debug.Log($"Touched obstacle: {other.name} | Total obstacles: {obstacleCount}");

            // Reset cooldown if already running
            inCooldown = true;
            cooldownTimer = obstacleCooldown;
        }

        if (other.CompareTag("Player") && obstacleCount == 0 && !inCooldown)
        {
            detectedTarget = other.gameObject;
            ChangeChildrenMaterials();
            UpdateSpotlights();
            rotateZ.rotationSpeed = 300;
            PlayAnimations();
            DetectNearbyPlayerDetectors();

            if (!hasSpawned)
            {
                SpawnObjects();
                hasSpawned = true;
            }

            // Switch to detected loop sound
            if (detectedLoopSound != null && audioSource.clip != detectedLoopSound)
            {
                audioSource.Stop();
                audioSource.clip = detectedLoopSound;
                audioSource.Play();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            obstacleCount = Mathf.Max(0, obstacleCount - 1);
            Debug.Log($"Obstacle left: {other.name} | Remaining obstacles: {obstacleCount}");

            // Start cooldown regardless of how many obstacles remain
            inCooldown = true;
            cooldownTimer = obstacleCooldown;
            Debug.Log($"Cooldown started for {obstacleCooldown} seconds.");
        }
    }

    private void ChangeChildrenMaterials()
    {
        foreach (Transform child in transform)
        {
            Renderer renderer = child.GetComponent<Renderer>();
            if (renderer != null)
                renderer.material = caughtMaterial;
        }
    }

    private void UpdateSpotlights()
    {
        foreach (Light spotlight in spotlightsToChange)
        {
            if (spotlight != null)
            {
                Color currentColor = spotlight.color;
                currentColor.g = 0f;
                spotlight.color = currentColor;
            }
        }
    }

    private void PlayAnimations()
    {
        if (animatorObject1 != null && !string.IsNullOrEmpty(animationState1))
            animatorObject1.Play(animationState1);

        if (animatorObject2 != null && !string.IsNullOrEmpty(animationState2))
            animatorObject2.Play(animationState2);
    }

    private void DetectNearbyPlayerDetectors()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius, detectionLayer);
        foreach (Collider hit in hits)
        {
            PlayerDetector detector = hit.GetComponent<PlayerDetector>();
            if (detector != null)
            {
                Debug.Log($"Found {hit}");
                detector.target = detectedTarget;
            }
        }
    }

    public void SpawnObjects()
    {
        for (int i = 0; i < 4; i++)
        {
            Instantiate(objectToSpawn, spawnPoints[i].position, spawnPoints[i].rotation);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
