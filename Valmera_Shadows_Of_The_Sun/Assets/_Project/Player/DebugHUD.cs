using TMPro;
using UnityEngine;
using System.Text;

public class DebugHUD : MonoBehaviour
{
    public TextMeshProUGUI debugText;

    private EventBus eventBus;
    private InventorySystem inventory;

    private string currentZone = "None";
    private string lastAction = "None";

    void Start()
    {
        var brain = FindFirstObjectByType<WorldBrain>();
        eventBus = brain.EventBus;
        inventory = brain.Inventory;

        eventBus.Subscribe<PlayerEnteredZoneEvent>(OnZoneEntered);
        eventBus.Subscribe<NPCKilledEvent>(e => SetAction("Killed " + e.NpcId));
        eventBus.Subscribe<NPCCuredEvent>(e => SetAction("Cured " + e.NpcId));
        eventBus.Subscribe<CraftingStartedEvent>(e => SetAction("Crafting " + e.Recipe.DisplayName));
        eventBus.Subscribe<CraftingCompletedEvent>(e => SetAction("Crafted " + e.Recipe.DisplayName));
        eventBus.Subscribe<InventoryChangedEvent>(e => Refresh());
    }

    void OnZoneEntered(PlayerEnteredZoneEvent e)
    {
        currentZone = e.ZoneId;
        SetAction("Entered zone " + e.ZoneId);
    }

    void SetAction(string action)
    {
        lastAction = action;
        Refresh();
    }

    void Refresh()
    {
        StringBuilder sb = new StringBuilder();

        sb.AppendLine("=== DEBUG INFO ===");
        sb.AppendLine("Zone: " + currentZone);
        sb.AppendLine("Last Action: " + lastAction);
        sb.AppendLine("");
        sb.AppendLine("Inventory:");

        foreach (var slot in inventory.Slots)
        {
            if (!slot.IsEmpty)
            {
                sb.AppendLine(
                    slot.Item.Definition.DisplayName +
                    " x" + slot.Item.Quantity
                );
            }
        }

        debugText.text = sb.ToString();
    }
}