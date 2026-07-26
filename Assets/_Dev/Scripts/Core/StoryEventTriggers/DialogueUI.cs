using UnityEngine;
using System.Collections;
using TMPro;

public class DialogueUI : MonoBehaviour
{
    public static DialogueUI Instance;

    [Header("UI")]
    public TextMeshProUGUI dialogueText;
    [SerializeField] private CanvasGroup canvasGroup;

    [Header("Settings")]
    [SerializeField] private float delayBeforeFade = 0.5f;
    [SerializeField] private float fadeSpeed = 4f;
    private Coroutine dialogueRoutine;

    private void Awake() 
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
    }
    public void ShowDialogue(string message, float duration)
    {
        if (dialogueRoutine != null) StopCoroutine(dialogueRoutine);
    
        dialogueRoutine = StartCoroutine(DialogueRoutine(message, duration));
    }
    private IEnumerator DialogueRoutine(string message, float duration)
    {
        canvasGroup.blocksRaycasts = true;
        dialogueText.text = message;

        yield return new WaitForSeconds(delayBeforeFade);
        yield return StartCoroutine(Fade(1f));
        yield return new WaitForSeconds(duration);
        yield return StartCoroutine(Fade(0f));
        
        canvasGroup.blocksRaycasts = false;
        dialogueRoutine = null;
    }
    private IEnumerator Fade(float target)
    {
        while (!Mathf.Approximately(canvasGroup.alpha, target))
        {
            canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, target, fadeSpeed * Time.deltaTime);

            yield return null;
        }
        canvasGroup.alpha = target;
    }
}
