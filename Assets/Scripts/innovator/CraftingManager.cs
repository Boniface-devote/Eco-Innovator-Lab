using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq; // Required for List.SequenceEqual and OrderBy

public class CraftingManager : MonoBehaviour
{
    [Header("Crafting Slots")]
    // --- CHANGED: Use a List for input slots ---
    public List<DropZone> inputSlots; // Reference to all your CraftingSlot_Prefab instances

    public Image outputSlot;

    [Header("Crafting Recipes")]
    public List<CraftingRecipe> recipes;

    [Header("UI References")]
    public Button craftButton;

    private void Start()
    {
        if (craftButton != null)
        {
            craftButton.onClick.AddListener(AttemptCraft);
        }
        else
        {
            Debug.LogError("Craft Button not assigned in CraftingManager!");
        }
        ClearOutputSlot();
    }

    public void AttemptCraft()
    {
        Debug.Log("Attempting craft...");

        // Get current items in slots
        List<DraggableUIItem> currentItemsInSlots = new List<DraggableUIItem>();
        foreach (DropZone slot in inputSlots)
        {
            DraggableUIItem item = GetItemInSlot(slot);
            if (item != null)
            {
                currentItemsInSlots.Add(item);
            }
        }

        if (currentItemsInSlots.Count == 0)
        {
            Debug.LogWarning("No items in crafting slots.");
            ClearOutputSlot();
            return;
        }

        // Iterate through all defined recipes
        foreach (CraftingRecipe recipe in recipes)
        {
            if (MatchRecipe(currentItemsInSlots, recipe))
            {
                Debug.Log("Recipe Matched: " + recipe.name);
                CraftItem(recipe.outputPrefab);
                ClearInputSlots();
                SoundManager.Instance.PlaySuccess();
                return; // Recipe found and crafted, exit
            }
        }

        // No recipe matched
        Debug.Log("No matching recipe found for these items.");
        SoundManager.Instance.PlayWrong();
        ClearOutputSlot();
    }

    // Helper to get the DraggableUIItem from a DropZone's child
    private DraggableUIItem GetItemInSlot(DropZone slot)
    {
        if (slot.transform.childCount > 0)
        {
            return slot.transform.GetChild(0).GetComponent<DraggableUIItem>();
        }
        return null;
    }

    // --- CHANGED: MatchRecipe now compares Lists ---
    private bool MatchRecipe(List<DraggableUIItem> currentItems, CraftingRecipe recipe)
    {
        // First, check if the number of items matches
        if (currentItems.Count != recipe.requiredInputs.Count)
        {
            return false;
        }

        // Get the item types currently in slots
        List<RecycleItemType> currentItemTypes = currentItems.Select(item => item.itemType).ToList();

        // Sort both lists to make comparison order-agnostic
        currentItemTypes.Sort();
        List<RecycleItemType> requiredTypesSorted = recipe.requiredInputs.OrderBy(type => type).ToList();

        // Compare the sorted lists
        return currentItemTypes.SequenceEqual(requiredTypesSorted);
    }

    private void CraftItem(GameObject outputPrefab)
    {
        ClearOutputSlot();
        GameObject craftedItem = Instantiate(outputPrefab, outputSlot.transform);
        craftedItem.name = outputPrefab.name;
        craftedItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero; // Center it in output slot
        // If the crafted item is draggable, ensure it can be dragged out
        DraggableUIItem draggableOutput = craftedItem.GetComponent<DraggableUIItem>();
        if (draggableOutput != null)
        {
            draggableOutput.originalParent = outputSlot.transform; // Set its original parent to output slot
        }
    }

    // --- CHANGED: ClearInputSlots now iterates through the list ---
    private void ClearInputSlots()
    {
        foreach (DropZone slot in inputSlots)
        {
            if (slot.transform.childCount > 0)
            {
                Destroy(slot.transform.GetChild(0).gameObject);
            }
        }
    }

    private void ClearOutputSlot()
    {
        if (outputSlot.transform.childCount > 0)
        {
            Destroy(outputSlot.transform.GetChild(0).gameObject);
        }
        outputSlot.sprite = null;
        outputSlot.color = new Color(outputSlot.color.r, outputSlot.color.g, outputSlot.color.b, 0f); // Make output slot transparent if empty
    }
}