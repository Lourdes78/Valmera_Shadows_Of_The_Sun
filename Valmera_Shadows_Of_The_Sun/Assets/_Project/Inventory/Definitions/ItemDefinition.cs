using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Game/Inventory/Item Definition")]
//[CreateAssetMenu(fileName = "NewItem", menuName = "Item Definition")]

public class ItemDefinition : ScriptableObject
{
    [Header("Identity")]
    public string Id;
    public string DisplayName;
    [TextArea] public string Description;
    public Sprite Icon;

    [Header("Classification")]
    public ItemCategory Category;

    [Header("Stacking")]
    public bool Stackable = true;
    public int MaxStack = 20;

    [Header("Usage")]
    public bool IsConsumable;

    [Header("Boosts")]
    public ItemBoostData Boosts;
}