using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    [SerializeField] private HoldInteractUI holdUI;
    [SerializeField] private float radius = 2f;
    [SerializeField] private LayerMask interactableLayers;
    [SerializeField] private InteractPrompt prompt;
    private Collider[] buffer = new Collider[32];
    private IInteractable focused;
    private IHoldInteractable focusedHold;
    private float holdTimer;

    // Update is called once per frame
    void Update()
    {
        IInteractable nearest = FindNearestInteractable();
        UpdateFocus(nearest);

        if (focused == null)
        {
            holdTimer = 0;
            holdUI.Hide();
            return;
        }

        if (focusedHold != null && focusedHold.ShouldHold && focusedHold.CanHold)
        {
            if (Input.GetKey(KeyCode.E))
            {
                holdTimer += Time.deltaTime;

                holdUI.Show();
                holdUI.SetProgress(holdTimer / focusedHold.HoldDuration);

                if (holdTimer >= focusedHold.HoldDuration)
                {
                    focusedHold.HoldCompleted();
                    holdTimer = 0;
                    holdUI.Hide();
                }
            }
            else
            {
                holdTimer = 0;
                holdUI.Hide();   
            }
            return;
        }
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
            //IInteractable interactable = col.GetComponentInParent<IInteractable>();

            Interactable interactableComponent = col.GetComponent<Interactable>();
            if (interactableComponent != null && interactableComponent.IgnoreInteraction) continue;
            IInteractable interactable = interactableComponent;

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
        
        holdTimer = 0;

        if (focused != null)
        {
            focused.OnFocusGained();
            prompt.Show(focused);

            focusedHold = (focused as MonoBehaviour)?.GetComponent<IHoldInteractable>();
        }
        else
        {
            focusedHold = null;
            prompt.Hide();
            holdUI.Hide();
        }
    }
    public void RefreshPrompt()
    {
        if (focused != null)
        {
            prompt.Show(focused);
        }
    }
}
