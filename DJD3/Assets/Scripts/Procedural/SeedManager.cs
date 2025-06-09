using UnityEngine;

public class SeedManager : MonoBehaviour
{
    public static SeedManager Instance;

    public int Seed { get; private set; }
    public bool UseRandomSeed { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetSeed(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            Seed = Random.Range(0, int.MaxValue);
            UseRandomSeed = true;
        }
        else if (int.TryParse(input, out int parsedSeed))
        {
            Seed = parsedSeed;
            UseRandomSeed = false;
        }
        else
        {
            Debug.LogWarning("[SeedManager] Invalid seed input. Using random seed.");
            Seed = Random.Range(0, int.MaxValue);
            UseRandomSeed = true;
        }
    }
}
