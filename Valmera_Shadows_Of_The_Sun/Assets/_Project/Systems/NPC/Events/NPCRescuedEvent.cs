public class NPCRescuedEvent
{
    public string NpcId;
    public bool WasInfected;

    public NPCRescuedEvent(string npcId, bool wasInfected)
    {
        NpcId = npcId;
        WasInfected = wasInfected;
    }
}