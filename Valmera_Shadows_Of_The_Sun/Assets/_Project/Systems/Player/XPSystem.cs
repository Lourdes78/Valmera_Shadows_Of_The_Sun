using UnityEngine;

public class XPSystem
{
    private int currentXP;
    private int currentLevel;

    private EventBus eventBus;

    public int CurrentXP => currentXP;
    public int CurrentLevel => currentLevel;

    public XPSystem(int startLevel, EventBus bus)
    {
        currentLevel = startLevel;
        currentXP = 0;
        eventBus = bus;
    }

    public void AddXP(int amount)
    {
        currentXP += amount;

        int xpToNext = GetXPRequiredForNextLevel();

        while (currentXP >= GetXPRequiredForNextLevel())
        {
            currentXP -= GetXPRequiredForNextLevel();
            LevelUp();
        }

        eventBus.Publish(new PlayerXPChangedEvent
        {
            CurrentXP = currentXP,
            XPToNextLevel = GetXPRequiredForNextLevel()
        });
    }

    private void LevelUp()
    {
        currentLevel++;

        eventBus.Publish(new PlayerLevelUpEvent
        {
            NewLevel = currentLevel
        });
    }

    private int GetXPRequiredForNextLevel()
    {
        return 100 + (currentLevel * 25);
    }
}