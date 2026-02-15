using System.Collections.Generic;

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
}