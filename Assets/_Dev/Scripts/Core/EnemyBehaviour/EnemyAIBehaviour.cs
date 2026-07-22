using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyAIBehaviour : MonoBehaviour
{
    public enum EnemyState
    {
        Patrol,
        Chase,
        Search,
        Attack,
        Stunned
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

    [Header("Chase")]
    [SerializeField] private GameObject chaseTriggerUI;

    [Header("Attack")]
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float attackCooldown = 1.5f;

    [Header("Search")]
    [SerializeField] private float searchDuration = 5f;

    [Header("Stunned")]
    [SerializeField] private float stunDuration = 3f;

    [SerializeField]
    private EnemyState enemyState;
    private Vector3 searchPos;
    private Vector3 patrolPoint;
    private bool patrolPointSet;
    private bool alreadyAttacked;
    private bool ignoreDetection;
    private float patrolTimer;
    private float searchTimer;
    private Coroutine stunRoutine;
    private GameManager gameManager;

    public EnemyState CurrentState => enemyState;
    public bool IsChasing => enemyState == EnemyState.Chase || enemyState == EnemyState.Attack;

    private void Awake() 
    {
        if (agent == null) agent = GetComponent<NavMeshAgent>();
        if (detection == null) detection = GetComponent<EnemyDetection>();
        if (player == null) player = GameObject.FindGameObjectWithTag("Player").transform;

        gameManager = FindFirstObjectByType<GameManager>();
    }
    void Start()
    {
        ChangeState(EnemyState.Patrol);
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyState == EnemyState.Stunned) return;

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
                Chase();
                break;

            case EnemyState.Attack : 
                Attack();
                break;

            case EnemyState.Search : 
                
                if (!ignoreDetection && detection.PlayerVisible)
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
        //StartCoroutine(ShowChaseTrigger());  IT KEEP BLINKING FIX!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        agent.speed = chaseSpeed;
        agent.SetDestination(player.position);
    }
    public void TriggerAttack()
    {
        if (enemyState == EnemyState.Attack) return;
        if (enemyState == EnemyState.Stunned) return;
        ChangeState(EnemyState.Attack);
    }
    private void Attack()
    {
        agent.ResetPath();

        Vector3 lookPos = player.position;
        lookPos.y = transform.position.y;
        transform.LookAt(lookPos);

        if (alreadyAttacked) return;
        alreadyAttacked = true;

        if (gameManager != null)
        {
            gameManager.PlayerCaught(this);
        }
        Invoke(nameof(ResetAttack), attackCooldown);
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
        if (enemyState != EnemyState.Chase && enemyState != EnemyState.Attack) return;

        searchPos = lastPos;
        ChangeState(EnemyState.Search);
    }
    public void Stun()
    {
        Stun(stunDuration);
    }
    public void Stun(float duration)
    {
        if (stunRoutine != null) StopCoroutine(stunRoutine);
        
        stunRoutine = StartCoroutine(StunRoutine(duration));
    }
    private IEnumerator StunRoutine(float duration)
    {
        ChangeState(EnemyState.Stunned);

        agent.ResetPath();
        agent.isStopped = true;

        yield return new WaitForSeconds(duration);

        agent.isStopped = false;
        ChangeState(EnemyState.Chase);
        stunRoutine = null;
    }
    public void ResetEnemy(Vector3 position)
    {
        agent.ResetPath();
        agent.isStopped = true;

        agent.Warp(position);

        alreadyAttacked = false;
        patrolPointSet = false;

        searchPos = Vector3.zero;
        agent.isStopped = false;

        ChangeState(EnemyState.Patrol);
        DisableDetection(1f);
    }
    public void DisableDetection(float duration)
    {
        if (gameObject.activeInHierarchy) StartCoroutine(DetectionCooldown(duration));
    }
    private IEnumerator DetectionCooldown(float duration)
    {
        ignoreDetection = true;
        detection.PlayerVisible = false;
        yield return new WaitForSeconds(duration);
        ignoreDetection = false;
    }
    private IEnumerator ShowChaseTrigger()
    {
        chaseTriggerUI.SetActive(true);
        yield return new WaitForSeconds(1f);
        chaseTriggerUI.SetActive(false);
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
