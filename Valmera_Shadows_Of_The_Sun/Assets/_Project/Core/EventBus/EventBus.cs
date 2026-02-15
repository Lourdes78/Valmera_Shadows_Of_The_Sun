using System;
using System.Collections.Generic;

public class EventBus
{
    private Dictionary<Type, Delegate> eventTable = new();

    public void Subscribe<T>(Action<T> listener)
    {
        var type = typeof(T);

        if (eventTable.ContainsKey(type))
            eventTable[type] = Delegate.Combine(eventTable[type], listener);
        else
            eventTable[type] = listener;
    }

    public void Unsubscribe<T>(Action<T> listener)
    {
        var type = typeof(T);

        if (!eventTable.ContainsKey(type))
            return;

        var currentDel = Delegate.Remove(eventTable[type], listener);

        if (currentDel == null)
            eventTable.Remove(type);
        else
            eventTable[type] = currentDel;
    }

    public void Publish<T>(T eventData)
    {
        var type = typeof(T);

        if (!eventTable.ContainsKey(type))
            return;

        var callback = eventTable[type] as Action<T>;
        callback?.Invoke(eventData);
    }
}