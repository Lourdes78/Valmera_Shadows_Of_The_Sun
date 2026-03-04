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
    public bool ConsumeItem(ItemDefinition definition)
    {
        if (!definition.IsConsumable)
            return false;

        if (!RemoveItem(definition, 1))
            return false;

        eventBus.Publish(new ItemConsumedEvent
        {
            Item = definition
        });

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
    public bool RemoveItem(ItemDefinition definition, int amount)
    {
        int totalAvailable = 0;

        foreach (var slot in slots)
        {
            if (!slot.IsEmpty && slot.Item.Definition == definition)
                totalAvailable += slot.Item.Quantity;
        }

        if (totalAvailable < amount)
            return false;

        foreach (var slot in slots)
        {
            if (slot.IsEmpty)
                continue;

            if (slot.Item.Definition == definition)
            {
                int remove = System.Math.Min(slot.Item.Quantity, amount);
                slot.Item.Remove(remove);
                amount -= remove;

                if (slot.Item.Quantity <= 0)
                    slot.Clear();

                PublishSlotChange(slots.IndexOf(slot));

                if (amount <= 0)
                    break;
            }
        }

        return true;
    }
}