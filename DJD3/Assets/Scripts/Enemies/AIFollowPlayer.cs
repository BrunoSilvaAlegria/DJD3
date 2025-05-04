using UnityEngine;
using UnityEngine.AI;

public class AIFollowTarget : MonoBehaviour
{
    public string targetTag = "Target";   // The tag of the target object
    public float stopDistance = 5f;       // Distance at which the AI will stop
    public float speed = 3f;              // Speed of the AI's movement

    private NavMeshAgent agent;
    private Transform target;

    void Start()
    {
        // Get the NavMeshAgent component
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;

        // Find the target by its tag
        target = GameObject.FindGameObjectWithTag(targetTag)?.transform;
        
        if (target == null)
        {
            Debug.LogError("Target with tag '" + targetTag + "' not found in the scene.");
        }
    }

    void Update()
    {
        // Ensure the target has been found
        if (target != null)
        {
            // Calculate the distance between the AI and the target
            float distanceToTarget = Vector3.Distance(transform.position, target.position);

            // If the distance is greater than the stop distance, move towards the target
            if (distanceToTarget > stopDistance)
            {
                agent.SetDestination(target.position);
            }
            else
            {
                // Stop the agent if within the stop distance
                agent.SetDestination(transform.position);
            }
        }
    }
}
