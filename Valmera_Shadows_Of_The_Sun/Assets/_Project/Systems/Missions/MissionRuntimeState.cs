using System.Collections.Generic;
using System.Linq;

public class MissionRuntimeState
{
    public MissionDefinition Definition;
    public List<ObjectiveRuntimeState> Objectives;

    public int CurrentObjectiveIndex;

    public bool IsCompleted;

    public MissionRuntimeState(MissionDefinition definition)
    {
        Definition = definition;
        Objectives = new List<ObjectiveRuntimeState>();
        CurrentObjectiveIndex = 0;

        foreach (var obj in definition.Objectives)
        {
            Objectives.Add(new ObjectiveRuntimeState(obj));
        }

        IsCompleted = false;
    }

    public bool AreAllObjectivesCompleted()
    {
        foreach (var obj in Objectives)
        {
            if (!obj.IsCompleted)
                return false;
        }

        return true;
    }
}