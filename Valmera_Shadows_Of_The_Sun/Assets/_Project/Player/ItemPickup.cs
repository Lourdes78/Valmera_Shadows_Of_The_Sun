using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public ItemDefinition definition;
    public int amount = 1;

    private InventorySystem inventory;

    void Start()
    {
        inventory = FindFirstObjectByType<WorldBrain>().Inventory;
    }

    public void Pick()
    {
        if (inventory.AddItem(definition, amount))
            Destroy(gameObject);
    }
}