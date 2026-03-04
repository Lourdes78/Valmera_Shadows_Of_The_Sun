public class NPCCuredEvent
{
    public string NpcId;
    public string ZoneId;
    public NPCFaction Faction;

    public NPCCuredEvent(string npcId, string zoneId, NPCFaction faction)
    {
        NpcId = npcId;
        ZoneId = zoneId;
        Faction = faction;
    }
}