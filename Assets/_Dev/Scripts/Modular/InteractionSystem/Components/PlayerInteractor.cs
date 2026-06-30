using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    [SerializeField] private float radius = 2f;
    [SerializeField] private LayerMask interactableLayers;
    [SerializeField] private InteractPrompt prompt;
    private Collider[] buffer = new Collider[32];
    private IInteractable focused;

    // Update is called once per frame
    void Update()
    {
        IInteractable nearest = FindNearestInteractable();
        UpdateFocus(nearest);

        if (focused != null && Input.GetKeyDown(KeyCode.E))
        {
            if (focused.CanInteract()) focused.Interact();
        }
    }
    private IInteractable FindNearestInteractable()
    {
        int count = Physics.OverlapSphereNonAlloc(transform.position, radius, buffer, interactableLayers, QueryTriggerInteraction.Collide);
        IInteractable nearest = null;
        float bestDistSq = float.MaxValue;

        for (int i = 0; i < count; i++)
        {
            Collider col = buffer[i];
            if (col == null) continue;
            IInteractable interactable = col.GetComponentInParent<IInteractable>();
            if (interactable == null) continue;
            if (!interactable.CanInteract()) continue;

            float distSq = (col.transform.position - transform.position).sqrMagnitude;
            if (distSq < bestDistSq)
            {
                bestDistSq = distSq;
                nearest = interactable;
            }
        }
        return nearest;
    }
    private void UpdateFocus(IInteractable nearest)
    {
        if (ReferenceEquals(focused, nearest)) return;
        focused?.OnFocusLost();
        focused = nearest;
        
        if (focused != null)
        {
            focused.OnFocusGained();
            prompt.Show(focused);
        }
        else
        {
            prompt.Hide();
        }
    }
}
