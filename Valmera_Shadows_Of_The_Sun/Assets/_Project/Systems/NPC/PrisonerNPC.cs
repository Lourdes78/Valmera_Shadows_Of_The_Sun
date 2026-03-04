using UnityEngine;

public class PrisonerNPC : MonoBehaviour
{
    public bool IsCaptured = true;
    public bool IsInfected = false;

    private EventBus eventBus;

    void Start()
    {
        eventBus = FindFirstObjectByType<WorldBrain>().EventBus;
        IsInfected = Random.value > 0.5f;
    }

    void Update()
    {
        if (IsCaptured && PlayerInRange() && Input.GetKeyDown(KeyCode.E))
        {
            Rescue();
        }
    }

    void Rescue()
    {
        IsCaptured = false;

        eventBus.Publish(
           new NPCRescuedEvent(gameObject.name, IsInfected)
        );


        Destroy(gameObject);
    }

    bool PlayerInRange()
    {
        return true; // simplificat
    }
}