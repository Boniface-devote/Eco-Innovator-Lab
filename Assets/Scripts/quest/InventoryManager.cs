using UnityEngine;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    [Header("Inventory Settings")]
    public Transform inventoryGridContainer; // The parent GameObject that holds all inventory items (e.g., InventoryPanel)
    public List<GameObject> initialInventoryItems; // List of prefabs to populate the inventory with

    private List<GameObject> currentInventoryInstances = new List<GameObject>(); // Keep track of current items in inventory

    private void Start()
    {
        if (inventoryGridContainer == null)
        {
            Debug.LogError("Inventory Grid Container not assigned in InventoryManager!", this);
        }
        RefreshInventory(); // Populate inventory at the start of the game
    }

    public void RefreshInventory()
    {
        Debug.Log("Refreshing Inventory...");

        // 1. Clear all existing items from the inventory grid container
        foreach (Transform child in inventoryGridContainer)
        {
            // Ensure we don't accidentally destroy crafting slots if they somehow became children
            if (child.GetComponent<DraggableUIItem>() != null)
            {
                Destroy(child.gameObject);
            }
        }
        currentInventoryInstances.Clear(); // Clear our tracking list

        // 2. Instantiate new items based on the initialInventoryItems list
        foreach (GameObject itemPrefab in initialInventoryItems)
        {
            if (itemPrefab != null)
            {
                GameObject newItem = Instantiate(itemPrefab, inventoryGridContainer);
                newItem.name = itemPrefab.name; // Keep prefab name for clarity
                // The GridLayoutGroup will handle positioning
                currentInventoryInstances.Add(newItem); // Add to tracking list
            }
        }
        Debug.Log($"Inventory refreshed with {currentInventoryInstances.Count} items.");
    }
}