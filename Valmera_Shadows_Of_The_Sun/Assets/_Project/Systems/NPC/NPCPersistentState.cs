using System;

public class NPCPersistentState
{
    public string RuntimeId { get; }
    public NPCDefinition Definition { get; }
    public string CurrentZoneId { get; private set; }

    public int CurrentHealth { get; private set; }
    public bool IsAlive { get; private set; } = true;

    public NPCCondition Condition { get; private set; } = NPCCondition.Normal;

    public NPCPersistentState(NPCDefinition definition, string zoneId)
    {
        RuntimeId = Guid.NewGuid().ToString();

        Definition = definition;
        CurrentZoneId = zoneId;
        CurrentHealth = definition.baseHealth;

        // Si és zombi, estat inicial correcte
        if (definition.faction == NPCFaction.Zombie)
            Condition = NPCCondition.Zombie;

        // Si pot estar infectat, random
        if (definition.canBeInfected && UnityEngine.Random.value > 0.7f)
            Condition = NPCCondition.Infected;
    }

    public void Kill()
    {
        IsAlive = false;
    }

    public void Cure(EventBus eventBus)
    {
        if (Condition != NPCCondition.Infected)
            return;

        Condition = NPCCondition.Normal;

        eventBus.Publish(new NPCCuredEvent(
            RuntimeId,
            CurrentZoneId,
            Definition.faction
        ));
    }
}