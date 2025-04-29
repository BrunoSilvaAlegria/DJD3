using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject terrainPrefab;
    public GameObject defaultPrefab;
    public GameObject heavyPrefab;
    public Transform whereToSpawn;

    private void OnCollisionEnter(Collision collision)
    {
        GameObject hitObject = collision.gameObject;
        int terrainLayer = LayerMask.NameToLayer("Terrain");

        if (hitObject.layer == terrainLayer)
        {
            Debug.Log("Hit terrain");
            ReplaceObject(terrainPrefab);
        }
        else if (hitObject.CompareTag("Default"))
        {
            Debug.Log("Hit default tagged object");
            Destroy(hitObject); // Optionally destroy the object
            ReplaceObject(defaultPrefab);
        }
        else if (hitObject.CompareTag("Heavy"))
        {
            Debug.Log("Hit heavy tagged object");
            ReplaceObject(heavyPrefab);
        }
        else
        {
            Debug.Log("Hit unknown object");
        }

        Destroy(gameObject); // Destroy the projectile
    }

    private void ReplaceObject(GameObject prefab)
    {
        if (prefab != null && whereToSpawn != null)
        {
            Instantiate(prefab, whereToSpawn.position, prefab.transform.rotation);
        }
    }
}
