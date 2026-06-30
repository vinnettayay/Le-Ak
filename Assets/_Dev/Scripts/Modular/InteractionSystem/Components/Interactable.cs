using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour, IInteractable
{
    [SerializeField] private string displayName = "Interact";
    [SerializeField] private bool isEnabled = true;
    [SerializeField] private UnityEvent onInteract;
    public string DisplayName => displayName;
    //public bool CanInteract() => isEnabled;

    private Outline outline;


    //AltarInteraction
    private Altar altar;
    private void Awake() 
    {
        outline = gameObject.AddComponent<Outline>();
        outline.OutlineMode = Outline.Mode.OutlineVisible;
        outline.OutlineColor = Color.yellow;
        outline.OutlineWidth = 15f;
        outline.enabled = false;

        altar = GetComponent<Altar>();
    }
    public bool CanInteract()
    {
        if (!isEnabled) return false;
        if (altar != null) return altar.CanPlace();

        return true;
    }
    public void Interact()
    {
        onInteract?.Invoke();
    }
    public void OnFocusGained()
    {
        if (outline != null) outline.enabled = true;
    }
    public void OnFocusLost()
    {
        if (outline != null) outline.enabled = false;
    }
}
