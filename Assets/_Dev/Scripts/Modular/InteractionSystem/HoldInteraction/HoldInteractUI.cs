using UnityEngine;
using UnityEngine.UI;

public class HoldInteractUI : MonoBehaviour
{
    [SerializeField] private Image fillImage;
    public float duration = 3f;
    private void Awake() 
    {
        Hide();
    }
    public void SetProgress(float value)
    {
        fillImage.fillAmount = Mathf.Clamp01(value);
    }
    public void Show()
    {
        gameObject.SetActive(true);
    }
    public void Hide()
    {
        fillImage.fillAmount = 0;
        gameObject.SetActive(false);
    }
}
