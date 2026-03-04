using UnityEngine;

public class NPCBehaviour : MonoBehaviour
{
    public string npcId;
    public NPCFaction faction;
    public string zoneId;

    private EventBus eventBus;

    void Start()
    {
        eventBus = FindFirstObjectByType<WorldBrain>().EventBus;
    }

    public void Kill()
    {
        eventBus.Publish(new NPCKilledEvent(npcId, faction, zoneId));
        Destroy(gameObject);
    }

    public void Cure()
    {
        eventBus.Publish(new NPCCuredEvent(npcId, zoneId, faction));
    }
}