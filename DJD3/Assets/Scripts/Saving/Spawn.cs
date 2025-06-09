using UnityEngine;

public class Spawn : MonoBehaviour
{
    [SerializeField] private GameObject characterPrefab;

    void Start()
    {
        CheckpointManager checkpointManager = FindObjectOfType<CheckpointManager>();
        if (checkpointManager == null)
        {
            Debug.LogWarning("CheckpointManager not found in the scene.");
            return;
        }

        int targetID = checkpointManager.currentCheckpoint;
        Checkpoint[] allCheckpoints = FindObjectsOfType<Checkpoint>();

        foreach (Checkpoint checkpoint in allCheckpoints)
        {
            if (checkpoint.checkpointID == targetID)
            {
                Transform spawnPoint = checkpoint.transform.Find("SpawnPoint");
                if (spawnPoint != null)
                {
                    Instantiate(characterPrefab, spawnPoint.position, Quaternion.identity);
                    Debug.Log("Spawned character at checkpoint ID: " + targetID);
                }
                else
                {
                    Debug.LogWarning("SpawnPoint child not found under checkpoint ID: " + targetID);
                }

                return;
            }
        }

        Debug.LogWarning("No matching checkpoint found with ID: " + targetID);
    }
}
