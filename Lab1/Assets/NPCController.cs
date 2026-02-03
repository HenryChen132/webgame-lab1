using UnityEngine;
using UnityEngine.AI;

public class NPCController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;  
    [SerializeField] private NavMeshAgent agent;

    [Header("Patrol")]
    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private float waitAtPoint = 1f;

    [Header("Chase")]
    [SerializeField] private float detectRange = 6f;
    [SerializeField] private float loseRange = 8f;

    private int patrolIndex = 0;
    private float waitTimer = 0f;
    private bool chasing = false;

    private void Reset()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Awake()
    {
        if (agent == null) agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (player == null || agent == null) return;

        float dist = Vector3.Distance(transform.position, player.position);

      
        if (!chasing && dist <= detectRange) chasing = true;
        if (chasing && dist >= loseRange) chasing = false;

        if (chasing)
        {
       
            agent.isStopped = false;
            agent.SetDestination(player.position);
            return;
        }

  
        Patrol();
    }

    private void Patrol()
    {
        if (patrolPoints == null || patrolPoints.Length == 0)
        {
            agent.isStopped = true;
            return;
        }

        Transform target = patrolPoints[patrolIndex];
        agent.isStopped = false;
        agent.SetDestination(target.position);

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            waitTimer += Time.deltaTime;
            if (waitTimer >= waitAtPoint)
            {
                waitTimer = 0f;
                patrolIndex = (patrolIndex + 1) % patrolPoints.Length;
            }
        }
    }
}
