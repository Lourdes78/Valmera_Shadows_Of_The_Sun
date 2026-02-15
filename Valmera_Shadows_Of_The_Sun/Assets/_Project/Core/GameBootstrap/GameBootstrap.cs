using UnityEngine;
using UnityEngine.SceneManagement;

public class GameBootstrap : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        InitializeCore();
        LoadMainWorld();
    }

    private void InitializeCore()
    {
        ServiceLocator.Clear();

        var eventBus = new EventBus();
        eventBus.EnableDebug(true);
        ServiceLocator.Register(eventBus);

        var playerState = new PlayerState(eventBus);
        ServiceLocator.Register(playerState);

        Debug.Log("Core Initialized");
    }

    private void LoadMainWorld()
    {
        SceneManager.LoadScene("World_Main");
    }

    private void Start()
    {
        var player = ServiceLocator.Get<PlayerState>();

        player.Stats.ModifyHealth(-20);
        player.ReputationSystem.ModifyReputation(FactionType.Villagers, 10);
        Debug.Log("Current HP: " + player.Stats.CurrentHealth);

        player.XPSystem.AddXP(200);
    }

}