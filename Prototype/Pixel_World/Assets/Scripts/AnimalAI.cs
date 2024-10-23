using UnityEngine;
using UnityEngine.AI;

public class AnimalAI : MonoBehaviour{
    public float wanderRadius = 10f;
    public float wanderTimer = 5f;

    private NavMeshAgent agent;
    private float timer;

    void Start(){
        agent = GetComponent<NavMeshAgent>();
        timer = wanderTimer;
    }

    void Update(){
        timer += Time.deltaTime;

        if (timer >= wanderTimer){
            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
            agent.SetDestination(newPos);
            timer = 0;
        }
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask){
        Vector3 randDirection = Random.insideUnitSphere * dist;
        randDirection += origin;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);
        return navHit.position;
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