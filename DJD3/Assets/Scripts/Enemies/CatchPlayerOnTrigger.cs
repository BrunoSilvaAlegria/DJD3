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

    public GameObject objectToSpawn;
    public Transform[] spawnPoints; // Assign 4 Transforms in the Inspector

    private GameObject detectedTarget; // Updated to GameObject

    private void Start()
    {
        if (caughtMaterial == null)
            Debug.LogWarning("Caught material is not assigned.");

        if (animatorObject1 == null || animatorObject2 == null)
            Debug.LogWarning("One or both Animator references are missing.");
    }

    private void Update()
    {
        //DetectNearbyPlayerDetectors();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
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
