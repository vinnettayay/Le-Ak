using UnityEngine;
using UnityEngine.AI;

public class EnemyAIBehaviour : MonoBehaviour
{
    public enum EnemyState
    {
        Patrol,
        Chase,
        Search,
        Attack
    }

    [Header("References")]
    [SerializeField] private EnemyDetection detection;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Transform player;

    [Header("Movement")]
    [SerializeField] private float patrolSpeed = 3.5f;
    [SerializeField] private float chaseSpeed = 5.5f;
    
    [Header("Patrol")]
    [SerializeField] private float patrolRadius = 15f;
    [SerializeField] private float patrolDelayTime = 2f;

    [Header("Attack")]
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float attackCooldown = 1.5f;

    [Header("Search")]
    [SerializeField] private float searchDuration = 5f;

    [SerializeField]
    private EnemyState enemyState;
    private Vector3 searchPos;
    private Vector3 patrolPoint;
    private bool patrolPointSet;
    private bool alreadyAttacked;
    private float patrolTimer;
    private float searchTimer;

    public EnemyState CurrentState => enemyState;

    private void Awake() 
    {
        if (agent == null) agent = GetComponent<NavMeshAgent>();
        if (detection == null) detection = GetComponent<EnemyDetection>();
        if (player == null) player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    void Start()
    {
        ChangeState(EnemyState.Patrol);
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        switch (enemyState)
        {
            case EnemyState.Patrol : 
                if (detection.PlayerVisible)
                {
                    ChangeState(EnemyState.Chase);
                    return;
                }
                Patrol();
                break;

            case EnemyState.Chase : 
                if (distance <= attackRange)
                {
                    ChangeState(EnemyState.Attack);
                    return;
                }
                Chase();
                break;

            case EnemyState.Attack : 
                if (distance > attackRange)
                {
                    ChangeState(EnemyState.Chase);
                    return;
                }
                Attack();
                break;

            case EnemyState.Search : 
                
                if (detection.PlayerVisible)
                {
                    ChangeState(EnemyState.Chase);
                    return;
                }
                Search();
                break;
        }
    }
    private void ChangeState(EnemyState newState)
    {
        enemyState = newState;

        switch (newState)
        {
            case EnemyState.Patrol : 
                patrolPointSet = false;
                patrolTimer = 0f;
                break;
            case EnemyState.Search : 
                searchTimer = 0f;
                break;
        }
        Debug.Log("CurrentState : " + enemyState);
    }
    private void Patrol()
    {
        agent.speed = patrolSpeed;

        if (!patrolPointSet)
        {
            patrolTimer += Time.deltaTime;
            if (patrolTimer >= patrolDelayTime)
            {
                patrolTimer = 0f;
                SearchPatrolPoint();
            }
            return;
        } 
        agent.SetDestination(patrolPoint);
        
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance) patrolPointSet = false;
    }
    private void SearchPatrolPoint()
    {
        Vector3 randomDirection = Random.insideUnitSphere * patrolRadius;

        randomDirection += transform.position;
        randomDirection.y = transform.position.y;

        if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, patrolRadius, NavMesh.AllAreas))
        {
            patrolPoint = hit.position;
            patrolPointSet = true;
        }
    }
    private void Chase()
    {
        agent.speed = chaseSpeed;
        agent.SetDestination(player.position);
    }
    private void Attack()
    {
        agent.SetDestination(transform.position);

        Vector3 lookPos = player.position;
        lookPos.y = transform.position.y;
        transform.LookAt(lookPos);

        if (!alreadyAttacked)
        {
            Debug.Log("Attack!");
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), attackCooldown);
        }
    }
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }
    private void Search()
    {
        agent.speed = patrolSpeed;
        agent.SetDestination(searchPos);

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            searchTimer += Time.deltaTime;
            if (searchTimer >= searchDuration) ChangeState(EnemyState.Patrol);
        }
    }
    public void EnterSafeZone(Vector3 lastPos)
    {
        Debug.Log("CurrentState : " + enemyState);
        Debug.Log("Hasil : " + (enemyState != EnemyState.Chase && enemyState != EnemyState.Attack));
        
        if (enemyState != EnemyState.Chase && enemyState != EnemyState.Attack) return;

        Debug.Log("ChangeState TO Search");
        searchPos = lastPos;
        ChangeState(EnemyState.Search);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        if (patrolPointSet)
        {
            Gizmos.DrawSphere(patrolPoint, 0.3f);
            Gizmos.DrawLine(transform.position, patrolPoint);
        }
    }
}
