using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using TMPro;

public class StoryManager : MonoBehaviour
{
    public static StoryManager Instance;
    private HashSet<StoryEvent> completedEvents = new HashSet<StoryEvent>();
    [SerializeField] private TextMeshProUGUI objectiveText;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public bool HasTriggered(StoryEvent storyEvent)
    {
        return completedEvents.Contains(storyEvent);
    }
    public void TriggerEvent(StoryEvent storyEvent)
    {   
        if (storyEvent == null) return;
        if (completedEvents.Contains(storyEvent)) return;
        if (!RequirementsMet(storyEvent)) return;

        completedEvents.Add(storyEvent);

        if (storyEvent.hasDialogue)
        {
            DialogueUI.Instance.dialogueText.color = storyEvent.textColor;
            DialogueUI.Instance.ShowDialogue(storyEvent.dialogue, storyEvent.dialogueDuration);
        }
        SetObjectiveUI(storyEvent.eventName);
        Debug.Log("Story Event Triggered : " + storyEvent.eventName);
        OnStoryEventTriggered?.Invoke(storyEvent);

        if (storyEvent.nextEvent != null) StartCoroutine(TriggerNextEvent(storyEvent.nextEvent, storyEvent.nextEventDelay));
    }
    public delegate void StoryEventDelegate(StoryEvent storyEvent);
    public event StoryEventDelegate OnStoryEventTriggered;
    private void SetObjectiveUI(string objective)
    {
        objectiveText.text = objective;
    }
    private bool RequirementsMet(StoryEvent storyEvent)
    {
        if (storyEvent.requiredEvents == null || storyEvent.requiredEvents.Length == 0) return true;

        foreach (StoryEvent required in storyEvent.requiredEvents)
        {
            if (required == null) continue;
            if (!completedEvents.Contains(required)) return false;
        }
        return true;
    }
    private IEnumerator TriggerNextEvent(StoryEvent nextEvent, float delay)
    {
        yield return new WaitForSeconds(delay);
        TriggerEvent(nextEvent);
    }
}
