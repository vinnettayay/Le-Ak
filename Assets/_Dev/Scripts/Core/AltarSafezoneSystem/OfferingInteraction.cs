using UnityEngine;

public class OfferingInteraction : MonoBehaviour, IHoldInteractable
{
    private Altar altar;
    public bool ShouldHold => true;
    public bool CanHold => true;
    public float HoldDuration => altar.HoldDuration;

    public void Initialize(Altar altar)
    {
        this.altar = altar;
    }
    public void HoldCompleted()
    {
        altar.HoldCompleted();
    }
    
}
