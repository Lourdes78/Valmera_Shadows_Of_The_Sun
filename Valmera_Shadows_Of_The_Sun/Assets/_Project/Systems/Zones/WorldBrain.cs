using System.Collections.Generic;
using UnityEngine;

public class WorldBrain : MonoBehaviour
{
    [SerializeField] private List<ZoneData> zoneDefinitions;

    private EventBus eventBus;

    private WorldStateManager worldState;
    private AlertSystem alertSystem;
    private PoliticalTensionSystem politicalSystem;
    private EconomyManager economyManager;
    private WorldEventLogger logger;

    private float tickTimer;
    private const float tickInterval = 10f;

    private void Awake()
    {
        Debug.Log("WorldBrain Awake");

        eventBus = ServiceLocator.Get<EventBus>();

        InitializeSystems();
        Debug.Log(worldState == null
    ? "WorldState is NULL before register"
    : "WorldState created correctly");

        ServiceLocator.Register(worldState);

        SubscribeToEvents();
    }

    private void InitializeSystems()
    {
        worldState = new WorldStateManager();
        worldState.Initialize(zoneDefinitions, eventBus);

        alertSystem = new AlertSystem(worldState);
        politicalSystem = new PoliticalTensionSystem(worldState);
        economyManager = new EconomyManager();
        logger = new WorldEventLogger();

        logger.Log("World initialized");
    }

    private void SubscribeToEvents()
    {
        eventBus.Subscribe<PlayerLevelUpEvent>(OnPlayerLevelUp);
        eventBus.Subscribe<ReputationChangedEvent>(OnReputationChanged);
        eventBus.Subscribe<ZoneAlertStateChangedEvent>(OnZoneAlertStateChanged);
    }

    private void OnZoneAlertStateChanged(ZoneAlertStateChangedEvent e)
    {
        logger.Log($"Zone {e.ZoneId} changed alert state to {e.NewState}");
    }

    private void OnPlayerLevelUp(PlayerLevelUpEvent e)
    {
        alertSystem.ReduceAlertOnLevelUp();
        logger.Log($"Player level up: {e.NewLevel}");
    }

    private void OnReputationChanged(ReputationChangedEvent e)
    {
        politicalSystem.HandleReputationChange(e.Faction, e.NewValue);
    }

    private void Update()
    {
        tickTimer += Time.deltaTime;

        if (tickTimer >= tickInterval)
        {
            tickTimer = 0f;
            WorldTick();
        }
    }

    private void WorldTick()
    {
        alertSystem.GlobalDecay(0.01f);
        economyManager.WorldTick();

        logger.Log("World Tick executed");
    }
}