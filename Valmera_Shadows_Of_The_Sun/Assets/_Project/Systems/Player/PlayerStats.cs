using System;

[Serializable]
public class PlayerStats
{
    public int MaxHealth { get; private set; }
    public int CurrentHealth { get; private set; }

    public int MaxStamina { get; private set; }
    public int CurrentStamina { get; private set; }

    public int ToxicityResistance { get; private set; }

    public PlayerStats()
    {
        MaxHealth = 100;
        CurrentHealth = 100;

        MaxStamina = 100;
        CurrentStamina = 100;

        ToxicityResistance = 0;
    }

    public void ModifyHealth(int amount)
    {
        CurrentHealth = Math.Clamp(CurrentHealth + amount, 0, MaxHealth);
    }

    public void ModifyStamina(int amount)
    {
        CurrentStamina = Math.Clamp(CurrentStamina + amount, 0, MaxStamina);
    }

    public void IncreaseMaxHealth(int amount)
    {
        MaxHealth += amount;
        CurrentHealth = MaxHealth;
    }

    public void IncreaseMaxStamina(int amount)
    {
        MaxStamina += amount;
        CurrentStamina = MaxStamina;
    }

    public void SetHealth(int value)
    {
        CurrentHealth = value;
    }

    public void SetMaxHealth(int value)
    {
        MaxHealth = value;
    }

    public void SetStamina(int value)
    {
        CurrentStamina = value;
    }

    public void SetMaxStamina(int value)
    {
        MaxStamina = value;
    }

}