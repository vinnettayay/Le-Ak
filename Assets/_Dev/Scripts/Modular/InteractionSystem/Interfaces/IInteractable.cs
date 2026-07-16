using UnityEngine;

public interface IInteractable
{
    Transform transform { get; }
    string DisplayName { get; }
    float InteractionRadius { get; }
    bool CanInteract();
    void Interact();
    void OnFocusGained();
    void OnFocusLost();
}
