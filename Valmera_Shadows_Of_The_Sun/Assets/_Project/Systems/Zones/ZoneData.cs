using UnityEngine;

[CreateAssetMenu(menuName = "World/Zone Data")]
public class ZoneData : ScriptableObject
{
    [Header("Identity")]
    public string ZoneId;
    public string DisplayName;
    public ZoneType ZoneType;

    [Header("Base Simulation Values")]
    [Range(0f, 1f)]
    public float DefaultAlertLevel = 0.1f;

    [Range(0f, 1f)]
    public float PoliticalWeight = 0.5f;

    [Range(0f, 1f)]
    public float EconomyWeight = 0.5f;
}