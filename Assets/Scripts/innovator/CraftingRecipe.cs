using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewCraftingRecipe", menuName = "Eco Innovator Lab/Crafting Recipe", order = 1)]
public class CraftingRecipe : ScriptableObject
{
    [Header("Recipe Input Items")]
    // --- CHANGED: Use a List for inputs ---
    public List<RecycleItemType> requiredInputs; // List of item types needed

    [Header("Recipe Output Item")]
    public GameObject outputPrefab;
}