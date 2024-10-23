using UnityEngine;
using UnityEngine.AI;

public class AnimalAI : MonoBehaviour{
    public float wanderRadius = 10f;
    public float wanderTimer = 5f;

    private NavMeshAgent agent;
    private float timer;

    void Start(){
        // Get the NavMeshAgent component attached to the animal
        agent = GetComponent<NavMeshAgent>();
        timer = wanderTimer;
    }

    void Update(){
        // Increment timer
        timer += Time.deltaTime;

        // If the timer exceeds the wanderTimer, set a new destination
        if (timer >= wanderTimer) {
            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
            if (agent != null && agent.isOnNavMesh) {
                agent.SetDestination(newPos);
            }
            timer = 0;
        }
        
        Debug.Log(agent.isOnNavMesh);
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask){
        
        Vector3 randDirection = Random.insideUnitSphere * dist;
        randDirection += origin;

        NavMeshHit navHit;
        if (NavMesh.SamplePosition(randDirection, out navHit, dist, layermask))
        {
            return navHit.position;
        }

        return origin; // Return origin if no valid position found
    }
    
    public void FleeFrom(Vector3 attackerPosition)
    {
        Vector3 fleeDirection = (transform.position - attackerPosition).normalized;
        Vector3 fleePosition = transform.position + fleeDirection * wanderRadius;

        NavMeshHit navHit;
        if (NavMesh.SamplePosition(fleePosition, out navHit, wanderRadius, -1))
        {
            agent.SetDestination(navHit.position);
        }
    }

}