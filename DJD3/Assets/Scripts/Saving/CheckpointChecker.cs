using UnityEngine;

public class CheckpointChecker : MonoBehaviour
{
    public int checkpointID;

    void Start()
    {
        CheckpointManager checkpointManager = FindObjectOfType<CheckpointManager>();

        if (checkpointManager != null)
        {
            if (!checkpointManager.unlockedCheckpoints.Contains(checkpointID))
            {
                Destroy(gameObject);
            }
        }
        else
        {
            Debug.LogWarning("CheckpointManager not found. Cannot verify checkpoint.");
        }
    }

    // Call this method from a UI Button to update the current checkpoint
    public void SetCheckpointToSelf()
    {
        CheckpointManager checkpointManager = FindObjectOfType<CheckpointManager>();

        if (checkpointManager != null)
        {
            if (checkpointID > checkpointManager.currentCheckpoint)
            {
                checkpointManager.currentCheckpoint = checkpointID;
                checkpointManager.SaveCheckpoint();
                Debug.Log("Checkpoint manually set to ID: " + checkpointID);
            }
            else
            {
                Debug.Log("Attempted to set an older or equal checkpoint. Ignored.");
            }
        }
        else
        {
            Debug.LogWarning("CheckpointManager not found. Cannot set checkpoint.");
        }
    }
}
