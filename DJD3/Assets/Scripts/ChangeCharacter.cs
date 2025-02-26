using UnityEngine;

public class ChangeCharacter : MonoBehaviour
{
    
    [SerializeField]
    private Camera mainCamera;
    private GameObject targetCharacter;
    private GameObject currentChild;
    private Vector3 currentCameraPosition;
    private Quaternion currentCameraRotation;
    private bool canControll = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("c") && canControll == true)
        {
            Debug.Log("C pressed");
            ChangeController();
            canControll = false;
        }
    }
    private void FindChildWithTag(GameObject parent, string tag)
    {
        foreach (Transform transform in parent.transform)
        {
            if (transform.CompareTag(tag))
            {
                currentChild = transform.gameObject;
                Debug.Log($"Current child name = {currentChild.name}");
            }
        }
    }

    private void ChangeCameraPosition()
    {
        
        currentCameraPosition = mainCamera.transform.position;
        currentCameraRotation = mainCamera.transform.rotation;

        FindChildWithTag(targetCharacter, "CamPos");
        mainCamera.transform.position = currentChild.transform.position;
        mainCamera.transform.rotation = currentChild.transform.rotation;
        Debug.Log("Camera moved to target position");
    }
    private void ChangeController()
    {
        ChangeCameraPosition();
        Debug.Log("Controller Changed");
    }

    private void OnTriggerEnter(Collider target)
    {
        Debug.Log("Trigger Entered");
        if (target.CompareTag("Controllable"))
        {
            Debug.Log("Controllable found");
            targetCharacter = target.gameObject;
            Debug.Log($"target name = {targetCharacter.name}");
            canControll = true;    
        }
            
    }
}
