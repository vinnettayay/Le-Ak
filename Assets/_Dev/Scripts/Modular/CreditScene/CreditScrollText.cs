using UnityEngine;
using TMPro;

public class CreditScrollText : MonoBehaviour
{
    [Header("Scroll")]
    [SerializeField] private RectTransform creditTransform;
    [SerializeField] private float scrollSpeed = 40f;
    [SerializeField] private float endPos;
    [SerializeField] private string nextScene = "";
    [SerializeField] private SceneManagement sceneManagement;
    
    private void Update()
    {
        creditTransform.anchoredPosition += Vector2.up * scrollSpeed * Time.deltaTime;

        if (creditTransform.anchoredPosition.y >= endPos)
        {
            if (!string.IsNullOrEmpty(nextScene))
            {
                sceneManagement.LoadAnyScene(nextScene);
            }
        }

        if (Input.anyKeyDown)
        {
            sceneManagement.LoadAnyScene(nextScene);
        }
    }
}
