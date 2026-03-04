using UnityEngine;

public class NPCInteraction : MonoBehaviour
{
    public string npcId;
    public ItemDefinition requestedItem;

    private InventorySystem inventory;
    private EventBus eventBus;

    void Start()
    {
        var world = FindFirstObjectByType<WorldBrain>();
        inventory = world.Inventory;
        eventBus = world.EventBus;
    }

    public void Deliver()
    {
        if (inventory.RemoveItem(requestedItem, 1))
        {
            eventBus.Publish(new ItemDeliveredEvent
            {
                NpcId = npcId,
                Item = requestedItem,
                Amount = 1
            });
        }
    }
}