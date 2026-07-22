using UnityEngine;

[CreateAssetMenu(fileName = "StoryEvent", menuName = "Story/Story Event")]
public class StoryEvent : ScriptableObject
{
    [Header("Info")]
    public string eventName;

    [TextArea(2, 5)]
    public string description;

    [Header("Requirements")]
    [Tooltip("All of these events must already be completed before this event can trigger.")]
    public StoryEvent[] requiredEvents;

    [Header("Dialogue")]
    public bool hasDialogue;
    [TextArea(3, 8)]
    public string dialogue;
    public float dialogueDuration = 3f;
    public Color textColor;

    [Header("Next Event Trigger")]
    public StoryEvent nextEvent;
    [Min(0)]
    public float nextEventDelay = 0f;
}
