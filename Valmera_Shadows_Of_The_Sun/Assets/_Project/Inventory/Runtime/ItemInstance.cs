public class ItemInstance
{
    public ItemDefinition Definition { get; private set; }
    public int Quantity { get; private set; }

    public ItemInstance(ItemDefinition definition, int quantity)
    {
        Definition = definition;
        Quantity = quantity;
    }

    public void Add(int amount)
    {
        Quantity += amount;
    }

    public void Remove(int amount)
    {
        Quantity -= amount;
    }
}