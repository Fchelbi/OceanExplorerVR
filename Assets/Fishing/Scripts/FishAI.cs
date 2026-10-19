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
        // Si le poisson est presque arrivé à destination, il en choisit une nouvelle
        if (!agent.pathPending && agent.remainingDistance < 1f)
        {
            MoveToRandomPoint();
        }
    }

    void MoveToRandomPoint()
    {
        // Trouve un point aléatoire sur la zone bleue (NavMesh)
        Vector3 randomDirection = Random.insideUnitSphere * 15f; // 15f = rayon de recherche
        randomDirection += transform.position;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, 15f, 1))
        {
            agent.SetDestination(hit.position);
        }
    }
}