using UnityEngine;

public class PlayerSafezone : MonoBehaviour
{
    public bool InsideSafezone;

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Safezone"))
        {
            InsideSafezone = true;

            EnemyAIBehaviour enemy = FindFirstObjectByType<EnemyAIBehaviour>();
            enemy.EnterSafeZone(transform.position);
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Safezone"))
        {
            InsideSafezone = true;

            EnemyAIBehaviour enemy = FindFirstObjectByType<EnemyAIBehaviour>();
            enemy.EnterSafeZone(transform.position);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Safezone")) InsideSafezone = false;
    }
}
