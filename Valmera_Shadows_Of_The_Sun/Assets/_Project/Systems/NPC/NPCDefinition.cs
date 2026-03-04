using UnityEngine;

public enum NPCFaction
{
    Civil,
    Merchant,
    Contrabandist,
    Ambient,
    Soldier,
    Zombie
}

public enum NPCType
{
    Civilian,
    Guard,
    Merchant,
    Zombie,
    Ambient
}

public enum SoldierTier
{
    None,
    Basic,
    Veteran,
    Advanced,
    Elite,
    Alpha
}
public enum ZombieTier
{
    None,
    Basic,
    Rotten,
    Mutated,
    Brutal,
    Alpha
}

[CreateAssetMenu(menuName = "NPC/NPC Definition")]
public class NPCDefinition : ScriptableObject
{
    [Header("Identity")]
    public string npcId;
    public string displayName;

    [Header("Classification")]
    public NPCFaction faction;
    public NPCType npcType;

    [Header("Enemy Classification")]
    public SoldierTier soldierTier;
    public ZombieTier zombieTier;

    [Header("Stats Base")]
    public int baseHealth;
    public int baseDamage;

    [Header("Behavior Flags")]
    public bool canGiveMissions;
    public bool canTrade;
    public bool canBeInfected;

    [Header("Economy")]
    public int baseLootMinXP;
    public int baseLootMaxXP;

    public int baseLootMinCS;
    public int baseLootMaxCS;
}