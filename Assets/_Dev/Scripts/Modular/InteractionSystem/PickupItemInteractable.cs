using System;
using UnityEngine;

[RequireComponent(typeof(Interactable))]
public class PickupItemInteractable : MonoBehaviour
{
    [SerializeField] private Item item;
    private Inventory inventory;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inventory = FindFirstObjectByType<Inventory>();
    }

    // Update is called once per frame
    public void Pickup()
    {
        inventory.Pickup(item);
        Destroy(gameObject);
    }
}
