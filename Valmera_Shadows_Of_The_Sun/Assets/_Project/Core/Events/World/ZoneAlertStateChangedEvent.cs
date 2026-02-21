public class ZoneAlertStateChangedEvent
{
    public string ZoneId { get; }
    public AlertState NewState { get; }

    public ZoneAlertStateChangedEvent(string zoneId, AlertState newState)
    {
        ZoneId = zoneId;
        NewState = newState;
    }
}