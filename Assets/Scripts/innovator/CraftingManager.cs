using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq; // Required for List.SequenceEqual and OrderBy
using UnityEngine.Events; // Required for UnityEvent

public class CraftingManager : MonoBehaviour
{
    [Header("Crafting Slots")]
    public List<DropZone> inputSlots; // Reference to all your CraftingSlot_Prefab instances
    public Image outputSlot;

    [Header("Crafting Recipes")]
    public List<CraftingRecipe> recipes;

    [Header("UI References")]
    public Button craftButton;

    // --- Game Managers ---
    [Header("Game Managers")]
    public QuestManager questManager;
    public InventoryManager inventoryManager;

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

        if (questManager == null)
        {
            Debug.LogError("QuestManager is not assigned in CraftingManager!", this);
        }

        if (inventoryManager == null)
        {
            Debug.LogError("InventoryManager is not assigned in CraftingManager!", this);
        }
    }

    public void AttemptCraft()
    {
        Debug.Log("Attempting craft...");

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

        foreach (CraftingRecipe recipe in recipes)
        {
            if (MatchRecipe(currentItemsInSlots, recipe))
            {
                Debug.Log("Recipe Matched: " + recipe.name);
                CraftItem(recipe.outputPrefab);
                ClearInputSlots();
                return;
            }
        }

        Debug.Log("No matching recipe found for these items.");
        ClearOutputSlot();
    }

    private DraggableUIItem GetItemInSlot(DropZone slot)
    {
        if (slot.transform.childCount > 0)
        {
            return slot.transform.GetChild(0).GetComponent<DraggableUIItem>();
        }
        return null;
    }

    private bool MatchRecipe(List<DraggableUIItem> currentItems, CraftingRecipe recipe)
    {
        if (currentItems.Count != recipe.requiredInputs.Count)
            return false;

        List<RecycleItemType> currentItemTypes = currentItems.Select(item => item.itemType).ToList();
        currentItemTypes.Sort();
        List<RecycleItemType> requiredTypesSorted = recipe.requiredInputs.OrderBy(type => type).ToList();

        return currentItemTypes.SequenceEqual(requiredTypesSorted);
    }

    private void CraftItem(GameObject outputPrefab)
    {
        ClearOutputSlot();

        GameObject craftedItem = Instantiate(outputPrefab, outputSlot.transform);
        craftedItem.name = outputPrefab.name;
        craftedItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

        DraggableUIItem draggableOutput = craftedItem.GetComponent<DraggableUIItem>();
        if (draggableOutput != null)
        {
            draggableOutput.originalParent = outputSlot.transform;
        }

        // Notify QuestManager after successful craft
        if (questManager != null)
        {
            questManager.OnItemCrafted(outputPrefab);
        }

        // Refresh Inventory
        if (inventoryManager != null)
        {
            inventoryManager.RefreshInventory();
        }

        // Remove crafted item after 5 seconds
        StartCoroutine(RemoveCraftedItemAfterSeconds(craftedItem, 5f));
    }

    private IEnumerator RemoveCraftedItemAfterSeconds(GameObject item, float seconds)
    {
        yield return new WaitForSeconds(seconds);

        if (item != null)
        {
            Destroy(item);
            ClearOutputSlot();
        }
    }

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
        outputSlot.color = new Color(outputSlot.color.r, outputSlot.color.g, outputSlot.color.b, 0f);
    }
}
