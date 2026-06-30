using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering;

public class InventoryUI : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject uiItemPrefab;

    [Header("References")]
    [SerializeField] private Inventory inventory;
    [SerializeField] private Transform uiInventoryParent;

    [Header("State")]
    [Header("SerializeField")]
    SerializedDictionary<string, GameObject> inventoryUI = new();
    public void AddUIItem(string inventoryId, Item item)
    {
        var itemUI = Instantiate(uiItemPrefab).GetComponent<ItemUI>();
        itemUI.transform.SetParent(uiInventoryParent, false);
        inventoryUI.Add(inventoryId, itemUI.gameObject);
        itemUI.Initialize(inventoryId, item, inventory.DropItem);
    }
    public void RemoveUIItem(string inventoryId)
    {
        var itemUI = inventoryUI.GetValueOrDefault(inventoryId);
        inventoryUI.Remove(inventoryId);
        Destroy(itemUI);
    }
}
