using UnityEngine;

public class SceneContext : MonoBehaviour
{
    private PlayerState playerState;
    private EventBus eventBus;

    private void Awake()
    {
        eventBus = ServiceLocator.Get<EventBus>();
        playerState = ServiceLocator.Get<PlayerState>();

        eventBus.Subscribe<PlayerLevelUpEvent>(OnPlayerLevelUp);
        eventBus.Subscribe<ReputationChangedEvent>(e =>
        {
            Debug.Log($"Reputation changed with {e.Faction}: {e.NewValue}");
        });

    }

    private void OnPlayerLevelUp(PlayerLevelUpEvent e)
    {
        Debug.Log("LEVEL UP DETECTED IN WORLD: " + e.NewLevel);
    }

    private void OnDestroy()
    {
        eventBus.Unsubscribe<PlayerLevelUpEvent>(OnPlayerLevelUp);
    }
}