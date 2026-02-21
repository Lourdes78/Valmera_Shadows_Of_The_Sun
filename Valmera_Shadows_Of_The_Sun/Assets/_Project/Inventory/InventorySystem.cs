using System.Collections.Generic;

public class InventorySystem
{
    private readonly List<InventorySlot> slots;
    private readonly EventBus eventBus;

    public IReadOnlyList<InventorySlot> Slots => slots;
    public int MaxSlots { get; private set; }

    public InventorySystem(int maxSlots, EventBus bus)
    {
        MaxSlots = maxSlots;
        eventBus = bus;

        slots = new List<InventorySlot>();

        for (int i = 0; i < MaxSlots; i++)
            slots.Add(new InventorySlot());
    }

    public bool AddItem(ItemDefinition definition, int amount)
    {
        if (definition.Stackable)
        {
            foreach (var slot in slots)
            {
                if (!slot.IsEmpty &&
                    slot.Item.Definition == definition &&
                    slot.Item.Quantity < definition.MaxStack)
                {
                    int space = definition.MaxStack - slot.Item.Quantity;
                    int toAdd = System.Math.Min(space, amount);

                    slot.Item.Add(toAdd);
                    amount -= toAdd;

                    PublishSlotChange(slots.IndexOf(slot));

                    if (amount <= 0)
                        return true;
                }
            }
        }

        while (amount > 0)
        {
            var emptySlot = GetEmptySlot();
            if (emptySlot == null)
                return false; // inventari ple

            int stackAmount = definition.Stackable
                ? System.Math.Min(definition.MaxStack, amount)
                : 1;

            emptySlot.SetItem(new ItemInstance(definition, stackAmount));
            amount -= stackAmount;

            PublishSlotChange(slots.IndexOf(emptySlot));
        }

        return true;
    }

    private InventorySlot GetEmptySlot()
    {
        foreach (var slot in slots)
            if (slot.IsEmpty)
                return slot;

        return null;
    }

    private void PublishSlotChange(int index)
    {
        eventBus.Publish(new InventoryChangedEvent
        {
            SlotIndex = index
        });
    }
}