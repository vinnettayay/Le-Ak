using UnityEngine;
using System;
using UnityEngine.Rendering;

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

    [Serializable]
    public class InventoryEntry
    {
        public string inventoryId;
        public Item item;
        public int amount;

        public InventoryEntry(Item item)
        {
            inventoryId = Guid.NewGuid().ToString();
            this.item = item;
            amount = 1;
        }
    }

    [Header("Inventory")]
    [SerializeField] private SerializedDictionary<string, InventoryEntry> inventory = new();

    // private void AddItem(Item item)
    // {
    //     var inventoryId = Guid.NewGuid().ToString();
    //     inventory.Add(inventoryId, item);
    //     ui.AddUIItem(inventoryId, item);
    // }
    public void Pickup(Item item)
    {
        foreach(var pair in inventory)
        {
            if(pair.Value.item == item)
            {
                Debug.Log("Check conditions");
                pair.Value.amount++;
                ui.UpdateAmount(pair.Key, pair.Value.amount);
                audioSource.PlayOneShot(pickUpItemAudio);
                return;
            }
        }
        InventoryEntry entry = new InventoryEntry(item);
        inventory.Add(entry.inventoryId, entry);
        ui.AddUIItem(entry.inventoryId, item, entry.amount);
        audioSource.PlayOneShot(pickUpItemAudio);     
    }
    public bool HasItem(Item item)
    {
        foreach (var pair in inventory)
        {
            if (pair.Value.item == item) return true;
        }
        return false;
    }
    public void UseItem(string inventoryId)
    {
        if (!inventory.ContainsKey(inventoryId)) return;
        
        InventoryEntry entry = inventory[inventoryId];
        if (entry.item == matchItem)
        {
            if (lantern.NeedsRefill)
            {
                lantern.Refill();
                entry.amount--;

                if (entry.amount <= 0)
                {
                    inventory.Remove(inventoryId);
                    ui.RemoveUIItem(inventoryId);   
                }
                else
                {
                    ui.UpdateAmount(inventoryId, entry.amount);
                }
                return;
            }
        }
        DropItem(inventoryId);
    }
    public void DropItem(string inventoryId)
    {
        if (!inventory.ContainsKey(inventoryId)) return;
        InventoryEntry entry = inventory[inventoryId];
        var droppedItem = Instantiate(droppedItemPrefab, transform.position, Quaternion.identity).GetComponent<DroppedItem>();

        droppedItem.Initialize(entry.item);
        entry.amount--;
        if (entry.amount <= 0)
        {
            inventory.Remove(inventoryId);
            ui.RemoveUIItem(inventoryId);   
        }
        else
        {
            ui.UpdateAmount(inventoryId, entry.amount);
        }
        audioSource.PlayOneShot(dropItemAudio);
    }
    public bool RemoveItem(Item item)
    {
        foreach (var pair in inventory)
        {
            if (pair.Value.item == item)
            {
                pair.Value.amount--;
                if (pair.Value.amount <= 0)
                {
                    inventory.Remove(pair.Key);
                    ui.RemoveUIItem(pair.Key);
                }
                else
                {
                    ui.UpdateAmount(pair.Key, pair.Value.amount);
                }
                return true;
            }
        }
        return false;
    }
    public int GetItemAmount(Item item)
    {
        foreach ( var pair in inventory)
        {
            if (pair.Value.item == item) return pair.Value.amount;
        }
        return 0;
    }
}
