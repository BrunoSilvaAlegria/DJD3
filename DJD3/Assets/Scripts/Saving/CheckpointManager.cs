using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class CheckpointManager : MonoBehaviour
{
    public int currentCheckpoint = -1;
    public List<int> unlockedCheckpoints = new List<int>();

    private string savePath;

    void Awake()
    {
        savePath = Path.Combine(Application.persistentDataPath, "checkpoint.json");
        LoadCheckpoint();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            DeleteCheckpointData();
        }
    }

    public void SaveCheckpoint()
    {
        if (!unlockedCheckpoints.Contains(currentCheckpoint))
        {
            unlockedCheckpoints.Add(currentCheckpoint);
        }

        CheckpointData data = new CheckpointData
        {
            currentCheckpoint = currentCheckpoint,
            unlockedCheckpoints = unlockedCheckpoints
        };

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
            unlockedCheckpoints = data.unlockedCheckpoints ?? new List<int>();
            Debug.Log("Checkpoint loaded: " + currentCheckpoint);
        }
        else
        {
            Debug.Log("No checkpoint save file found. Starting fresh.");
        }
    }

    public void DeleteCheckpointData()
    {
        if (File.Exists(savePath))
        {
            File.Delete(savePath);
            currentCheckpoint = -1;
            unlockedCheckpoints.Clear();
            Debug.Log("Checkpoint data deleted.");
        }
        else
        {
            Debug.Log("No checkpoint file to delete.");
        }
    }
}
