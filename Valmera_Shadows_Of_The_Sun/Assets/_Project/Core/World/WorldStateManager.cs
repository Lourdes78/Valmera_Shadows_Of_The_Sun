using System.Collections.Generic;

public class WorldStateManager
{
    private Dictionary<string, ZoneRuntimeState> zones =
        new Dictionary<string, ZoneRuntimeState>();
    private EventBus eventBus;

    public void Initialize(List<ZoneData> zoneDefinitions, EventBus eventBus)
    {
        zones.Clear();
        this.eventBus = eventBus;

        foreach (var zone in zoneDefinitions)
        {
            zones.Add(zone.ZoneId, new ZoneRuntimeState(zone, eventBus));
        }
    }

    public IEnumerable<ZoneRuntimeState> GetAllZones()
    {
        return zones.Values;
    }

    public ZoneRuntimeState GetZone(string id)
    {
        zones.TryGetValue(id, out var zone);
        return zone;
    }
}