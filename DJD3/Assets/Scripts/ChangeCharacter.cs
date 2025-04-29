using UnityEngine;

public class ChangeCharacter : MonoBehaviour
{
    public GameObject defaultReplacement; // Prefab for Default-tagged objects
    public GameObject heavyReplacement;   // Prefab for Heavy-tagged objects
    public GameObject objectToDestroy;    // Object to destroy when switching

    private GameObject targetObjectInTrigger = null;
    private string targetTag = "";

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Default") || other.CompareTag("Heavy"))
        {
            targetObjectInTrigger = other.gameObject;
            targetTag = other.tag;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == targetObjectInTrigger)
        {
            targetObjectInTrigger = null;
            targetTag = "";
        }
    }

    void Update()
    {
        if (targetObjectInTrigger != null && Input.GetKeyDown(KeyCode.C))
        {
            GameObject replacementPrefab = null;

            if (targetTag == "Default")
            {
                replacementPrefab = defaultReplacement;
                Debug.Log("Switching Default character");
            }
            else if (targetTag == "Heavy")
            {
                replacementPrefab = heavyReplacement;
                Debug.Log("Switching Heavy character");
            }

            if (replacementPrefab != null)
            {
                GameObject newObject = Instantiate(replacementPrefab,
                    targetObjectInTrigger.transform.position,
                    targetObjectInTrigger.transform.rotation);
                newObject.transform.localScale = targetObjectInTrigger.transform.localScale;
            }

            if (objectToDestroy != null)
            {
                Destroy(objectToDestroy);
            }

            Destroy(targetObjectInTrigger);
            targetObjectInTrigger = null;
            targetTag = "";
        }
    }
}
