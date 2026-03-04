using System.Collections.Generic;

public class MissionDefinition
{
    public string Id;
    public string Name;
    public MissionType Type;
    public int XPReward;
    public bool IsSequential;

    public List<ObjectiveDefinition> Objectives;
}