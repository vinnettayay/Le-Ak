using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.Rendering;
using Unity.VisualScripting;

[RequireComponent(typeof(Collider))]
public class Inventory : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InventoryUI ui;
    [SerializeField] private AudioSource audioSource;

    [Header("Lantern")]
    [SerializeField] private LanternController lantern;

    [Header("Item")]
    [SerializeField] private Item matchItem;

    [Header("Prefabs")]
    [SerializeField] private GameObject droppedItemPrefab;

    [Header("AudioClips")]
    [SerializeField] private AudioClip pickUpItemAudio;
    [SerializeField] private AudioClip dropItemAudio;

    [Header("State")]
    [SerializeField] private SerializedDictionary<string, Item> inventory = new();

    private void AddItem(Item item)
    {
        var inventoryId = Guid.NewGuid().ToString();
        inventory.Add(inventoryId, item);
        ui.AddUIItem(inventoryId, item);
    }
    public void Pickup(Item item)
    {
        var inventoryId = Guid.NewGuid().ToString();
        inventory.Add(inventoryId, item);
        ui.AddUIItem(inventoryId, item);
        audioSource.PlayOneShot(pickUpItemAudio);
    }
    public bool HasItem(Item item)
    {
        foreach (var inventoryItem in inventory.Values)
        {
            if (inventoryItem == item)
            {
                return true;
            }
        }
        return false;
    }
    public void UseItem(string inventoryId)
    {
        Debug.Log("UseItem Called!");
        if (!inventory.ContainsKey(inventoryId)) return;
        
        Item item = inventory[inventoryId];
        if (item == matchItem)
        {
            if (lantern.NeedsRefill)
            {
                lantern.Refill();

                inventory.Remove(inventoryId);
                ui.RemoveUIItem(inventoryId);
                return;
            }
        }
        DropItem(inventoryId);
    }
    public void DropItem(string inventoryId)
    {
        if (!inventory.ContainsKey(inventoryId)) return;
        Item item = inventory[inventoryId];
        var droppedItem = Instantiate(droppedItemPrefab, transform.position, Quaternion.identity).GetComponent<DroppedItem>();

        droppedItem.Initialize(item);
        inventory.Remove(inventoryId);
        ui.RemoveUIItem(inventoryId);
        audioSource.PlayOneShot(dropItemAudio);
    }
    public bool RemoveItem(Item item)
    {
        string removeId = null;

        foreach (var pair in inventory)
        {
            if (pair.Value == item)
            {
                removeId = pair.Key;
                break;
            }
        }

        if (removeId == null) return false;

        inventory.Remove(removeId);
        ui.RemoveUIItem(removeId);
        return true;
    }
}
