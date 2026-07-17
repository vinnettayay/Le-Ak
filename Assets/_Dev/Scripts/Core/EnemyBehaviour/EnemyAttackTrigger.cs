using UnityEngine;

public class EnemyAttackTrigger : MonoBehaviour
{
    [SerializeField] private EnemyAIBehaviour enemy;
    
    private void Awake() 
    {
        if (enemy == null) enemy = GetComponentInParent<EnemyAIBehaviour>();    
    }
    private void OnTriggerEnter(Collider other) 
    {
        if (!other.CompareTag("Player")) return;
        enemy.TriggerAttack();    
    }
}
