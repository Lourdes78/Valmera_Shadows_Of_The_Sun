public class ObjectiveRuntimeState
{
    public ObjectiveDefinition Definition;
    public int CurrentAmount;
    public bool IsCompleted;

    public ObjectiveRuntimeState(ObjectiveDefinition def)
    {
        Definition = def;
        CurrentAmount = 0;
        IsCompleted = false;
    }

    public void AddProgress(int amount = 1)
    {
        if (IsCompleted)
            return;

        CurrentAmount += amount;

        if (CurrentAmount >= Definition.RequiredAmount)
        {
            IsCompleted = true;
        }
    }

    public void SetProgress(int value)
    {
        CurrentAmount = value;

        if (CurrentAmount > Definition.RequiredAmount)
            CurrentAmount = Definition.RequiredAmount;
    }
}