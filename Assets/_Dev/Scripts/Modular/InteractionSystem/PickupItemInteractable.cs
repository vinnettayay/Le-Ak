using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Interactable))]
public class PickupItemInteractable : MonoBehaviour
{
    [Header("Item")]
    [SerializeField] private Item item;

    [Header("Respawn")]
    [SerializeField] private bool respawnAfterPickup = false;
    [SerializeField] private float respawnTime = 240f;
    private Inventory inventory;
    private Collider pickupCollider;
    private Renderer[] renderers;
    private Rigidbody itemRb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inventory = FindFirstObjectByType<Inventory>();

        pickupCollider = GetComponent<Collider>();
        renderers = GetComponentsInChildren<Renderer>();
        itemRb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    public void Pickup()
    {
        inventory.Pickup(item);

        if (respawnAfterPickup) StartCoroutine(RespawnRoutine());
        else Destroy(gameObject);
    }
    private IEnumerator RespawnRoutine()
    {
        pickupCollider.enabled = false;
        foreach (Renderer renderer in renderers)
        {
            renderer.enabled = false;  
        } 
        if (itemRb != null)
        {
            itemRb.linearVelocity = Vector3.zero;
            itemRb.angularVelocity = Vector3.zero;
            itemRb.isKinematic = true;
        }
        
        yield return new WaitForSeconds(respawnTime);
        foreach (Renderer renderer in renderers)
        {
            renderer.enabled = true;
        }
        pickupCollider.enabled = true;

        if (itemRb != null)
        {
            itemRb.isKinematic = false;
        }
    }
}
