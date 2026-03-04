using UnityEngine;

public class NPCKilledEvent
{
    public string NpcId;
    public NPCFaction Faction;
    public string ZoneId;

    public NPCKilledEvent(string npcId, NPCFaction faction, string zoneId)
    {
        NpcId = npcId;
        Faction = faction;
        ZoneId = zoneId;
    }
}