using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ItemUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI amountText;
    [SerializeField] private Button button;

    private string inventoryId;
    private Action<string> useItemAction;
    public void Initialize(string inventoryId, Item item, int amount, Action<string> useItemAction)
    {
        this.inventoryId = inventoryId;
        this.useItemAction = useItemAction;

        image.sprite = item.icon;
        UpdateAmount(amount);
        transform.localScale = Vector3.one;

        button.onClick.RemoveAllListeners();    
        button.onClick.AddListener(OnClick);
    }
    private void OnClick()
    {
        useItemAction?.Invoke(inventoryId);
    }
    public void UpdateAmount(int amount)
    {
        if (amountText == null) return;
        amountText.text = amount > 1 ? $"{amount}" : "";
    }
    private void OnDestroy()
    {
        button.onClick.RemoveAllListeners();
    }
}
