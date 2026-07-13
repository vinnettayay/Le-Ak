using UnityEngine;

public interface IHoldInteractable
{
    bool ShouldHold { get; }
    bool CanHold { get; }
    float HoldDuration { get; }
    void HoldCompleted();
}
