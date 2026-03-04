using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float interactDistance = 3f;

    public RecipeDefinition testRecipe;
    public CraftingStationType testStation;
    private EventBus eventBus;
    private InventorySystem inventory;
    private CraftingSystem crafting;

    void Start()
    {
        var brain = FindFirstObjectByType<WorldBrain>();
        eventBus = brain.EventBus;
        inventory = brain.Inventory;
        crafting = brain.Crafting;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
            TryKill();

        if (Input.GetKeyDown(KeyCode.C))
            TryCure();

        if (Input.GetKeyDown(KeyCode.E))
            TryPickup();

        if (Input.GetKeyDown(KeyCode.R))
            crafting.Craft(testRecipe, 1, testStation);
    }

    void TryKill()
    {
        Debug.Log("Pressed K");

        if (Physics.Raycast(Camera.main.transform.position,
                            Camera.main.transform.forward,
                            out RaycastHit hit,
                            interactDistance,
                            ~0,
                            QueryTriggerInteraction.Collide))
        {
            Debug.Log("Hit: " + hit.collider.name);

            var npc = hit.collider.GetComponent<NPCBehaviour>();
            if (npc != null)
            {
                Debug.Log("Found NPC");
                npc.Kill();
            }
        }
    }
    void TryCure()
    {
        if (Physics.Raycast(Camera.main.transform.position,
                            Camera.main.transform.forward,
                            out RaycastHit hit,
                            interactDistance))
        {
            var npc = hit.collider.GetComponent<NPCBehaviour>();
            if (npc != null)
                npc.Cure();
        }
    }

    void TryPickup()
    {
        if (Physics.Raycast(Camera.main.transform.position,
                            Camera.main.transform.forward,
                            out RaycastHit hit,
                            interactDistance))
        {
            var pickup = hit.collider.GetComponent<ItemPickup>();
            if (pickup != null)
                pickup.Pick();
        }
    }
}