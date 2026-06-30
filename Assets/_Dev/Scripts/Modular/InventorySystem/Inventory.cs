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
    public void DropItem(string inventoryId)
    {
        var droppedItem = Instantiate(droppedItemPrefab, transform.position, Quaternion.identity).GetComponent<DroppedItem>();
        var item = inventory.GetValueOrDefault(inventoryId);
        droppedItem.Initialize(item);
        inventory.Remove(inventoryId);
        ui.RemoveUIItem(inventoryId);
        audioSource.PlayOneShot(dropItemAudio);
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
