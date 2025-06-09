using UnityEngine;

public class CheckpointTrigger : MonoBehaviour
{
    private CheckpointManager checkpointManager;

    void Start()
    {
        // Find the CheckpointManager in the scene (optional: assign it directly if preferred)
        checkpointManager = FindObjectOfType<CheckpointManager>();

        if (checkpointManager == null)
        {
            Debug.Log("CheckpointManager not found in the scene.");
        }
        else
        {
            Debug.Log("Found CheckpointManager");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the object we collided with has a Checkpoint component
        Checkpoint checkpoint = other.GetComponent<Checkpoint>();
        if (checkpoint != null && checkpointManager != null)
        {
            // Only update if the new checkpoint ID is greater than the current one
            if (checkpoint.checkpointID > checkpointManager.currentCheckpoint)
            {
                checkpointManager.currentCheckpoint = checkpoint.checkpointID;
                checkpointManager.SaveCheckpoint();
                Debug.Log("Checkpoint updated to ID: " + checkpoint.checkpointID);
            }
        }
    }

}
