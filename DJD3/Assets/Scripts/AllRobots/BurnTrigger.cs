using UnityEngine;

public class BurnTrigger : MonoBehaviour
{
    [Tooltip("The layer assigned to robot objects (set in Unity > Layers)")]
    public LayerMask robotLayer;

    private void OnTriggerStay(Collider other)
    {
        // Check if the object is in the Robot layer
        if (((1 << other.gameObject.layer) & robotLayer) != 0)
        {
            RobotStatus robotStatus = other.GetComponent<RobotStatus>();

            if (robotStatus != null)
            {
                robotStatus.StartOrResetBurn();
            }
        }
    }
}
