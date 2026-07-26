using UnityEngine;
using System.Collections;

public class MainGateController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Inventory inventory;
    [SerializeField] private Item statueItem;
    [SerializeField] private Item gateKey;
    [SerializeField] private Animator animator;
    [SerializeField] private Collider gateCollider;
    [SerializeField] private string openTrigger = "Open";

    [Header("Ending")]
    [SerializeField] private string endingScene;
    [SerializeField] private float openDelay = 2f;
    [SerializeField] private float blackfadeDelay = 3f;
    [SerializeField] private SceneManagement sceneManagement;
    private Interactable interactable;
    private bool animationPlayed;
    private bool escaped;
    private void Awake() 
    {
        interactable = GetComponent<Interactable>();    
    }
    void Start()
    {
        StoryManager.Instance.OnGateUnsealed += RefreshGate;
        RefreshGate();
    }

    public void RefreshGate()
    {
        if (!StoryManager.Instance.GateUnsealed)
        {
            interactable.SetDisplayName($"Offer Statue ({StoryManager.Instance.StatuesPlaced}/{StoryManager.Instance.RequiredStatues})");
            return;
        }
        interactable.SetDisplayName("Escape");
    }
    private void DisableCollider()
    {
        if (gateCollider != null) gateCollider.enabled = false;
    }
    public void InteractGate()
    {
        if (escaped) return;
        if (!StoryManager.Instance.GateUnsealed)
        {
            if (!inventory.RemoveItem(statueItem))
            {
                return;
            }
            StoryManager.Instance.AddStatue();
            RefreshGate();
            return;
        }
        if (!inventory.HasItem(gateKey))
        {
            return;
        }    

        inventory.RemoveItem(gateKey);
        escaped = true;

        if (!string.IsNullOrEmpty(endingScene))
        {
            //LoadScene
            StartCoroutine(Escaped());
        }
    }
    private IEnumerator Escaped()
    {
        if (!animationPlayed)
        {
            animationPlayed = true;
            if (animator != null) animator.SetTrigger(openTrigger);
            Invoke(nameof(DisableCollider), openDelay);
        }
        yield return new WaitForSeconds(blackfadeDelay);
        sceneManagement.LoadAnyScene(endingScene);
    }
    private void OnDestroy()
    {
        if (StoryManager.Instance != null) StoryManager.Instance.OnGateUnsealed -= RefreshGate;
    }
}
