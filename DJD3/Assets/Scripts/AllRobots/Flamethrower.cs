using UnityEngine;

public class Flamethrower : MonoBehaviour
{
    [SerializeField] private GameObject flameObject;       // The object to activate/deactivate
    [SerializeField] private Transform targetToFollow;     // The target to follow

    [SerializeField] private Vector3 positionOffset = Vector3.zero;
    [SerializeField] private Vector3 rotationOffset = Vector3.zero;
    public int fuelRate;

    private PlayerManager playerManager;

    void Start()
    {
        playerManager = FindAnyObjectByType<PlayerManager>();
    }

    void FixedUpdate()
    {
        // Flame control logic
        if (Input.GetMouseButton(0) && playerManager.currentFuel > 0) // While holding MB0
        {
            if (!flameObject.activeSelf)
                flameObject.SetActive(true);
                playerManager.SpendFuel(fuelRate);
        }
        else // When not holding MB0
        {
            if (flameObject.activeSelf)
                flameObject.SetActive(false);
        }
    }

    void LateUpdate()
    {
        // Follow the target
        if (targetToFollow != null)
        {
            transform.position = targetToFollow.position + targetToFollow.TransformDirection(positionOffset);
            transform.rotation = targetToFollow.rotation * Quaternion.Euler(rotationOffset);
        }
    }
}
