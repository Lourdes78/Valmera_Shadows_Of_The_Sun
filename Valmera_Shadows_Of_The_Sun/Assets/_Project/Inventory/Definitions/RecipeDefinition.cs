using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Game/Inventory/Recipe Definition")]
public class RecipeDefinition : ScriptableObject
{
    [Header("Identity")]
    public string Id;
    public string DisplayName;
    [TextArea] public string Description;
    public Sprite Icon;

    [Header("Classification")]
    public ItemCategory Category;
    public CraftingStationType StationType;

    [Header("Result")]
    public ItemDefinition ResultItem;
    public int ResultQuantity = 1;

    [Header("Ingredients")]
    public List<IngredientData> Ingredients = new List<IngredientData>();

    [Header("Crafting")]
    public float CraftTime = 3f; // segons

}