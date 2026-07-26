using Unity.VisualScripting;
using UnityEngine;

public class Altar : MonoBehaviour, IHoldInteractable
{
    [Header("Particle System")]
    [SerializeField] private ParticleSystem activationParticle;
    [SerializeField] private ParticleSystem activeParticle;
    [SerializeField] private Transform effectSpawnPos;

    [Header("Tutorial")]
    [SerializeField] private bool isTutorialAltar = false;

    [Header("Offering")]
    [SerializeField] private Item requiredItem;
    [SerializeField] private Transform placePoint;

    [Header("Checkpoint")]
    [SerializeField] private Transform respawnPoint;

    [Header("HoldInteraction")]
    [SerializeField] private float holdDuration = 3f;
    public BoxCollider safezoneArea;
    public float HoldDuration => holdDuration;
    public bool ShouldHold => offeringPlaced;
    public bool CanHold => !prayCompleted;

    private Inventory inventory;
    private GameObject spawnedOffering;
    private bool offeringPlaced;
    private bool prayCompleted = false;
    private Collider thisColl;
    private void Start()
    {
        inventory = FindFirstObjectByType<Inventory>();
        safezoneArea.enabled = false; 
        thisColl = GetComponent<Collider>();
        thisColl.enabled = true;
        
        effectSpawnPos = safezoneArea.transform;
    }
    public bool CanPlace()
    {
        if (offeringPlaced) return true;

        return inventory.HasItem(requiredItem);
    }

    public void Place()
    {
        if (offeringPlaced) return;
        if (!inventory.RemoveItem(requiredItem)) return;

        spawnedOffering = Instantiate(requiredItem.prefab, placePoint.position, placePoint.rotation);
        offeringPlaced = true;
        GetComponent<Interactable>().SetDisplayName("Pray");
        FindFirstObjectByType<PlayerInteractor>()?.RefreshPrompt();

        Interactable interactable = spawnedOffering.GetComponent<Interactable>();
        if (interactable != null) interactable.SetIgnoreInteraction(true);
    }
    public void HoldCompleted()
    {
        if (prayCompleted) return;

        prayCompleted = true;
        safezoneArea.enabled = true;
        safezoneArea.tag = "Safezone";

        Transform spawnPoint = effectSpawnPos != null ? effectSpawnPos : transform;
        if (activationParticle != null)
        {
            ParticleSystem activation = Instantiate(activationParticle, spawnPoint.position, Quaternion.Euler(-90f, 0f, 0f));
            Destroy(activation.gameObject, activation.main.duration + activation.main.startLifetime.constantMax + 0.5f);
        }
        if (activeParticle != null)
        {
            Instantiate(activeParticle, spawnPoint.position, Quaternion.Euler(-90f, 0f, 0f));
        }

        if (GameManager.Instance != null) GameManager.Instance.SetCheckpoint(respawnPoint);

        Interactable interactable = GetComponent<Interactable>();
        if (interactable != null) interactable.SetInteractable(false);

        if (isTutorialAltar)
        {
            GameManager.Instance.FinishTutorial();
        }
    }
}
