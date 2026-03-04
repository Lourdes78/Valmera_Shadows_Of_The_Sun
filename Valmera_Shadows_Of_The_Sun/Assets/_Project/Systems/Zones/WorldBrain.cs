using UnityEngine;
using System.Collections.Generic;

public class WorldBrain : MonoBehaviour
{
    [Header("Zone Definitions")]
    [SerializeField] private List<ZoneData> zoneDefinitions;

    [Header("NPC Definitions")]
    [SerializeField] private NPCDefinition guardDefinition;
    [SerializeField] private NPCDefinition civilDefinition;

    [Header("Inventory Settings")]
    [SerializeField] private int inventorySlots = 20;

    private EventBus eventBus;
    private WorldStateManager worldState;
    private NPCSystem npcSystem;
    private ReputationSystem reputationSystem;
    private InventorySystem inventory;
    private CraftingSystem crafting;
    private XPSystem xpSystem;
    private CurrencySystem currencySystem;

    private float tickTimer;
    private const float tickInterval = 10f;

    public InventorySystem Inventory => inventory;
    public CraftingSystem Crafting => crafting;
    public XPSystem XPSystem => xpSystem;
    public CurrencySystem CurrencySystem => currencySystem;
    public EventBus EventBus => eventBus;

    private void Awake()
    {
        InitializeSystems();
        SpawnInitialNPCs();
    }

    private void Update()
    {
        tickTimer += Time.deltaTime;
        if (tickTimer >= tickInterval)
        {
            tickTimer = 0f;
            worldState.Tick();
        }
    }

    private void InitializeSystems()
    {
        eventBus = new EventBus();

        worldState = new WorldStateManager();
        worldState.Initialize(zoneDefinitions, eventBus);

        xpSystem = new XPSystem(1, eventBus);
        reputationSystem = new ReputationSystem(eventBus);
        currencySystem = new CurrencySystem(eventBus);

        // FIX 1: passem maxSlots
        inventory = new InventorySystem(inventorySlots, eventBus);

        // FIX 2: passem el runner (aquest MonoBehaviour)
        crafting = new CraftingSystem(inventory, eventBus, this);

        npcSystem = new NPCSystem(worldState, eventBus);

        eventBus.Subscribe<NPCKilledEvent>(OnNPCKilled);
        eventBus.Subscribe<NPCCuredEvent>(OnNPCCured);
    }

    private void SpawnInitialNPCs()
    {
        if (zoneDefinitions == null || zoneDefinitions.Count == 0)
            return;

        string firstZone = zoneDefinitions[0].ZoneId;

        if (guardDefinition != null)
            npcSystem.SpawnNPC(guardDefinition, firstZone);

        if (civilDefinition != null)
            npcSystem.SpawnNPC(civilDefinition, firstZone);
    }

    private void OnNPCKilled(NPCKilledEvent e)
    {
        reputationSystem.ModifyReputation(e.Faction, -1);
    }

    private void OnNPCCured(NPCCuredEvent e)
    {
        reputationSystem.ModifyReputation(e.Faction, 1);
    }
}