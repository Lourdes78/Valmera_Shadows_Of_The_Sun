using UnityEngine;
using UnityEngine.SceneManagement;

public class GameBootstrap : MonoBehaviour
{
    [SerializeField] private ItemDefinition healthPotion, wood, stone;
    [SerializeField] private RecipeDefinition recipeHealthPotion;
    private SaveSystem saveSystem;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Debug.Log("WorldBrain Awake");
        InitializeCore();
        LoadMainWorld();
    }

    private void InitializeCore()
    {
        ServiceLocator.Clear();

        var eventBus = new EventBus();
        eventBus.EnableDebug(true);
        ServiceLocator.Register(eventBus);

        var playerState = new PlayerState(eventBus, this);
        ServiceLocator.Register(playerState);

        saveSystem = new SaveSystem();          
        ServiceLocator.Register(saveSystem);    

        Debug.Log("Core Initialized");
    }

    private void LoadMainWorld()
    {
        SceneManager.LoadScene("World_Main");
    }

    private void Start()
    {
        var player = ServiceLocator.Get<PlayerState>();

        saveSystem = ServiceLocator.Get<SaveSystem>();

        var data = saveSystem.Load();
        player.ApplyLoadedData(data);

        player.Stats.ModifyHealth(-20);
        player.ReputationSystem.ModifyReputation(NPCFaction.Civil, -100);

        player.InventorySystem.AddItem(healthPotion, 5);
        player.InventorySystem.AddItem(wood, 5);
        player.InventorySystem.AddItem(stone, 3);

        player.CraftingSystem.Craft(recipeHealthPotion, 1, CraftingStationType.Player);

        player.XPSystem.AddXP(200);
    }

}