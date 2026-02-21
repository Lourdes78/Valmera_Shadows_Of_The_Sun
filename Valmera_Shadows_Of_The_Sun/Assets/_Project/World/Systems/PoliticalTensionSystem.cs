using UnityEngine;

public class PoliticalTensionSystem
{
    private readonly WorldStateManager worldState;

    private const float politicalImpactFactor = 0.001f;

    public PoliticalTensionSystem(WorldStateManager worldState)
    {
        this.worldState = worldState;
    }

    public void HandleReputationChange(FactionType faction, int newValue)
    {
        foreach (var zone in worldState.GetAllZones())
        {
            float impact = -newValue * zone.Definition.PoliticalWeight * politicalImpactFactor;

            zone.ModifyAlert(impact);
        }

        Debug.Log($"Political system applied tension from faction {faction}");
    }
}