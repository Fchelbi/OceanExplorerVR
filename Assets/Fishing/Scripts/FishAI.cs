using UnityEngine;
using UnityEngine.AI;

public class FishAI : MonoBehaviour
{
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        MoveToRandomPoint();
    }

    void Update()
    {
        
        if (!agent.pathPending && agent.remainingDistance < 1f)
        {
            MoveToRandomPoint();
        }
    }

    void MoveToRandomPoint()
    {
       
        Vector3 randomDirection = Random.insideUnitSphere * 15f; 
        randomDirection += transform.position;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, 15f, 1))
        {
            agent.SetDestination(hit.position);
        }
    }
}