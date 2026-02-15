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

        // Core Systems
        ServiceLocator.Register(new EventBus());

        Debug.Log("Core Initialized");
    }

    private void LoadMainWorld()
    {
        SceneManager.LoadScene("World_Main");
    }
}