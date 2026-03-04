using UnityEngine;

public class PrisonCampZone : MonoBehaviour
{
    public string zoneId = "prison_zone_1";
    public int prisonersRequired = 3;

    private int rescuedCount = 0;
    private bool isActive = false;

    private EventBus eventBus;

    void Start()
    {
        eventBus = FindFirstObjectByType<WorldBrain>().EventBus;
        eventBus.Subscribe<NPCRescuedEvent>(OnNPCRescued);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ActivateZone();
        }
    }

    void ActivateZone()
    {
        isActive = true;
        ShowBanner("Allibera els presoners retinguts");
    }

    void OnNPCRescued(NPCRescuedEvent e)
    {
        if (!isActive) return;

        rescuedCount++;

        if (rescuedCount >= prisonersRequired)
        {
            CompleteZone();
        }
    }

    void CompleteZone()
    {
        int xp = Random.Range(200, 400);
        int cs = Random.Range(20, 50);

        var world = FindFirstObjectByType<WorldBrain>();
        world.XPSystem.AddXP(xp);
        world.CurrencySystem.Add(cs);

        ShowBanner("Zona completada!");

        isActive = false;
    }

    void ShowBanner(string message)
    {
        Debug.Log(message);
    }
}
