using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshBaker : MonoBehaviour
{
    public NavMeshSurface navMeshSurface;

    public void BakeNavMesh()
    {
        if (navMeshSurface == null)
        {
            Debug.LogError("[NavMeshBaker] NavMeshSurface reference is missing!");
            return;
        }
        Debug.Log("[NavMeshBaker] Baking NavMesh...");
        navMeshSurface.BuildNavMesh();
        Debug.Log("[NavMeshBaker] NavMesh bake complete.");
    }
}
