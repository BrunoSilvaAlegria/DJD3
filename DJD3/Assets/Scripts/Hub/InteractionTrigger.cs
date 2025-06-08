using UnityEngine;

public class InteractionTrigger : MonoBehaviour
{
    public GameObject objectToShow;          // MeshRenderer object shown while player is inside
    public GameObject objectToToggle;        // Object activated/deactivated when pressing F
    public MonoBehaviour scriptToToggle;     // Script enabled/disabled when pressing F
    public bool canInteract { get; private set; } = false;

    private MeshRenderer meshRenderer;
    private bool toggled = false;

    private void Start()
    {
        if (objectToShow != null)
        {
            meshRenderer = objectToShow.GetComponent<MeshRenderer>();
            if (meshRenderer != null)
                meshRenderer.enabled = false;
        }

        if (objectToToggle != null)
            objectToToggle.SetActive(false);

        if (scriptToToggle != null)
            scriptToToggle.enabled = true;

        // Hide and lock cursor at start
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if (canInteract && Input.GetKeyDown(KeyCode.F))
        {
            toggled = !toggled;

            // Toggle object
            if (objectToToggle != null)
                objectToToggle.SetActive(toggled);

            // Toggle script
            if (scriptToToggle != null)
                scriptToToggle.enabled = !toggled;

            // Toggle cursor
            Cursor.visible = toggled;
            Cursor.lockState = toggled ? CursorLockMode.None : CursorLockMode.Locked;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (meshRenderer != null)
                meshRenderer.enabled = true;

            canInteract = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (meshRenderer != null)
                meshRenderer.enabled = false;

            canInteract = false;

            // Reset toggle state
            toggled = false;

            if (objectToToggle != null)
                objectToToggle.SetActive(false);

            if (scriptToToggle != null)
                scriptToToggle.enabled = true;

            // Hide and lock cursor again
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
