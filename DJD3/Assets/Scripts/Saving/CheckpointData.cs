using System.Collections.Generic;

[System.Serializable]
public class CheckpointData
{
    public int currentCheckpoint;
    public List<int> unlockedCheckpoints = new List<int>();
}
