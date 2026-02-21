using UnityEngine;

public class WorldEventLogger
{
    public void Log(string message)
    {
        Debug.Log($"[WORLD] {message}");
    }
}