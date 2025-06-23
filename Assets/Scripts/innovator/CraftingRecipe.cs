using UnityEngine;
using System.Collections.Generic; // Required for List

// This attribute allows you to create instances of this ScriptableObject via the Assets menu
[CreateAssetMenu(fileName = "NewCraftingRecipe", menuName = "Eco Innovator Lab/Crafting Recipe", order = 1)]
public class CraftingRecipe : ScriptableObject
{
    [Header("Recipe Input Items")]
    // We'll define two input slots for now. Can be expanded to a List for more complex recipes.
    public RecycleItemType input1Type;
    public RecycleItemType input2Type; // Could be None if only 1 input is needed for a specific recipe

    [Header("Recipe Output Item")]
    public GameObject outputPrefab; // Reference to the crafted item's prefab
}