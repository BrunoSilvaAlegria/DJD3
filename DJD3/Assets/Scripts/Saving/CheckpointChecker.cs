using UnityEngine;
using UnityEngine.SceneManagement;

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
            checkpointManager.currentCheckpoint = checkpointID;
            checkpointManager.SaveCheckpoint();
            Debug.Log("Checkpoint manually set to ID: " + checkpointID);
            
        }
        else
        {
            Debug.LogWarning("CheckpointManager not found. Cannot set checkpoint.");
        }
    }

    public void ChangeToFactory()
    {
        SetCheckpointToSelf();
        SceneManager.LoadSceneAsync("Factory");

    }

    public void ChangeToSewer()
    {
        SetCheckpointToSelf();
        SceneManager.LoadSceneAsync("Level1");
    }
}
