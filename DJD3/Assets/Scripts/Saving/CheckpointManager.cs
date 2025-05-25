using UnityEngine;
using System.IO;

public class CheckpointManager : MonoBehaviour
{
    public int currentCheckpoint = -1; // default to -1 meaning no checkpoint

    private string savePath;

    void Awake()
    {
        savePath = Path.Combine(Application.persistentDataPath, "checkpoint.json");
        LoadCheckpoint();
    }

    public void SaveCheckpoint()
    {
        CheckpointData data = new CheckpointData { currentCheckpoint = currentCheckpoint };
        File.WriteAllText(savePath, JsonUtility.ToJson(data));
        Debug.Log("Checkpoint saved to: " + savePath);
    }

    public void LoadCheckpoint()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            CheckpointData data = JsonUtility.FromJson<CheckpointData>(json);
            currentCheckpoint = data.currentCheckpoint;
            Debug.Log("Checkpoint loaded: " + currentCheckpoint);
        }
        else
        {
            Debug.Log("No checkpoint save file found. Starting fresh.");
        }
    }
}
