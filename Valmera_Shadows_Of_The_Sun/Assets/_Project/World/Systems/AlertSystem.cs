public class AlertSystem
{
    private readonly WorldStateManager worldState;

    public AlertSystem(WorldStateManager worldState)
    {
        this.worldState = worldState;
    }

    public void GlobalDecay(float amount)
    {
        foreach (var zone in worldState.GetAllZones())
        {
            zone.ModifyAlert(-amount);
        }
    }

    public void ReduceAlertOnLevelUp()
    {
        foreach (var zone in worldState.GetAllZones())
        {
            zone.ModifyAlert(-0.02f);
        }
    }

    public void IncreaseAlert(string zoneId, float amount)
    {
        var zone = worldState.GetZone(zoneId);
        zone?.ModifyAlert(amount);
    }
}