public class CurrencySystem
{
    private int currentCurrency;
    private EventBus eventBus;

    public int Current => currentCurrency;

    public CurrencySystem(EventBus bus)
    {
        eventBus = bus;
        currentCurrency = 0;
    }

    public void Add(int amount)
    {
        currentCurrency += amount;
    }

    public bool Spend(int amount)
    {
        if (currentCurrency < amount)
            return false;

        currentCurrency -= amount;
        return true;
    }
}