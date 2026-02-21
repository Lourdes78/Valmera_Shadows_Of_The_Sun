using UnityEngine;
using System.Linq;
using System.Collections;

public class CraftingSystem
{
    private readonly InventorySystem inventory;
    private readonly EventBus eventBus;

    private readonly MonoBehaviour coroutineRunner;

    public CraftingSystem(InventorySystem inventorySystem, EventBus bus, MonoBehaviour runner)
    {
        inventory = inventorySystem;
        eventBus = bus;
        coroutineRunner = runner;
    }

    public bool CanCraft(RecipeDefinition recipe, int amount)
    {
        foreach (var ingredient in recipe.Ingredients)
        {
            int totalRequired = ingredient.QuantityRequired * amount;
            int totalAvailable = GetItemCount(ingredient.Item);

            if (totalAvailable < totalRequired)
                return false;
        }

        return true;
    }

    public bool Craft(RecipeDefinition recipe, int amount, CraftingStationType stationType)
    {
        if (recipe == null)
        {
            UnityEngine.Debug.LogError("Recipe is NULL");
            return false;
        }

        if (recipe.ResultItem == null)
        {
            UnityEngine.Debug.LogError($"Recipe {recipe.name} has no ResultItem assigned.");
            return false;
        }

        if (recipe.StationType != stationType)
        {
            UnityEngine.Debug.LogWarning($"Recipe {recipe.DisplayName} cannot be crafted at {stationType}");
            return false;
        }

        if (!CanCraft(recipe, amount))
            return false;

        //  ARA NO TOQUEM INVENTARI AQUÍ
        coroutineRunner.StartCoroutine(CraftRoutine(recipe, amount));

        return true;
    }

    private int GetItemCount(ItemDefinition definition)
    {
        int total = 0;

        foreach (var slot in inventory.Slots)
        {
            if (!slot.IsEmpty &&
                slot.Item.Definition == definition)
            {
                total += slot.Item.Quantity;
            }
        }

        return total;
    }

    private void RemoveItem(ItemDefinition definition, int amount)
    {
        foreach (var slot in inventory.Slots)
        {
            if (slot.IsEmpty)
                continue;

            if (slot.Item.Definition == definition)
            {
                int remove = System.Math.Min(slot.Item.Quantity, amount);
                slot.Item.Remove(remove);
                amount -= remove;

                if (slot.Item.Quantity <= 0)
                    slot.Clear();

                if (amount <= 0)
                    return;
            }
        }
    }
    private IEnumerator CraftRoutine(RecipeDefinition recipe, int amount)
    {
        eventBus.Publish(new CraftingStartedEvent
        {
            Recipe = recipe,
            Duration = recipe.CraftTime
        });

        Debug.Log("Before Wait");

        yield return new WaitForSeconds(recipe.CraftTime);

        Debug.Log("After Wait");

        foreach (var ingredient in recipe.Ingredients)
        {
            int totalRequired = ingredient.QuantityRequired * amount;
            RemoveItem(ingredient.Item, totalRequired);
        }

        inventory.AddItem(recipe.ResultItem, recipe.ResultQuantity * amount);

        eventBus.Publish(new CraftingCompletedEvent
        {
            Recipe = recipe,
            CraftedAmount = amount
        });
    }
}