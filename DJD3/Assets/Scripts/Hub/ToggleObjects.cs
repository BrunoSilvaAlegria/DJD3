using UnityEngine;

public class ToggleObjects : MonoBehaviour
{
    public GameObject objectA;
    public GameObject objectB;

    public void Toggle()
    {
        if (objectA.activeSelf)
        {
            objectA.SetActive(false);
            objectB.SetActive(true);
        }
        else
        {
            objectA.SetActive(true);
            objectB.SetActive(false);
        }
    }
}
