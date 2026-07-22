using UnityEngine;

[RequireComponent(typeof(Collider))]
public class StoryTrigger : MonoBehaviour
{
    [Header("StoryEvent")]
    public StoryEvent storyEvent;

    [Header("Settings")]
    public bool triggerOnce = true;
    private bool triggered;

    private void Reset()
    {
        GetComponent<Collider>().isTrigger = true;
    }
    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (triggered && triggerOnce) return;

        triggered = true;
        StoryManager.Instance.TriggerEvent(storyEvent);
    }
}
