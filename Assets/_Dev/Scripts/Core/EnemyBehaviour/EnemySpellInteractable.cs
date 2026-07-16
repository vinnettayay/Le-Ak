using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Interactable))]
public class EnemySpellInteractable : MonoBehaviour, IHoldInteractable
{
    [Header("References")]
    [SerializeField] private EnemyAIBehaviour enemyAI;
    [SerializeField] private Interactable interactable;

    [Header("Spell")]
    [SerializeField] private float holdDuration = 4f;
    [SerializeField] private float stunDuration = 3f;
    [SerializeField] private float spellCooldown = 5f;

    private bool cooldown;
    public bool ShouldHold => true;
    public bool CanHold => enemyAI != null && enemyAI.IsChasing && !cooldown;
    public float HoldDuration => holdDuration;

    void Awake()
    {
        if (enemyAI == null) enemyAI = GetComponent<EnemyAIBehaviour>();

        if (interactable == null) interactable = GetComponent<Interactable>();
    }
    // Update is called once per frame
    void Update()
    {
        if (enemyAI == null) return;

        if (enemyAI.IsChasing)
        {
            interactable.SetIgnoreInteraction(false);
            interactable.SetDisplayName("Read Spell");
        }
        else
        {
            interactable.SetIgnoreInteraction(true);
        }
    }
    public void HoldCompleted()
    {
        if (!CanHold) return;

        enemyAI.Stun(stunDuration);
        StartCoroutine(CooldownRoutine());
    }
    private IEnumerator CooldownRoutine()
    {
        cooldown = true;
        interactable.SetDisplayName("Spell Cooling..");
        yield return new WaitForSeconds(spellCooldown);

        cooldown = false;
        if (enemyAI.IsChasing) interactable.SetDisplayName("ReadSpell");
    }
}
