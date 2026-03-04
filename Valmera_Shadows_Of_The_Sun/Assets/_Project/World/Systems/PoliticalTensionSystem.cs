using UnityEngine;
using System.Collections.Generic;

public class PoliticalTensionSystem
{
    private readonly WorldStateManager worldState;

    private const float politicalImpactFactor = 0.001f;
    private Dictionary<NPCFaction, int> currentReputations
    = new Dictionary<NPCFaction, int>();

    public PoliticalTensionSystem(WorldStateManager worldState)
    {
        this.worldState = worldState;
    }

    public void HandleReputationChange(NPCFaction faction, int newValue)
    {
        currentReputations[faction] = newValue;
        Debug.Log($"Political system stored reputation for {faction}: {newValue}");
    }
    public void ApplyTension()
    {
        foreach (var zone in worldState.GetAllZones())
        {
            foreach (var rep in currentReputations)
            {
                float alertIncrease = 0f;

                if (rep.Value < 30)
                    alertIncrease += 0.01f;

                if (rep.Value < 10)
                    alertIncrease += 0.02f;

                if (alertIncrease > 0f)
                    zone.ModifyAlert(alertIncrease);
            }
        }
    }

}