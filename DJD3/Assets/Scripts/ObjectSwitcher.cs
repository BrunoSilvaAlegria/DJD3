using UnityEngine;

public class ObjectSwitcher : MonoBehaviour
{
    public GameObject objectA;
    public GameObject objectB;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            if (objectA != null && objectB != null)
            {
                bool isAActive = objectA.activeSelf;
                objectA.SetActive(!isAActive);
                objectB.SetActive(isAActive);
            }
        }
    }
}
