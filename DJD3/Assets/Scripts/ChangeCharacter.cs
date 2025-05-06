using TMPro;
using UnityEngine;

public class ChangeCharacter : MonoBehaviour
{
    public GameObject defaultReplacement; // Prefab for Default-tagged objects
    public GameObject heavyReplacement;   // Prefab for Heavy-tagged objects
    public GameObject objectToDestroy;    // Object to destroy when switching

    private GameObject targetObjectInTrigger = null;
    private string targetTag = " ";

    public Transform whereToSpawn;

    private PlayerManager playerManager;

    private GameObject textToActivate;

    private TextMeshProUGUI textToActivateTMP;

    private string text = "TAKE OVER";
    
    void Start()
    {
        textToActivate = GameObject.FindWithTag("TakeOver");
        textToActivateTMP = textToActivate.GetComponent<TextMeshProUGUI>();
        textToActivateTMP.SetText(" ");
        playerManager = FindObjectOfType<PlayerManager>();
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Default") || other.CompareTag("Heavy"))
        {
            targetObjectInTrigger = other.gameObject;
            targetTag = other.tag;
            textToActivateTMP.SetText(text);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == targetObjectInTrigger)
        {
            targetObjectInTrigger = null;
            targetTag = "";
            textToActivateTMP.SetText(" ");
        }
    }

    void Update()
    {
        if (targetObjectInTrigger != null && Input.GetKeyDown(KeyCode.C))
        {
            textToActivateTMP.SetText(" ");
            GameObject replacementPrefab = null;
            RobotStatus robotStatus = targetObjectInTrigger.GetComponent<RobotStatus>();
            if (targetTag == "Default")
            {
                playerManager.currentHealth = robotStatus.currentHealth;
                replacementPrefab = defaultReplacement;
                Debug.Log("Switching Default character");
            }
            else if (targetTag == "Heavy")
            {
                playerManager.currentHealth = robotStatus.currentHealth;
                replacementPrefab = heavyReplacement;
                Debug.Log("Switching Heavy character");
            }

            if (replacementPrefab != null)
            {
               ReplaceObject(replacementPrefab);
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

    private void ReplaceObject(GameObject prefab)
    {
        if (prefab != null && whereToSpawn != null)
        {
            Instantiate(prefab, whereToSpawn.position, prefab.transform.rotation);
        }
    }
}
