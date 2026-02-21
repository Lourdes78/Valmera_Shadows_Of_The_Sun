using System;
using UnityEngine;


public class ZoneRuntimeState
{
    public ZoneData Definition { get; private set; }

    public float CurrentAlertLevel { get; private set; }

    private EventBus eventBus;

    public AlertState CurrentState { get; private set; }

    public ZoneRuntimeState(ZoneData definition, EventBus eventBus)
    {
        Definition = definition;
        this.eventBus = eventBus;

        CurrentAlertLevel = definition.DefaultAlertLevel;

        EvaluateState(); // inicialitzem estat
    }
    private void EvaluateState()
    {
        AlertState previous = CurrentState;

        if (CurrentAlertLevel < 0.3f)
            CurrentState = AlertState.Calm;
        else if (CurrentAlertLevel < 0.6f)
            CurrentState = AlertState.Suspicious;
        else if (CurrentAlertLevel < 0.85f)
            CurrentState = AlertState.Unrest;
        else
            CurrentState = AlertState.Lockdown;

        if (previous != CurrentState)
        {
            eventBus?.Publish(
                new ZoneAlertStateChangedEvent(Definition.ZoneId, CurrentState)
            );
        }
    }

    public void ModifyAlert(float amount)
    {
        CurrentAlertLevel = Mathf.Clamp01(CurrentAlertLevel + amount);
        EvaluateState();
    }

    public void SetAlert(float value)
    {
        CurrentAlertLevel = Mathf.Clamp01(value);
        EvaluateState();
    }
}