using UnityEngine;

public class Altar : MonoBehaviour
{
    [SerializeField] private Item requiredItem;
    [SerializeField] private Transform placePoint;

    private Inventory inventory;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        inventory = FindFirstObjectByType<Inventory>();   
    }
    public bool CanPlace()
    {
        return inventory.HasItem(requiredItem);
    }

    public void Place()
    {
        if (!inventory.RemoveItem(requiredItem)) return;

        Instantiate(requiredItem.prefab, placePoint.position, placePoint.rotation);
    }
}
