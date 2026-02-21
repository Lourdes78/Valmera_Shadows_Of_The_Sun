public class InventorySlot
{
    public ItemInstance Item { get; private set; }

    public bool IsEmpty => Item == null;

    public void SetItem(ItemInstance instance)
    {
        Item = instance;
    }

    public void Clear()
    {
        Item = null;
    }
}