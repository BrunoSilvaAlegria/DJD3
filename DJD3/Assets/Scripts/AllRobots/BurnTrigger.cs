using System.Collections.Generic;
using UnityEngine;

public class BurnTrigger : MonoBehaviour
{
    [Tooltip("The layer assigned to robot objects (set in Unity > Layers)")]
    public LayerMask robotLayer;

    [Tooltip("Time (in seconds) a robot must stay in the trigger before burning starts")]
    public float burnDelay = 3f;

    // Tracks how long each robot has been in the trigger
    private Dictionary<Collider, float> robotsInTrigger = new Dictionary<Collider, float>();

    private void OnTriggerStay(Collider other)
    {
        // Check if the object is in the Robot layer
        if (((1 << other.gameObject.layer) & robotLayer) != 0)
        {
            if (!robotsInTrigger.ContainsKey(other))
            {
                robotsInTrigger[other] = 0f;
            }

            robotsInTrigger[other] += Time.deltaTime;

            if (robotsInTrigger[other] >= burnDelay)
            {
                RobotStatus robotStatus = other.GetComponent<RobotStatus>();

                if (robotStatus != null)
                {
                    robotStatus.StartOrResetBurn();
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Remove robot from tracking when it leaves the trigger
        if (robotsInTrigger.ContainsKey(other))
        {
            robotsInTrigger.Remove(other);
        }
    }
}
