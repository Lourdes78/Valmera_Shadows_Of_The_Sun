using System.Collections.Generic;
using System.Linq;

public class ReputationSystem
{
    private readonly Dictionary<NPCFaction, int> reputation =
        new Dictionary<NPCFaction, int>();

    private readonly EventBus eventBus;

    public ReputationSystem(EventBus bus)
    {
        eventBus = bus;

        foreach (NPCFaction faction in System.Enum.GetValues(typeof(NPCFaction)))
        {
            reputation[faction] = 0;
        }
    }

    public int GetReputation(NPCFaction faction)
    {
        return reputation[faction];
    }

    public void ModifyReputation(NPCFaction faction, int amount)
    {
        reputation[faction] += amount;

        eventBus.Publish(new ReputationChangedEvent
        {
            Faction = faction,
            NewValue = reputation[faction]
        });
    }
    public Dictionary<NPCFaction, int> GetAll()
    {
        return new Dictionary<NPCFaction, int>(reputation);
    }
   
    public void SetReputation(NPCFaction faction, int value)
    {
        reputation[faction] = value;
    }

    public void ClearAll()
    {
        foreach (var key in reputation.Keys.ToList())
            reputation[key] = 0;
    }

}