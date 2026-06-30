using UnityEngine;
using System;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ItemUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Image image;
    [SerializeField] private Button button;

    public void Initialize(string inventoryId, Item item, Action<string> removeItemAction)
    {
        image.sprite = item.icon;
        transform.localScale = Vector3.one;
        button.onClick.AddListener(() => removeItemAction.Invoke(inventoryId));
    }
    private void OnDestroy()
    {
        button.onClick.RemoveAllListeners();
    }
}
