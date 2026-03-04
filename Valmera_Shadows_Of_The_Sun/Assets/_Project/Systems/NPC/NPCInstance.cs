using UnityEngine;

public class NPCInstance : MonoBehaviour
{
    [SerializeField] private NPCDefinition definition;

    public NPCDefinition Definition => definition;

    public bool IsInfected { get; private set; }
    public bool IsRescued { get; private set; }

    private int currentHealth;

    public void Initialize(bool infectedOverride = false)
    {
        currentHealth = definition.baseHealth;

        if (definition.canBeInfected)
            IsInfected = infectedOverride;
        else
            IsInfected = false;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0)
            Die();
    }

    private void Die()
    {
        // EventBus.Publish(new NPCDiedEvent(this));
        Destroy(gameObject);
    }

    public void Rescue()
    {
        if (IsRescued)
            return;

        IsRescued = true;

        // EventBus.Publish(new NPCRescuedEvent(this));
    }

    public void CureInfection()
    {
        if (!IsInfected)
            return;

        IsInfected = false;

        // EventBus.Publish(new NPCCuredEvent(this));
    }
}
