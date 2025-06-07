using UnityEngine;
using System.Collections.Generic;
using System;

public class SewerGenerator : MonoBehaviour
{
    public Transform startPoint;
    public List<GameObject> hallwayPrefabs;
    public int numberOfSegments = 20;
    public int seed = 12345;

    private System.Random rng;
    private List<GameObject> allSpawnedTiles = new List<GameObject>();
    private GameObject lastPlacedTile = null;

    private int hallwayLayer;

    void Start()
    {
        hallwayLayer = LayerMask.NameToLayer("Hallway");
        if (hallwayLayer == -1)
        {
            Debug.LogError("[SEWER GENERATOR] Layer 'Hallway' does not exist. Please create it in Unity.");
            return;
        }

        rng = new System.Random(seed);
        GenerateSewer();
    }

    void GenerateSewer()
    {
        Transform currentAttachPoint = startPoint;

        for (int i = 0; i < numberOfSegments; i++)
        {
            bool placed = false;
            int attempts = 0;
            int maxAttempts = hallwayPrefabs.Count * 3;

            while (!placed && attempts < maxAttempts)
            {
                int index = rng.Next(hallwayPrefabs.Count);
                GameObject prefab = hallwayPrefabs[index];

                Debug.Log($"[GENERATION] Starting placement of tile {i + 1}, attempt {attempts + 1}, prefab: {prefab.name}");
                attempts++;

                GameObject instance = Instantiate(prefab);
                Transform entry = instance.transform.Find("EntryPoint");
                Transform exit = instance.transform.Find("ExitPoint");

                if (entry == null || exit == null)
                {
                    Debug.LogError($"Prefab '{prefab.name}' is missing EntryPoint or ExitPoint.");
                    Destroy(instance);
                    break;
                }

                // Step 1: Reset rotation
                instance.transform.rotation = Quaternion.identity;

                // Step 2: Align forward vectors
                Quaternion rotationOffset = Quaternion.FromToRotation(entry.forward, currentAttachPoint.forward);
                instance.transform.rotation = rotationOffset * instance.transform.rotation;

                // Step 3: Reset X and Z rotation only
                Vector3 euler = instance.transform.rotation.eulerAngles;
                euler.x = 0f;
                euler.z = 0f;
                instance.transform.rotation = Quaternion.Euler(euler);

                // Step 4: Align position
                Vector3 entryWorldPos = entry.position;
                Vector3 offset = currentAttachPoint.position - entryWorldPos;
                instance.transform.position += offset;

                // Step 5: Collision check (only Hallway layer colliders)
                if (IsOverlapping(instance, prefab.name))
                {
                    Destroy(instance);
                    continue;
                }

                // Placement successful
                allSpawnedTiles.Add(instance);
                lastPlacedTile = instance;
                Debug.Log($"[GENERATION] Tile {i + 1} successfully placed. Updated lastPlacedTile to '{instance.name}'.");

                currentAttachPoint = instance.transform.Find("ExitPoint");
                placed = true;
            }

            if (!placed)
            {
                Debug.LogWarning($"[GENERATION] No fitting prefab found for tile {i + 1} after {attempts} attempts. Ending generation early.");
                break;
            }
        }
    }

    bool IsOverlapping(GameObject candidate, string candidateName)
    {
        Collider[] candidateColliders = candidate.GetComponentsInChildren<Collider>();

        foreach (GameObject otherTile in allSpawnedTiles)
        {
            if (otherTile == lastPlacedTile)
                continue;

            Collider[] otherColliders = otherTile.GetComponentsInChildren<Collider>();

            foreach (var cc in candidateColliders)
            {
                if (cc.isTrigger || cc.gameObject.layer != hallwayLayer) continue;

                foreach (var oc in otherColliders)
                {
                    if (oc.isTrigger || oc.gameObject.layer != hallwayLayer) continue;

                    if (cc.bounds.Intersects(oc.bounds))
                    {
                        Debug.LogWarning($"[COLLISION] Candidate '{candidateName}' collider '{cc.name}' " +
                                         $"(layer: {LayerMask.LayerToName(cc.gameObject.layer)}) " +
                                         $"overlaps with '{otherTile.name}' collider '{oc.name}'.");
                        return true;
                    }
                }
            }
        }

        return false;
    }
}
