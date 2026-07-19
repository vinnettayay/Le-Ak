using UnityEngine;
using UnityEngine.Rendering;

public class InventoryUI : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject uiItemPrefab;

    [Header("References")]
    [SerializeField] private Inventory inventory;
    [SerializeField] private Transform uiInventoryParent;

    [Header("State")]
    [SerializeField] private SerializedDictionary<string, ItemUI> inventoryUI = new();
    public void AddUIItem(string inventoryId, Item item, int amount)
    {
        Debug.Log("AddUIItem called for: " + item.name);
        var itemUI = Instantiate(uiItemPrefab, uiInventoryParent).GetComponent<ItemUI>();
        Debug.Log("Instantiated: " + itemUI.name);
        itemUI.Initialize(inventoryId, item, amount, inventory.UseItem);
        inventoryUI.Add(inventoryId, itemUI);
    }
    public void UpdateAmount(string inventoryId, int amount)
    {
        Debug.Log("Inventory Updated!");
        if (!inventoryUI.ContainsKey(inventoryId)) return;
        inventoryUI[inventoryId].UpdateAmount(amount);
    }
    public void RemoveUIItem(string inventoryId)
    {
        if (!inventoryUI.ContainsKey(inventoryId)) return;
        Destroy(inventoryUI[inventoryId].gameObject);
        inventoryUI.Remove(inventoryId);
    }
}
