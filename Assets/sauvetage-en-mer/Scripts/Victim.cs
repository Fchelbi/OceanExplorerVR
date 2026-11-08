using UnityEngine;
using UnityEngine.AI;

public enum VictimState { Drowning, OnBuoy, Pulled, OnBoard, Safe }

public class Victim : MonoBehaviour
{
    public VictimState state = VictimState.Drowning;
    public float bobHeight = 0.1f;
    public float bobSpeed = 2f;
    public float pullSpeed = 5f;

    float baseY;
    float phase;
    RescueBuoy buoy;
    RescueTool tool;
    NavMeshAgent agent;
    Collider col;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        col = GetComponent<Collider>();
        phase = Random.Range(0f, 6.28f);
    }

    void Start()
    {
        baseY = transform.position.y;
    }

    void Update()
    {
        if (state == VictimState.Drowning || state == VictimState.OnBuoy)
        {
            Vector3 p = transform.position;
            p.y = baseY + Mathf.Sin(Time.time * bobSpeed + phase) * bobHeight;
            transform.position = p;
        }
        else if (state == VictimState.Pulled)
        {
            transform.position = Vector3.MoveTowards(transform.position, tool.seat.position, pullSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, tool.seat.position) < 0.2f)
            {
                state = VictimState.OnBoard;
                tool.Board(this);
            }
        }

        if (buoy != null && (state == VictimState.OnBuoy || state == VictimState.Pulled))
            buoy.transform.position = transform.position;
    }

    public void GrabBuoy(RescueBuoy b)
    {
        buoy = b;
        state = VictimState.OnBuoy;
    }

    public void StartPull(RescueTool t)
    {
        tool = t;
        col.enabled = false;
        state = VictimState.Pulled;
    }

    public void Disembark(Vector3 landPos, Vector3 destination)
    {
        state = VictimState.Safe;
        transform.SetParent(null);
        transform.position = landPos;
        col.enabled = true;

        agent.enabled = true;
        agent.Warp(landPos);
        agent.SetDestination(destination);

        Destroy(gameObject, 10f);
    }
}