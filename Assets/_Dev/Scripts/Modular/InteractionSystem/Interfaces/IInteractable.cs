using UnityEngine;

public interface IInteractable
{
    Transform transform { get; }
    string DisplayName { get; }
    bool CanInteract();
    void Interact();
    void OnFocusGained();
    void OnFocusLost();
}
