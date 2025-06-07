using UnityEngine;
using System.Collections.Generic;
using System;

public class SewerGenerator : MonoBehaviour
{
    public Transform startPoint;
    public List<GameObject> hallwayPrefabs;
    public GameObject finalAlleyPrefab;  // The special final prefab
    public int numberOfSegments = 20;    // Total number of segments including final alley
    public int seed = 12345;

    private System.Random rng;
    private List<GameObject> allSpawnedTiles = new List<GameObject>();
    private GameObject lastPlacedTile = null;
    private int hallwayLayer;

    private string lastPrefabName = null;

    void Start()
    {
        hallwayLayer = LayerMask.NameToLayer("Hallway");
        if (hallwayLayer == -1)
        {
            Debug.LogError("[SEWER GENERATOR] Layer 'Hallway' does not exist. Please create it in Unity.");
            return;
        }

        if (hallwayPrefabs == null || hallwayPrefabs.Count == 0)
        {
            Debug.LogError("[SEWER GENERATOR] No hallwayPrefabs assigned!");
            return;
        }

        if (finalAlleyPrefab == null)
        {
            Debug.LogError("[SEWER GENERATOR] Final Alley prefab not assigned!");
            return;
        }

        if (numberOfSegments < 1)
        {
            Debug.LogError("[SEWER GENERATOR] numberOfSegments must be at least 1");
            return;
        }

        rng = new System.Random(seed);
        GenerateSewer();
    }

    void GenerateSewer()
    {
        Transform currentAttachPoint = startPoint;

        // Generate main hallway segments: numberOfSegments - 1 tiles
        int mainHallwayCount = numberOfSegments - 1;

        for (int i = 0; i < mainHallwayCount; i++)
        {
            bool placed = false;
            int attempts = 0;
            int maxAttempts = hallwayPrefabs.Count * 3;

            while (!placed && attempts < maxAttempts)
            {
                int index = GetWeightedPrefabIndex();

                GameObject prefab = hallwayPrefabs[index];
                string prefabName = prefab.name;

                if (index != 0 && prefabName == lastPrefabName)
                {
                    Debug.Log($"[SKIP] Prefab '{prefabName}' cannot spawn twice in a row. Skipping.");
                    attempts++;
                    continue;
                }

                Debug.Log($"[GENERATION] Starting placement of tile {i + 1}, attempt {attempts + 1}, prefab: {prefabName}");
                attempts++;

                GameObject instance = Instantiate(prefab);
                Transform entry = instance.transform.Find("EntryPoint");
                Transform exit = instance.transform.Find("ExitPoint");

                if (entry == null || exit == null)
                {
                    Debug.LogError($"Prefab '{prefabName}' is missing EntryPoint or ExitPoint.");
                    Destroy(instance);
                    break;
                }

                instance.transform.rotation = Quaternion.identity;

                Quaternion rotationOffset = Quaternion.FromToRotation(entry.forward, currentAttachPoint.forward);
                instance.transform.rotation = rotationOffset * instance.transform.rotation;

                Vector3 euler = instance.transform.rotation.eulerAngles;
                euler.x = 0f;
                euler.z = 0f;
                instance.transform.rotation = Quaternion.Euler(euler);

                Vector3 entryWorldPos = entry.position;
                Vector3 offset = currentAttachPoint.position - entryWorldPos;
                instance.transform.position += offset;

                Physics.SyncTransforms();

                if (IsOverlapping(instance, prefabName))
                {
                    Destroy(instance);
                    continue;
                }

                allSpawnedTiles.Add(instance);
                lastPlacedTile = instance;
                lastPrefabName = prefabName;

                Debug.Log($"[GENERATION] Tile {i + 1} placed: {instance.name}. Updated lastPlacedTile and lastPrefabName.");

                currentAttachPoint = instance.transform.Find("ExitPoint");
                placed = true;
            }

            if (!placed)
            {
                Debug.LogWarning($"[GENERATION] Could not place tile {i + 1} after {attempts} attempts. Ending generation.");
                return; // End generation early, no final tile
            }
        }

        // Place the final Alley tile as the last tile
        Debug.Log("[GENERATION] Attempting to place Final Alley tile as last tile...");

        bool finalPlaced = false;
        int finalAttempts = 0;
        int finalMaxAttempts = 10; // limit attempts for final tile

        while (!finalPlaced && finalAttempts < finalMaxAttempts)
        {
            finalAttempts++;

            GameObject instance = Instantiate(finalAlleyPrefab);
            Transform entry = instance.transform.Find("EntryPoint");
            Transform exit = instance.transform.Find("ExitPoint");

            if (entry == null || exit == null)
            {
                Debug.LogError($"Final Alley prefab is missing EntryPoint or ExitPoint.");
                Destroy(instance);
                break;
            }

            instance.transform.rotation = Quaternion.identity;

            Quaternion rotationOffset = Quaternion.FromToRotation(entry.forward, currentAttachPoint.forward);
            instance.transform.rotation = rotationOffset * instance.transform.rotation;

            Vector3 euler = instance.transform.rotation.eulerAngles;
            euler.x = 0f;
            euler.z = 0f;
            instance.transform.rotation = Quaternion.Euler(euler);

            Vector3 entryWorldPos = entry.position;
            Vector3 offset = currentAttachPoint.position - entryWorldPos;
            instance.transform.position += offset;

            Physics.SyncTransforms();

            if (IsOverlapping(instance, finalAlleyPrefab.name))
            {
                Destroy(instance);
                continue;
            }

            allSpawnedTiles.Add(instance);
            lastPlacedTile = instance;
            lastPrefabName = finalAlleyPrefab.name;

            Debug.Log("[GENERATION] Final Alley tile placed successfully as last tile.");
            finalPlaced = true;
        }

        if (!finalPlaced)
        {
            Debug.LogWarning("[GENERATION] Failed to place Final Alley tile after multiple attempts.");
        }
    }

    int GetWeightedPrefabIndex()
    {
        int totalWeight = 3 + (hallwayPrefabs.Count - 1);
        int roll = rng.Next(totalWeight);

        if (roll < 3) return 0;

        return 1 + (roll - 3);
    }

    bool IsOverlapping(GameObject candidate, string candidateName)
    {
        Collider[] candidateColliders = candidate.GetComponentsInChildren<Collider>();

        foreach (GameObject otherTile in allSpawnedTiles)
        {
            if (otherTile == lastPlacedTile)
            {
                Debug.Log($"[SKIP] Skipping overlap check with lastPlacedTile: {otherTile.name}");
                continue;
            }

            Collider[] otherColliders = otherTile.GetComponentsInChildren<Collider>();

            foreach (var cc in candidateColliders)
            {
                if (cc.isTrigger || cc.gameObject.layer != hallwayLayer) continue;

                DrawBoundsBox(cc.bounds, Color.green, 2f);

                foreach (var oc in otherColliders)
                {
                    if (oc.isTrigger || oc.gameObject.layer != hallwayLayer) continue;

                    DrawBoundsBox(oc.bounds, Color.red, 2f);

                    if (cc.bounds.Intersects(oc.bounds))
                    {
                        Debug.LogWarning(
                            $"[COLLISION] Candidate '{candidateName}' collider '{cc.name}' bounds " +
                            $"center: {cc.bounds.center}, size: {cc.bounds.size} " +
                            $"overlaps with '{otherTile.name}' collider '{oc.name}' bounds " +
                            $"center: {oc.bounds.center}, size: {oc.bounds.size}.");
                        return true;
                    }
                }
            }
        }

        return false;
    }

    void DrawBoundsBox(Bounds bounds, Color color, float duration)
    {
        Vector3 center = bounds.center;
        Vector3 extents = bounds.extents;

        Vector3[] corners = new Vector3[8];
        corners[0] = center + new Vector3(extents.x, extents.y, extents.z);
        corners[1] = center + new Vector3(extents.x, extents.y, -extents.z);
        corners[2] = center + new Vector3(-extents.x, extents.y, -extents.z);
        corners[3] = center + new Vector3(-extents.x, extents.y, extents.z);

        corners[4] = center + new Vector3(extents.x, -extents.y, extents.z);
        corners[5] = center + new Vector3(extents.x, -extents.y, -extents.z);
        corners[6] = center + new Vector3(-extents.x, -extents.y, -extents.z);
        corners[7] = center + new Vector3(-extents.x, -extents.y, extents.z);

        Debug.DrawLine(corners[0], corners[1], color, duration);
        Debug.DrawLine(corners[1], corners[2], color, duration);
        Debug.DrawLine(corners[2], corners[3], color, duration);
        Debug.DrawLine(corners[3], corners[0], color, duration);

        Debug.DrawLine(corners[4], corners[5], color, duration);
        Debug.DrawLine(corners[5], corners[6], color, duration);
        Debug.DrawLine(corners[6], corners[7], color, duration);
        Debug.DrawLine(corners[7], corners[4], color, duration);

        Debug.DrawLine(corners[0], corners[4], color, duration);
        Debug.DrawLine(corners[1], corners[5], color, duration);
        Debug.DrawLine(corners[2], corners[6], color, duration);
        Debug.DrawLine(corners[3], corners[7], color, duration);
    }
}
