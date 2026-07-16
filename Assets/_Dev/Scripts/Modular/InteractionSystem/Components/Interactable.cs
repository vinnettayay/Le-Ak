using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour, IInteractable
{
    [Header("Interaction")]
    [SerializeField] private string displayName = "Interact";
    [SerializeField] private float interactionRadius = 2f;
    [SerializeField] private bool isEnabled = true;
    [SerializeField] private bool ignoreInteraction = false;
    [SerializeField] private UnityEvent onInteract;
    //public bool CanInteract() => isEnabled;

    [Header("Outline")]
    private Outline outline;
    [SerializeField] private Color outlineColor = Color.yellow;
    [SerializeField] private float outlineWidth = 15f;

    private Altar altar;

    public string DisplayName => displayName;
    public bool IgnoreInteraction => ignoreInteraction;
    public float InteractionRadius => interactionRadius;
    private void Awake() 
    {
        outline = gameObject.AddComponent<Outline>();
        outline.OutlineMode = Outline.Mode.OutlineVisible;
        outline.OutlineColor = outlineColor;
        outline.OutlineWidth = outlineWidth;
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
    public void SetDisplayName(string text)
    {
        displayName = text;
    }
    public void SetIgnoreInteraction(bool value)
    {
        ignoreInteraction = value;
    }
    public void SetInteractable(bool value)
    {
        isEnabled = value;
    }
    public void SetOutlineColor(Color color)
    {
        outlineColor = color;

        if (outline != null) outline.OutlineColor = color;
    }
    public void SetOutlineWidth(float width)
    {
        outlineWidth = width;

        if (outline != null) outline.OutlineWidth = width;
    }
    public void SetInteractionRadius(float radius)
    {
        interactionRadius = radius;
    }
}
