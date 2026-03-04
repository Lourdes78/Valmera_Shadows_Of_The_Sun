using System.Collections.Generic;
using UnityEngine;

public class MissionSystem
{
    private List<MissionRuntimeState> activeMissions = new();

    private EventBus eventBus;
    private XPSystem playerProgression;
    private InventorySystem inventory;

    public MissionSystem(EventBus eventBus, XPSystem progression, InventorySystem inv)
    {
        this.eventBus = eventBus;
        this.playerProgression = progression;
        this.inventory = inv;

        Subscribe();
    }

    private void Subscribe()
    {
        eventBus.Subscribe<NPCKilledEvent>(OnNPCKilled);
        eventBus.Subscribe<NPCCuredEvent>(OnNPCCured);
        eventBus.Subscribe<PlayerEnteredZoneEvent>(OnPlayerEnteredZone);
        eventBus.Subscribe<InventoryChangedEvent>(OnInventoryChanged);
        eventBus.Subscribe<CraftingCompletedEvent>(OnCraftCompleted);
        eventBus.Subscribe<ItemDeliveredEvent>(OnItemDelivered);
        eventBus.Subscribe<NPCRescuedEvent>(OnNPCRescued);
    }

    public void AddMission(MissionDefinition definition)
    {
        Debug.Log("Mission added: " + definition.Name);
        activeMissions.Add(new MissionRuntimeState(definition));
    }

    private void OnNPCKilled(NPCKilledEvent e)
    {
        ProcessObjective(MissionObjectiveType.KillNPC, e.NpcId);
    }

    private void OnNPCCured(NPCCuredEvent e)
    {
        ProcessObjective(MissionObjectiveType.CureNPC, e.NpcId);
    }

    private void OnPlayerEnteredZone(PlayerEnteredZoneEvent e)
    {
        Debug.Log("Entered Zone: " + e.ZoneId);
        ProcessObjective(MissionObjectiveType.ReachZone, e.ZoneId);
    }
    private void OnNPCRescued(NPCRescuedEvent e)
    {
        ProcessObjective(MissionObjectiveType.RescueNPC, e.NpcId);
    }

    private void ProcessObjective(MissionObjectiveType type, string targetId)
    {
        foreach (var mission in activeMissions)
        {
            if (mission.IsCompleted)
                continue;

            if (mission.Definition.IsSequential)
            {
                var currentObj = mission.Objectives[mission.CurrentObjectiveIndex];

                if (currentObj.Definition.Type == type &&
                    currentObj.Definition.TargetId == targetId)
                {
                    currentObj.AddProgress();

                    Debug.Log(
                        $"[Mission: {mission.Definition.Name}] " +
                        $"Step {mission.CurrentObjectiveIndex + 1}/" +
                        $"{mission.Objectives.Count} completed"
                    );

                    if (currentObj.IsCompleted)
                    {
                        mission.CurrentObjectiveIndex++;

                        if (mission.CurrentObjectiveIndex >= mission.Objectives.Count)
                        {
                            CompleteMission(mission);
                        }
                    }
                }
            }
            else
            {
                // comportament normal (el que ja tens)
                foreach (var obj in mission.Objectives)
                {
                    if (obj.Definition.Type == type &&
                        obj.Definition.TargetId == targetId)
                    {
                        obj.AddProgress();
                        Debug.Log(
                            $"[Mission: {mission.Definition.Name}] " +
                        $"{obj.Definition.Type} {obj.CurrentAmount}/{obj.Definition.RequiredAmount}"
);
                    }
                }

                if (mission.AreAllObjectivesCompleted())
                {
                    Debug.Log("All objectives completed for: " + mission.Definition.Name);
                    CompleteMission(mission);
                }
            }
        }
    }
    private int GetItemCount(ItemDefinition definition)
    {
        int total = 0;

        foreach (var slot in inventory.Slots)
        {
            if (!slot.IsEmpty &&
                slot.Item.Definition == definition)
            {
                total += slot.Item.Quantity;
            }
        }

        return total;
    }
    private void OnInventoryChanged(InventoryChangedEvent e)
    {
        foreach (var mission in activeMissions)
        {
            if (!mission.Definition.IsSequential)
                continue;

            if (mission.IsCompleted)
                continue;

            var currentObj = mission.Objectives[mission.CurrentObjectiveIndex];

            if (currentObj.Definition.Type != MissionObjectiveType.CollectItem)
                continue;

            int count = GetItemCount(currentObj.Definition.TargetItem);

            if (count >= currentObj.Definition.RequiredAmount)
            {
                currentObj.AddProgress();

                mission.CurrentObjectiveIndex++;

                Debug.Log($"Step {mission.CurrentObjectiveIndex}/{mission.Objectives.Count}");

                if (mission.CurrentObjectiveIndex >= mission.Objectives.Count)
                    CompleteMission(mission);
            }
        }
    }
    private void OnCraftCompleted(CraftingCompletedEvent e)
    {
        foreach (var mission in activeMissions)
        {
            if (!mission.Definition.IsSequential)
                continue;

            if (mission.IsCompleted)
                continue;

            var currentObj = mission.Objectives[mission.CurrentObjectiveIndex];

            if (currentObj.Definition.Type != MissionObjectiveType.CraftItem)
                continue;

            if (currentObj.Definition.TargetItem == e.Recipe.ResultItem)
            {
                currentObj.AddProgress();

                mission.CurrentObjectiveIndex++;

                Debug.Log($"Step {mission.CurrentObjectiveIndex}/{mission.Objectives.Count}");

                if (mission.CurrentObjectiveIndex >= mission.Objectives.Count)
                    CompleteMission(mission);
            }
        }
    }
    private void OnItemDelivered(ItemDeliveredEvent e)
    {
        foreach (var mission in activeMissions)
        {
            if (mission.IsCompleted)
                continue;

            foreach (var objective in mission.Objectives)
            {
                if (objective.Definition.Type != MissionObjectiveType.DeliverItem)
                    continue;

                if (objective.Definition.TargetItem == e.Item &&
                    objective.Definition.TargetId == e.NpcId)
                {
                    objective.CurrentAmount += e.Amount;
                }
            }

            if (mission.AreAllObjectivesCompleted())
                CompleteMission(mission);
        }
    }

    private void CompleteMission(MissionRuntimeState mission)
    {
        mission.IsCompleted = true;

        playerProgression.AddXP(mission.Definition.XPReward);

        Debug.Log($"MISSION COMPLETED: {mission.Definition.Name}");
    }

}