using UnityEngine;

public class WorldEventRewardSystem
{
    private EventBus eventBus;
    private XPSystem xpSystem;
    private CurrencySystem currencySystem;
    private ReputationSystem reputationSystem;

    public WorldEventRewardSystem(
        EventBus bus,
        XPSystem xp,
        CurrencySystem currency,
        ReputationSystem reputation)
    {
        eventBus = bus;
        xpSystem = xp;
        currencySystem = currency;
        reputationSystem = reputation;

        eventBus.Subscribe<NPCRescuedEvent>(OnNPCRescued);
    }

    private void OnNPCRescued(NPCRescuedEvent e)
    {
        int xp = Random.Range(150, 300);
        int cs = Random.Range(10, 25);
        int rep = 1;

        if (e.WasInfected)
        {
            xp += 100;
            rep += 1;
        }

        xpSystem.AddXP(xp);
        currencySystem.Add(cs);
        reputationSystem.ModifyReputation(NPCFaction.Civil, rep);

        UnityEngine.Debug.Log($"Rescue Reward: XP {xp} | CS {cs} | REP {rep}");
    }
}