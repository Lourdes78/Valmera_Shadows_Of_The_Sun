using System.Collections.Generic;
using System.Linq;

public class ReputationSystem
{
    private readonly Dictionary<FactionType, int> reputation =
        new Dictionary<FactionType, int>();

    private readonly EventBus eventBus;

    public ReputationSystem(EventBus bus)
    {
        eventBus = bus;

        foreach (FactionType faction in System.Enum.GetValues(typeof(FactionType)))
        {
            reputation[faction] = 0;
        }
    }

    public int GetReputation(FactionType faction)
    {
        return reputation[faction];
    }

    public void ModifyReputation(FactionType faction, int amount)
    {
        reputation[faction] += amount;

        eventBus.Publish(new ReputationChangedEvent
        {
            Faction = faction,
            NewValue = reputation[faction]
        });
    }
    public Dictionary<FactionType, int> GetAll()
    {
        return new Dictionary<FactionType, int>(reputation);
    }
   
    public void SetReputation(FactionType faction, int value)
    {
        reputation[faction] = value;
    }

    public void ClearAll()
    {
        foreach (var key in reputation.Keys.ToList())
            reputation[key] = 0;
    }

}