using UnityEngine;

public class SceneContext : MonoBehaviour
{
    private PlayerState playerState;
    private EventBus eventBus;
    private SaveSystem saveSystem;

    private void Awake()
    {
        eventBus = ServiceLocator.Get<EventBus>();
        playerState = ServiceLocator.Get<PlayerState>();
        saveSystem = ServiceLocator.Get<SaveSystem>();

        eventBus.Subscribe<PlayerLevelUpEvent>(OnPlayerLevelUp);
        eventBus.Subscribe<ReputationChangedEvent>(e =>
        {
            Debug.Log($"Reputation changed with {e.Faction}: {e.NewValue}");
        });
        eventBus.Subscribe<InventoryChangedEvent>(e =>
        {
            Debug.Log($"Inventory slot changed: {e.SlotIndex}");
        });

        eventBus.Subscribe<CraftingCompletedEvent>(e =>
{
            Debug.Log($"Crafted: {e.Recipe.DisplayName} x{e.CraftedAmount}");
        });

    }

    private void OnPlayerLevelUp(PlayerLevelUpEvent e)
    {
        Debug.Log("LEVEL UP DETECTED IN WORLD: " + e.NewLevel);
        saveSystem.Save(playerState); 
    }

    private void OnDestroy()
    {
        eventBus.Unsubscribe<PlayerLevelUpEvent>(OnPlayerLevelUp);
    }
}