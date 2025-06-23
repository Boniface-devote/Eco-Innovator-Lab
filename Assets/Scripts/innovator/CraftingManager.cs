using UnityEngine;
using UnityEngine.UI; // For Button and Image
using System.Collections.Generic; // For List

public class CraftingManager : MonoBehaviour
{
    [Header("Crafting Slots")]
    public DropZone inputSlot1; // Reference to the DropZone script on CraftingSlot_Input1
    public DropZone inputSlot2; // Reference to the DropZone script on CraftingSlot_Input2
    public Image outputSlot;    // Reference to the Image component of CraftingSlot_Output

    [Header("Crafting Recipes")]
    public List<CraftingRecipe> recipes; // List to hold all your ScriptableObject recipes

    [Header("UI References")]
    public Button craftButton;  // Reference to your CRAFT button

    private void Start()
    {
        if (craftButton != null)
        {
            craftButton.onClick.AddListener(AttemptCraft); // Link button click to AttemptCraft method
        }
        else
        {
            Debug.LogError("Craft Button not assigned in CraftingManager!");
        }
        // Clear output slot on start
        ClearOutputSlot();
    }

    // Call this method when the Craft button is pressed
    public void AttemptCraft()
    {
        Debug.Log("Attempting craft...");

        // Get the DraggableUIItem components currently in the input slots
        DraggableUIItem item1 = GetItemInSlot(inputSlot1);
        DraggableUIItem item2 = GetItemInSlot(inputSlot2);

        // Basic validation: need at least one item
        if (item1 == null && item2 == null)
        {
            Debug.LogWarning("No items in crafting slots.");
            ClearOutputSlot();
            return;
        }

        // Iterate through all defined recipes
        foreach (CraftingRecipe recipe in recipes)
        {
            // Check if the current items match this recipe
            if (MatchRecipe(item1, item2, recipe))
            {
                Debug.Log("Recipe Matched: " + recipe.name);
                CraftItem(recipe.outputPrefab);
                ClearInputSlots();
                return; // Recipe found and crafted, exit
            }
        }

        // If we reached here, no recipe matched
        Debug.Log("No matching recipe found for these items.");
        ClearOutputSlot(); // Ensure output is empty if no match
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

    // Checks if the items in the slots match a given recipe
    private bool MatchRecipe(DraggableUIItem i1, DraggableUIItem i2, CraftingRecipe recipe)
    {
        // This logic assumes order doesn't matter for 2-item recipes
        // It also handles recipes with only one input (input2Type == None)

        bool match1A = (i1 != null && i1.itemType == recipe.input1Type);
        bool match1B = (i2 != null && i2.itemType == recipe.input1Type);

        bool match2A = (i1 != null && i1.itemType == recipe.input2Type);
        bool match2B = (i2 != null && i2.itemType == recipe.input2Type);

        // Case 1: Both inputs of recipe are required and present in some order
        if (recipe.input1Type != RecycleItemType.None && recipe.input2Type != RecycleItemType.None)
        {
            return (match1A && match2B) || (match1B && match2A);
        }
        // Case 2: Only input1 of recipe is required (input2Type is None)
        else if (recipe.input1Type != RecycleItemType.None && recipe.input2Type == RecycleItemType.None)
        {
            // Need exactly one item in the slots that matches input1Type
            return (match1A && i2 == null) || (match1B && i1 == null);
        }
        // Case 3: No inputs defined for recipe (error or special case)
        else
        {
            return false;
        }
    }


    // Instantiates the crafted item prefab into the output slot
    private void CraftItem(GameObject outputPrefab)
    {
        // Clear any existing item in the output slot first
        ClearOutputSlot();

        GameObject craftedItem = Instantiate(outputPrefab, outputSlot.transform);
        craftedItem.name = outputPrefab.name; // Keep prefab name for clarity
        // The DraggableUIItem's SnapToParent method handles RectTransform.zero for layout group parents
        craftedItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
    }

    // Clears items from the input slots
    private void ClearInputSlots()
    {
        if (inputSlot1.transform.childCount > 0)
        {
            Destroy(inputSlot1.transform.GetChild(0).gameObject);
        }
        if (inputSlot2.transform.childCount > 0)
        {
            Destroy(inputSlot2.transform.GetChild(0).gameObject);
        }
    }

    // Clears the output slot
    private void ClearOutputSlot()
    {
        if (outputSlot.transform.childCount > 0)
        {
            Destroy(outputSlot.transform.GetChild(0).gameObject);
        }
        outputSlot.sprite = null; // Clear the image
        outputSlot.color = new Color(outputSlot.color.r, outputSlot.color.g, outputSlot.color.b, 0f); // Make output slot transparent if empty
    }
}