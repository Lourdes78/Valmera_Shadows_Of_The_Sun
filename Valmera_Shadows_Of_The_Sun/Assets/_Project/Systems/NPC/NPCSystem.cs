using System.Collections.Generic;
using UnityEngine;

public class NPCSystem
{
    private Dictionary<string, NPCPersistentState> npcs =
        new Dictionary<string, NPCPersistentState>();

    private WorldStateManager worldState;
    private EventBus eventBus;

    public NPCSystem(WorldStateManager worldState, EventBus eventBus)
    {
        this.worldState = worldState;
        this.eventBus = eventBus;
    }

    public string SpawnNPC(NPCDefinition definition, string zoneId)
    {
        var npc = new NPCPersistentState(definition, zoneId);
        npcs.Add(npc.RuntimeId, npc);

        return npc.RuntimeId;
    }

    public NPCPersistentState GetNPC(string runtimeId)
    {
        if (npcs.TryGetValue(runtimeId, out var npc))
            return npc;

        return null;
    }

    public List<NPCPersistentState> GetAllNPCs()
    {
        return new List<NPCPersistentState>(npcs.Values);
    }

    public int GetNPCCountInZone(string zoneId)
    {
        int count = 0;

        foreach (var npc in npcs.Values)
        {
            if (npc.CurrentZoneId == zoneId && npc.IsAlive)
                count++;
        }

        return count;
    }

    public void KillNPC(string runtimeId)
    {
        if (!npcs.ContainsKey(runtimeId))
            return;

        var npc = npcs[runtimeId];

        if (!npc.IsAlive)
            return;

        npc.Kill();

        eventBus.Publish(new NPCKilledEvent(
            runtimeId,
            npc.Definition.faction,
            npc.CurrentZoneId
        ));
    }

    public void DebugCureFirstInfected()
    {
        foreach (var npc in npcs.Values)
        {
            if (npc.Condition == NPCCondition.Infected)
            {
                npc.Cure(eventBus);
                Debug.Log($"DEBUG: Cured {npc.RuntimeId}");
                return;
            }
        }

        Debug.Log("DEBUG: No infected NPC found");
    }
}