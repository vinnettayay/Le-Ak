using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float detectedRadius = 3f;

    [Header("Vision")]
    [SerializeField] private float visionDistance = 15f;
    [Range(0, 360)]
    [SerializeField] private float visionAngle = 90f;

    [Header("Memory")]
    [SerializeField] private float memoryDuration = 1.5f;

    [SerializeField] private LayerMask obstacleLayer;

    public bool PlayerVisible;
    public Vector3 PlayerLastPos { get; private set; }
    private PlayerSafezone playerSafezone;

    private void Awake() 
    {
        if (player == null) player = GameObject.FindGameObjectWithTag("Player").transform; 
        playerSafezone = player.GetComponent<PlayerSafezone>();
    }    
    // Update is called once per frame
    void Update()
    {

        if (playerSafezone != null && playerSafezone.InsideSafezone)
        {
            PlayerVisible = false;
            return;
        }

        PlayerVisible = CanSeePlayer();

        if (PlayerVisible) PlayerLastPos = player.position;
    }
    public bool CanSeePlayer()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= detectedRadius) return true;
        if (distance > visionDistance) return false;

        Vector3 direction = (player.position - transform.position).normalized;
        float angle = Vector3.Angle(transform.forward, direction);

        if (angle > visionAngle * 0.5f) return false;

        Vector3 eye = transform.position + Vector3.up * 1.6f;
        Vector3 target = player.position + Vector3.up;

        direction = (target - eye).normalized;
        float rayDistance = Vector3.Distance(eye, target);

        if (Physics.Raycast(eye, direction, rayDistance, obstacleLayer))
        {
            return false;
        }
        return true;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectedRadius);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, visionDistance);

        Vector3 left = Quaternion.Euler(0, -visionAngle / 2f, 0) * transform.forward;
        Vector3 right = Quaternion.Euler(0, visionAngle / 2f, 0) * transform.forward;

        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, left * visionDistance);
        Gizmos.DrawRay(transform.position, right * visionDistance);

        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(PlayerLastPos, 0.3f);
    }
}
