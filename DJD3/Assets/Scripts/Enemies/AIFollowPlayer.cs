using UnityEngine;
using UnityEngine.AI;

public class AIFollowPlayer : MonoBehaviour
{
    public GameObject aiObject; // Reference to the AI object
    public float minDistance = 5f; // Minimum distance the AI should stay from the player
    public float maxDistance = 10f; // Maximum distance the AI will follow the player
    private NavMeshAgent agent;
    private Transform player;

    void Start()
    {
        agent = aiObject.GetComponent<NavMeshAgent>(); // Get the NavMeshAgent component from the aiObject
        player = GameObject.FindGameObjectWithTag("Player").transform; // Find the player using its tag
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(aiObject.transform.position, player.position); // Distance from the AI to the player
        
        // If the player is farther than the maximum distance, move towards the player
        if (distanceToPlayer > maxDistance)
        {
            agent.SetDestination(player.position);
        }
        // If the player is closer than the minimum distance, move away from the player
        else if (distanceToPlayer < minDistance)
        {
            Vector3 directionAwayFromPlayer = aiObject.transform.position - player.position;
            Vector3 newPosition = aiObject.transform.position + directionAwayFromPlayer.normalized * (minDistance - distanceToPlayer);
            agent.SetDestination(newPosition);
        }
    }
}
