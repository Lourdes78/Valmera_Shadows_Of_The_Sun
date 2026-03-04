using System.Collections.Generic;
using UnityEngine;

public class EconomyManager
{
    private Dictionary<string, float> zoneEconomy = new();

    public EconomyManager()
    {
        // Constructor buit correcte
    }

    public void Initialize(WorldStateManager worldState)
    {
        foreach (var zone in worldState.GetAllZones())
        {
            zoneEconomy[zone.Definition.ZoneId] = 0.5f;
        }
    }

    public void WorldTick()
    {
        // De moment no fa res.
        // Però mantenim el mètode perquè WorldBrain el crida.
    }

    public void ModifyEconomy(string zoneId, float amount)
    {
        if (!zoneEconomy.ContainsKey(zoneId))
            return;

        zoneEconomy[zoneId] = Mathf.Clamp01(
            zoneEconomy[zoneId] + amount
        );
    }

    public float GetEconomy(string zoneId)
    {
        if (zoneEconomy.TryGetValue(zoneId, out var value))
            return value;

        return 0f;
    }
}