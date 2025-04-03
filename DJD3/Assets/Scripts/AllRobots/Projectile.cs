using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject replacementPrefab;
    public Transform whereToSpawn;

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the collided object has the "Controllable" tag
        if (collision.gameObject.CompareTag("Controllable"))
        {
            Debug.Log("controlled");
            Destroy(collision.gameObject);  // Destroy the "Controllable" object
        }
        else
        {
            ReplaceObject();
        }
        Destroy(gameObject);  // Destroy the projectile itself
    }

    public void ReplaceObject()
    {
        if (replacementPrefab != null)
        {
            // Use the prefab's rotation rather than whereToSpawn's rotation
            Instantiate(replacementPrefab, whereToSpawn.position, replacementPrefab.transform.rotation);
        }
    }
}
