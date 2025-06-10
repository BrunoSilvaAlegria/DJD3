using UnityEngine;
using System.Collections;

public class HeavySpawner : MonoBehaviour
{
    public GameObject prefabToSpawn;

    private GameObject currentSpawned;
    private bool isWaitingToRespawn = false;

    void Start()
    {
        SpawnNewObject();
    }

    void Update()
    {
        if (currentSpawned == null && !isWaitingToRespawn)
        {
            StartCoroutine(RespawnAfterDelay(10f));
        }
    }

    void SpawnNewObject()
    {
        currentSpawned = Instantiate(prefabToSpawn, transform.position, transform.rotation);
    }

    IEnumerator RespawnAfterDelay(float delay)
    {
        isWaitingToRespawn = true;
        yield return new WaitForSeconds(delay);
        SpawnNewObject();
        isWaitingToRespawn = false;
    }
}
