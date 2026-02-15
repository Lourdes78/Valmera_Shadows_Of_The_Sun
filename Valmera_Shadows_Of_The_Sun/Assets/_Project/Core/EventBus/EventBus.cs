using System;
using System.Collections.Generic;

public class EventBus
{
    private readonly Dictionary<Type, Delegate> eventTable = new();
    private bool debugMode = false;

    public void EnableDebug(bool enabled)
    {
        debugMode = enabled;
    }

    public void Subscribe<T>(Action<T> listener)
    {
        var type = typeof(T);

        if (listener == null)
            return;

        if (eventTable.TryGetValue(type, out var existingDelegate))
        {
            foreach (var d in existingDelegate.GetInvocationList())
            {
                if (d.Equals(listener))
                    return; // evita duplicats
            }

            eventTable[type] = Delegate.Combine(existingDelegate, listener);
        }
        else
        {
            eventTable[type] = listener;
        }

        if (debugMode)
            UnityEngine.Debug.Log($"[EventBus] Subscribed to {type.Name}");
    }

    public void Unsubscribe<T>(Action<T> listener)
    {
        var type = typeof(T);

        if (!eventTable.TryGetValue(type, out var existingDelegate))
            return;

        var newDelegate = Delegate.Remove(existingDelegate, listener);

        if (newDelegate == null)
            eventTable.Remove(type);
        else
            eventTable[type] = newDelegate;

        if (debugMode)
            UnityEngine.Debug.Log($"[EventBus] Unsubscribed from {type.Name}");
    }

    public void Publish<T>(T eventData)
    {
        var type = typeof(T);

        if (!eventTable.TryGetValue(type, out var del))
            return;

        if (debugMode)
            UnityEngine.Debug.Log($"[EventBus] Publishing {type.Name}");

        var callback = del as Action<T>;
        callback?.Invoke(eventData);
    }

    public void Clear()
    {
        eventTable.Clear();
    }
}