using UnityEngine;
using System.Linq;
using System.Collections;

public class PlayerState
{
    public XPSystem XPSystem { get; private set; }
    public PlayerStats Stats { get; private set; }

    public ReputationSystem ReputationSystem { get; private set; }
    public InventorySystem InventorySystem { get; private set; }
    public CraftingSystem CraftingSystem { get; private set; }

    public int Influence { get; private set; }

    private EventBus eventBus;

    public PlayerState(EventBus bus, MonoBehaviour runner)
    {
        eventBus = bus;

        XPSystem = new XPSystem(1, bus);
        ReputationSystem = new ReputationSystem(bus);
        InventorySystem = new InventorySystem(20, bus);
        CraftingSystem = new CraftingSystem(InventorySystem, bus, runner);

        Stats = new PlayerStats();   // <-- AQUESTA LÍNIA FALTA

        Influence = 0;
    }

    public void AddInfluence(int amount)
    {
        Influence += amount;
    }

    public void ApplyLoadedData(SaveData data)
    {
        if (data == null || data.Player == null)
        {
            Debug.LogWarning("No save data to apply.");
            return;
        }

        var playerData = data.Player;

        // LEVEL & XP
        XPSystem.SetLevel(playerData.Level);

        XPSystem.SetXP(playerData.CurrentXP);

        // STATS
        Stats.SetHealth(playerData.Stats.CurrentHealth);
        Stats.SetMaxHealth(playerData.Stats.MaxHealth);
        Stats.SetStamina(playerData.Stats.CurrentStamina);
        Stats.SetMaxStamina(playerData.Stats.MaxStamina);

        // REPUTATION
        ReputationSystem.ClearAll();

        for (int i = 0; i < playerData.Reputation.Factions.Length; i++)
        {
            var faction = (FactionType)System.Enum.Parse(
                typeof(FactionType),
                playerData.Reputation.Factions[i]);

            ReputationSystem.SetReputation(
                faction,
                playerData.Reputation.Values[i]);
        }

        Debug.Log("PlayerState applied loaded data.");
    }

}