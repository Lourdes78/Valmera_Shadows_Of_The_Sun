using UnityEngine;

public class ZoneTrigger : MonoBehaviour
{
    public string zoneId;

    private EventBus eventBus;

    void Start()
    {
        eventBus = FindFirstObjectByType<WorldBrain>().EventBus;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            eventBus.Publish(new PlayerEnteredZoneEvent(zoneId));
        }
    }
}