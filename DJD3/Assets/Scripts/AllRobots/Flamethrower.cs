using UnityEngine;

public class Flamethrower : MonoBehaviour
{
    [SerializeField] private GameObject flameObject; // The object to activate/deactivate

    void Update()
    {
        if (Input.GetMouseButton(0)) // While holding MB0
        {
            if (!flameObject.activeSelf)
                flameObject.SetActive(true);
        }
        else // When not holding MB0
        {
            if (flameObject.activeSelf)
                flameObject.SetActive(false);
        }
    }
}

