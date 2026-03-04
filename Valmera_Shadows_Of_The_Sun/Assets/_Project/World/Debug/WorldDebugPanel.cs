using UnityEngine;
using System.Text;

public class WorldDebugPanel : MonoBehaviour
{
    private WorldStateManager worldState;
    private bool visible = true;
    private EconomyManager economyManager;

    private void Awake()
    {

    }

    private void Start()
    {
        Debug.Log("WorldDebugPanel Start");
       
    }

    private void Update()
    {
        if (worldState == null)
        {
            TryResolveWorldState();
            return;
        }
        if (Input.GetKeyDown(KeyCode.F3))
        {
            visible = !visible;
        }
        // Toggle debug (ho arreglem després)
    }

    private void TryResolveWorldState()
    {
        try
        {
            worldState = ServiceLocator.Get<WorldStateManager>();
            Debug.Log("WorldDebugPanel connected to WorldStateManager");
        }
        catch
        {
            // Encara no registrat, ignorem
        }
    }

    private void OnGUI()
    {
        if (!visible || worldState == null)
            return;

        var zones = worldState.GetAllZones();
        var economy = ServiceLocator.Get<EconomyManager>();

        StringBuilder sb = new StringBuilder();
        sb.AppendLine("=== WORLD DEBUG ===");

        foreach (var zone in zones)
        {
            int npcCount = ServiceLocator.Get<NPCSystem>()
            .GetNPCCountInZone(zone.Definition.ZoneId);

            sb.AppendLine(
                $"{zone.Definition.ZoneId} | Alert: {zone.CurrentAlertLevel:F2} | " +
                $"State: {zone.CurrentState} | Eco: {economy.GetEconomy(zone.Definition.ZoneId):F2} | " +
                $"NPCs: {npcCount}"
            );
            /*  sb.AppendLine(
              $"{zone.Definition.ZoneId} | Alert: {zone.CurrentAlertLevel:F2} | " +
                $"PoliticalWeight: {zone.Definition.PoliticalWeight:F2} | " +
                $"EconomyWeight: {zone.Definition.EconomyWeight:F2}"

             // $"{zone.Definition.ZoneId} | Alert: {zone.CurrentAlertLevel:F2} | State: {zone.CurrentState} | Eco: {zone.EconomicStability:F2}"
            );*/
        }

        GUI.Box(new Rect(10, 10, 400, 200), sb.ToString());
    }
}