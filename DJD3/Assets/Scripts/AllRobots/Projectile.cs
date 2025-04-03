using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject replacementPrefab;
    public Transform whereToSpawn;

    private void OnCollisionEnter(Collision collision)
    {
        ReplaceObject();
        Destroy(gameObject);
    }
    public void ReplaceObject()
    {
        if (replacementPrefab != null)
        {
            Instantiate(replacementPrefab, whereToSpawn.position, replacementPrefab.transform.rotation);
        }
    }
}
