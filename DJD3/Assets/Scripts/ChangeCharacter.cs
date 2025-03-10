using UnityEngine;

public class ChangeCharacter : MonoBehaviour
{
    private GameObject targetCharacter;
    private GameObject currentCharacter;
    private GameObject previousCharacter;
    private bool canControl = false;

    void Start()
    {
        targetCharacter = null;
        currentCharacter = gameObject; // Start with the initial character
    }

    void Update()
    {
        if (Input.GetKeyDown("c") && canControl)
        {
            Debug.Log("C pressed");
            ChangeController();
        }
    }

    private void ChangeController()
    {
        if (targetCharacter == null)
        {
            Debug.LogWarning("No target character selected.");
            return;
        }

        DisableAllCharacters(); // Disable all movement, cameras, and scripts
        SetActiveCharacter(targetCharacter); // Activate only the new character
        //Destroy(gameObject);
        currentCharacter = targetCharacter;
        targetCharacter = previousCharacter;
        currentCharacter = gameObject;
    }

    private void DisableAllCharacters()
    {
        GameObject[] allCharacters = GameObject.FindGameObjectsWithTag("Controllable");
        foreach (GameObject character in allCharacters)
        {
            // Disable movement
            PlayerMovement movement = character.GetComponent<PlayerMovement>();
            if (movement != null)
            {
                movement.enabled = false;
            }

            // Disable camera
            Camera cam = GetCameraFromCharacter(character);
            if (cam != null)
            {
                cam.gameObject.SetActive(false);
            }

            // Disable this script
            ChangeCharacter script = character.GetComponent<ChangeCharacter>();
            if (script != null)
            {
                script.enabled = false;
            }
        }
    }

    private void SetActiveCharacter(GameObject character)
    {
        if (character != null)
        {
            // Enable movement
            PlayerMovement movement = character.GetComponent<PlayerMovement>();
            if (movement != null)
            {
                movement.enabled = true;
            }

            // Enable camera
            Camera cam = GetCameraFromCharacter(character);
            if (cam != null)
            {
                cam.gameObject.SetActive(true);
            }

            // Enable this script on the new character
            ChangeCharacter script = character.GetComponent<ChangeCharacter>();
            if (script != null)
            {
                script.enabled = true;
            }

            Debug.Log($"{character.name} is now the active character.");
        }
    }

    private Camera GetCameraFromCharacter(GameObject character)
    {
        if (character == null) return null;
        return character.GetComponentInChildren<Camera>(true); // Find even disabled cameras
    }

    private void OnTriggerEnter(Collider target)
    {
        if (target.CompareTag("Controllable") && target.gameObject != currentCharacter)
        {
            Debug.Log($"Entered trigger of {target.name}");
            targetCharacter = target.gameObject;
            canControl = true;
        }
    }
    private void OnTriggerExit(Collider target)
    {
        if (target.CompareTag("Controllable") && target.gameObject != currentCharacter)
        {
            Debug.Log($"Exited trigger of {target.name}");
            targetCharacter = null;
            canControl = false;
        }
    }
}
