using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class DroppedItem : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private bool autoStart;
    [SerializeField] private float enabledPickupDelay = 3.0f;

    [Header("State")]
    public Item item;
    public bool pickedUp;

    [SerializeField] private Collider itemCollider;

    private void Awake() 
    {
        itemCollider.GetComponent<Collider>();
    }
    private void Start()
    {
        if (autoStart && item != null) Initialize(item);
    }
    public void Initialize(Item item)
    {
        this.item = item;
        var droppedItem = Instantiate(item.prefab, transform);

        droppedItem.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        itemCollider.enabled = false;
        StartCoroutine(EnablePickup(enabledPickupDelay));
    }
    private IEnumerator EnablePickup(float  delay)
    {
        yield return new WaitForSeconds(delay);
        itemCollider.enabled = true;
    }
}
