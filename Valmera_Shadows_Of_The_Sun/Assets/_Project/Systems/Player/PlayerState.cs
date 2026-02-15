public class PlayerState
{
    public XPSystem XPSystem { get; private set; }
    public PlayerStats Stats { get; private set; }

    public int Influence { get; private set; }
    public ReputationSystem ReputationSystem { get; private set; }

    private EventBus eventBus;

    public PlayerState(EventBus bus)
    {
        eventBus = bus;

        Stats = new PlayerStats();
        XPSystem = new XPSystem(1, bus);

        Influence = 0;
        ReputationSystem = new ReputationSystem(bus);

    }

    public void AddInfluence(int amount)
    {
        Influence += amount;

        // Preparat per futur event
        // eventBus.Publish(new PlayerInfluenceChangedEvent(...));
    }
}